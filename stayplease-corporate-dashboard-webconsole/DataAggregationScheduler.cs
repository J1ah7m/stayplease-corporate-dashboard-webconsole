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
        _timer = new Timer(ExecuteTask, null, Timeout.Infinite, Timeout.Infinite);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting task scheduler...");
        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private async void ExecuteTask(object? state)
    {
        try
        {
            var hotels = _aggregationService.GetHotels();

            foreach (var hotel in hotels)
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(hotel.TimeZone ?? ""));

                if (localTime.Hour == 0 && (!_processedHotels.ContainsKey(hotel.HotelID) || _processedHotels[hotel.HotelID].Date != localTime.Date))
                {
                    var startDate = localTime.AddDays(-1).Date;
                    var endDate = localTime.Date;

                    Console.WriteLine("=======================================");
                    Console.WriteLine($"Processing data for hotel: {hotel.HotelName}... date: {startDate:yyyy-MM-dd HH:mm}");

                    var taskItemsHasData = await _aggregationService.HasDataInsertedToday(hotel, startDate, "TaskItems");
                    if (!taskItemsHasData)
                    {
                        Console.WriteLine($"Starting task item processing for hotel: {hotel.HotelName}, date range: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}. [{DateTime.UtcNow.AddHours(8):yyyy-MM-dd HH:mm:ss}]");
                        await _aggregationService.CoordinateTaskProcessingAsync(hotel, startDate, endDate, false);
                    }
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    await _aggregationService.CoordinateNotCompletedTaskProcessingAsync(hotel);

                    _processedHotels[hotel.HotelID] = localTime;

                    Console.WriteLine($"Data processing completed for hotel: {hotel.HotelName}. [{DateTime.UtcNow.AddHours(8):yyyy-MM-dd HH:mm:ss}]");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"**Task execution failed: {ex.Message}");
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
