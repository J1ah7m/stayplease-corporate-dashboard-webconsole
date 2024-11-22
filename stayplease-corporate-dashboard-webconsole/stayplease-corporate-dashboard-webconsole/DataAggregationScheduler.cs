using stayplease_corporate_dashboard_webconsole.Services;

namespace stayplease_corporate_dashboard_webconsole;

public class DataAggregationScheduler
{
    private readonly IAggregationCoordinatorService _aggregationService;
    private readonly Dictionary<int, DateTime> _processedHotels = new();
    private Timer _timer;

    public DataAggregationScheduler(IAggregationCoordinatorService aggregationService)
    {
        _aggregationService = aggregationService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting task scheduler...");
        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private async void ExecuteTask(object state)
    {
        try
        {
            var hotels = _aggregationService.GetHotels();

            foreach (var hotel in hotels)
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(hotel.TimeZone));

                if (localTime.Hour == 14 && (!_processedHotels.ContainsKey(hotel.HotelID) || _processedHotels[hotel.HotelID].Date != localTime.Date))
                {
                    Console.WriteLine($"Processing data for hotel: {hotel.HotelName}...");

                    var startDate = localTime.AddDays(-1).Date;
                    var endDate = localTime.Date;

                    var taskItemsHasData = await _aggregationService.HasDataInsertedToday(hotel, startDate, "TaskItems");
                    if (!taskItemsHasData)
                        await _aggregationService.CoordinateTaskProcessingAsync(hotel, startDate, endDate, false);

                    await _aggregationService.CoordinateNotCompletedTaskProcessingAsync(hotel);

                    _processedHotels[hotel.HotelID] = localTime;

                    Console.WriteLine($"Data processing completed for hotel: {hotel.HotelName}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Task execution failed: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping task scheduler...");
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
        return Task.CompletedTask;
    }
}
