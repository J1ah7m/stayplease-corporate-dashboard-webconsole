using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public async Task CoordinateTaskProcessingAsync(HotelConfig hotel, DateTime startDate, DateTime endDate,  bool useAutoBackup)
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
        var notCompletedTaskItems = await _corporateDashboardService.GetNotCompletedTaskItemAsync(hotel);
        var notcompletedIDs = string.Join(",", notCompletedTaskItems.Select(x => x.MissionID).Distinct().ToList());

        var taskItems = await _dataAggregationService.ProcessingTaskItemAsync(hotel, DateTime.Now, DateTime.Now, notcompletedIDs, false);
        await _corporateDashboardService.WriteOrUpdateDataInTaskItem(hotel, taskItems, DateTime.Now, DateTime.Now, false, false);

        var taskItems_backup = await _dataAggregationService.ProcessingTaskItemAsync(hotel, DateTime.Now, DateTime.Now, notcompletedIDs, true);
        await _corporateDashboardService.WriteOrUpdateDataInTaskItem(hotel, taskItems_backup, DateTime.Now, DateTime.Now, false, true);

    }

    public List<HotelConfig> GetHotels() 
    {
        return _hotelConfiguration.Hotels;
    }
}
