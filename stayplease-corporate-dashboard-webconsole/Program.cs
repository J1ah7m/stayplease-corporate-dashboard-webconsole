using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using stayplease_corporate_dashboard_webconsole.Services;

namespace stayplease_corporate_dashboard_webconsole
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var scheduler = serviceProvider.GetRequiredService<DataAggregationScheduler>();
            await scheduler.StartAsync(CancellationToken.None);

            Console.WriteLine("Task scheduler is running...");
            Console.WriteLine("Type 'exit' to quit the program or enter other commands to continue...");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Stopping task scheduler...");
                    await scheduler.StopAsync(CancellationToken.None);
                    Console.WriteLine("Task scheduler stopped. Exiting program...");
                    break;
                }
                else
                {
                    await HandleCommandAsync(input, serviceProvider);
                }
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<DataAggregationScheduler>();
            services.AddSingleton<HotelConfiguration>();
            services.AddSingleton<IDataAggregationService, DataAggregationService>();
            services.AddSingleton<ICorporateDashboardService, CorporateDashboardService>();
            services.AddSingleton<IAggregationCoordinatorService, AggregationCoordinatorService>();
        }

        private static async Task HandleCommandAsync(string command, IServiceProvider serviceProvider)
        {
            var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
            {
                Console.WriteLine("Invalid command.");
                return;
            }

            var action = parts[0].ToLower();
            switch (action)
            {
                case "resync":
                    if (parts.Length < 3)
                    {
                        Console.WriteLine("Invalid command format. Correct format: resync <HotelID> <Date>");
                        return;
                    }

                    var hotelIdStrings = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var hotelIds = new List<int>();
                    foreach (var hotelIdString in hotelIdStrings)
                    {
                        if (!int.TryParse(hotelIdString, out var hotelId))
                        {
                            Console.WriteLine($"Invalid Hotel ID: {hotelIdString}");
                            return;
                        }
                        hotelIds.Add(hotelId);
                    }

                    if (!DateTime.TryParse(parts[2], out var startDate))
                    {
                        Console.WriteLine("Invalid date format.");
                        return;
                    }

                    DateTime? endDate = null;
                    if (parts.Length == 4)
                    {
                        if (!DateTime.TryParse(parts[3], out var parsedEndDate))
                        {
                            Console.WriteLine("Invalid end date format.");
                            return;
                        }
                        endDate = parsedEndDate;
                    }

                    await HandleResyncCommand(serviceProvider, hotelIds, startDate, endDate);
                    break;

                default:
                    Console.WriteLine("Unknown command. Please try again.");
                    break;
            }
        }

        private static async Task HandleResyncCommand(IServiceProvider serviceProvider, List<int> hotelIds, DateTime startDate, DateTime? endDate)
        {
            var _aggregationCoordinatorService = serviceProvider.GetRequiredService<IAggregationCoordinatorService>();
            var _dataAggregationService = serviceProvider.GetRequiredService<IDataAggregationService>();
            var _corporateDashboardService = serviceProvider.GetRequiredService<ICorporateDashboardService>();

            var hotels = _aggregationCoordinatorService.GetHotels();

            foreach (var hotelId in hotelIds)
            {
                var hotel = hotels.Find(h => h.HotelID == hotelId);

                if (hotel == null)
                {
                    Console.WriteLine($"Hotel with ID {hotelId} not found.");
                    return;
                }

                var _startDate = startDate.Date;
                var _endDate = startDate.AddDays(1).Date;
                if (endDate.HasValue)
                {
                    _endDate = endDate.Value.AddDays(1).Date;
                    Console.WriteLine($"Resyncing data for hotel: {hotel.HotelName} from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}...");
                }
                else
                {
                    Console.WriteLine($"Resyncing data for hotel: {hotel.HotelName} for {startDate:yyyy-MM-dd}...");
                }
                var taskItems = await _dataAggregationService.ProcessingTaskItemAsync(hotel, _startDate, _endDate);
                var taskItems_backup = await _dataAggregationService.ProcessingTaskItemAsync(hotel, _startDate, _endDate, "", true);

                taskItems.AddRange(taskItems_backup);
                await _corporateDashboardService.WriteOrUpdateDataInTaskItem(hotel, taskItems, _startDate, _endDate, true, false);

                await Task.Delay(100);
                Console.WriteLine($"Resync completed for hotel {hotel.HotelName}.");
                Console.WriteLine("");
            }
                

            //var corporateDashboardService = serviceProvider.GetRequiredService<ICorporateDashboardService>();
            //await corporateDashboardService.WriteOrUpdateDataInTaskItem(hotel, new List<TaskItemModel>(), date, date, true, false);


        }
    }
}
