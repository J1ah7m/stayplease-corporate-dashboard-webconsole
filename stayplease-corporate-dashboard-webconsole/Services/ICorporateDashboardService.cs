namespace stayplease_corporate_dashboard_webconsole;

public interface ICorporateDashboardService
{
     Task WriteOrUpdateDataInTaskItem(HotelConfig hotel, List<TaskItemModel> data,
       DateTime? startDate, DateTime? endDate, bool reInsert = false, bool isUpdate = false);

    Task InsertSyncLog(SyncLogModel log);

    Task<bool> HasDataInsertedToday(HotelConfig hotel, DateTime syncDate, string syncTableName);

    Task<List<TaskItemModel>> GetNotCompletedTaskItemAsync(HotelConfig hotel);
}
