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

                    if (!int.TryParse(parts[1], out var hotelId))
                    {
                        Console.WriteLine("Invalid Hotel ID.");
                        return;
                    }

                    if (!DateTime.TryParse(parts[2], out var date))
                    {
                        Console.WriteLine("Invalid date format.");
                        return;
                    }

                    await HandleResyncCommand(serviceProvider, hotelId, date);
                    break;

                default:
                    Console.WriteLine("Unknown command. Please try again.");
                    break;
            }
        }

        private static async Task HandleResyncCommand(IServiceProvider serviceProvider, int hotelId, DateTime date)
        {
            var _aggregationCoordinatorService = serviceProvider.GetRequiredService<IAggregationCoordinatorService>();
            var hotels = _aggregationCoordinatorService.GetHotels();
            var hotel = hotels.Find(h => h.HotelID == hotelId);

            if (hotel == null)
            {
                Console.WriteLine($"Hotel with ID {hotelId} not found.");
                return;
            }

            Console.WriteLine($"Resyncing data for hotel {hotel.HotelName} on {date:yyyy-MM-dd}...");

            var startDate = date.Date;
            var endDate = date.AddDays(1).Date;

            await _aggregationCoordinatorService.CoordinateNotCompletedTaskProcessingAsync(hotel);

            //var corporateDashboardService = serviceProvider.GetRequiredService<ICorporateDashboardService>();
            //await corporateDashboardService.WriteOrUpdateDataInTaskItem(hotel, new List<TaskItemModel>(), date, date, true, false);

            Console.WriteLine($"Resync completed for hotel {hotel.HotelName}.");
        }
    }
}
