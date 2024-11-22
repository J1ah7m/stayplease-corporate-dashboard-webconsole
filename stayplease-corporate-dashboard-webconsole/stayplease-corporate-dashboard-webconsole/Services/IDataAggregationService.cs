namespace stayplease_corporate_dashboard_webconsole;

public interface IDataAggregationService
{
    Task<List<TaskItemModel>> ProcessingTaskItemAsync(HotelConfig hotel, DateTime startDate, DateTime endDate, string notcompletedIDs = "", bool useAutoBackup = false);

    //List<HotelConfig> GetHotels();
}
