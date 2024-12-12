namespace stayplease_corporate_dashboard_webconsole.Services;

public class AggregationCoordinatorService : IAggregationCoordinatorService
{
    private readonly IDataAggregationService _dataAggregationService;
    private readonly ICorporateDashboardService _corporateDashboardService;
    private readonly HotelConfiguration _hotelConfiguration;

    public AggregationCoordinatorService(
        IDataAggregationService dataAggregationService,
        ICorporateDashboardService corporateDashboardService,HotelConfiguration hotelConfiguration)
    {
        _dataAggregationService = dataAggregationService;
        _corporateDashboardService = corporateDashboardService;
        _hotelConfiguration = hotelConfiguration;
    }

    public async Task CoordinateTaskProcessingAsync(HotelConfig hotel, DateTime startDate, DateTime endDate, bool useAutoBackup)
    {
        var taskItems = await _dataAggregationService.ProcessingTaskItemAsync(hotel, startDate, endDate, "", useAutoBackup);

        await _corporateDashboardService.WriteOrUpdateDataInTaskItem(hotel, taskItems, startDate, endDate, false, useAutoBackup);
    }

    public async Task<bool> HasDataInsertedToday(HotelConfig hotel, DateTime syncDate, string syncTableName)
    {
        return await _corporateDashboardService.HasDataInsertedToday(hotel, syncDate, syncTableName);   
    }

    public async Task CoordinateNotCompletedTaskProcessingAsync(HotelConfig hotel)
    {
        Console.WriteLine($"Starting not completed task item processing for hotel: {hotel.HotelName}");

        var notCompletedTaskItems = await _corporateDashboardService.GetNotCompletedTaskItemAsync(hotel);
        var notcompletedIDs = string.Join(",", notCompletedTaskItems.Select(x => x.MissionID).Distinct().ToList());
        var count = 0;
        if (!string.IsNullOrEmpty(notcompletedIDs))
        {
            var taskItems = await _dataAggregationService.ProcessingTaskItemAsync(hotel, DateTime.Now, DateTime.Now, notcompletedIDs, false, true);
            var taskItems_backup = await _dataAggregationService.ProcessingTaskItemAsync(hotel, DateTime.Now, DateTime.Now, notcompletedIDs, true, true);

            taskItems.AddRange(taskItems_backup);

            await _corporateDashboardService.WriteOrUpdateDataInTaskItem(hotel, taskItems, DateTime.Now, DateTime.Now, false, true);

            count = taskItems.Count;
        }

        Console.WriteLine($"Not completed Task item processing completed successfully for hotel: {hotel.HotelName}. Records processed: {count}");
    }

    public List<HotelConfig> GetHotels() 
    {
        return _hotelConfiguration.Hotels;
    }
}
