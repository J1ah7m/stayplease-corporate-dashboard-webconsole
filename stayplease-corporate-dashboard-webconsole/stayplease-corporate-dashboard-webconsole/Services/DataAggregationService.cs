using Dapper;
using MySql.Data.MySqlClient;
namespace stayplease_corporate_dashboard_webconsole;

public class DataAggregationService : IDataAggregationService
{
    public async Task<List<TaskItemModel>> ProcessingTaskItemAsync(HotelConfig hotel, DateTime startDate, DateTime endDate, string notcompletedIDs = "", bool useAutoBackup = false)
    {
        List<TaskItemModel> taskItemList = new();

        try
        {
            Console.WriteLine($"Starting task item processing for hotel: {hotel.HotelName}, date range: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}.");

            var roomStatusList = await GetRoomStatusAsync(hotel);

            taskItemList = await GetTaskItemListAsync(hotel, startDate, endDate, notcompletedIDs, useAutoBackup);

            var IDs = string.Join(",", taskItemList.Select(x => x.MissionID).Distinct().ToList());

            var taskLocationStatusLogList = await GetTaskLocationStatusLogAsync(hotel, IDs, useAutoBackup);

            var taskItemLookup = taskItemList.GroupBy(x => x.ParentID).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var taskItem in taskItemList)
            {
                string todoRoomStatus = "", todoReservStatus = "", todoServiceStatus = "",
                       doingRoomStatus = "", doingReservStatus = "", doingServiceStatus = "",
                       doneRoomStatus = "", doneReservStatus = "", doneServiceStatus = "";

                if (taskLocationStatusLogList.TryGetValue(taskItem.MissionID, out var taskLocationStatusLog))
                {
                    var statusTypes = new[] { 1, 5, 10 };
                    var statusResults = statusTypes.Select(status => taskLocationStatusLog.AsEnumerable()
                        .Reverse()
                        .FirstOrDefault(x => x.TaskStatus == status))
                        .Select(status => GetStatusValues(status))
                        .ToList();

                    (todoRoomStatus, todoReservStatus, todoServiceStatus) = statusResults[0];
                    (doingRoomStatus, doingReservStatus, doingServiceStatus) = statusResults[1];
                    (doneRoomStatus, doneReservStatus, doneServiceStatus) = statusResults[2];

                    var startRoomStatus = doingRoomStatus ?? todoRoomStatus;

                    if (roomStatusList.TryGetValue(startRoomStatus, out var roomStatusModel))
                    {
                        var hasInspection = taskItem.TaskType == 1 &&
                                            taskItemLookup.TryGetValue(taskItem.MissionID, out var subTasks) &&
                                            subTasks.Any(x => x.TaskType == 3 || x.TaskType == 4);

                        doneRoomStatus = !hasInspection && taskItem.TaskStatus == 15
                            ? roomStatusModel.SelfCheckStatus
                            : roomStatusModel.AfterCleanStatusName;
                    }
                }

                taskItem.RoomStatus_ToDo = todoRoomStatus;
                taskItem.ReservStatus_ToDo = todoReservStatus;
                taskItem.ServiceStatus_ToDo = todoServiceStatus;

                taskItem.RoomStatus_Doing = doingRoomStatus;
                taskItem.ReservStatus_Doing = doingReservStatus;
                taskItem.ServiceStatus_Doing = doingServiceStatus;

                taskItem.RoomStatus_Done = doneRoomStatus;
                taskItem.ReservStatus_Done = doneReservStatus;
                taskItem.ServiceStatus_Done = doneServiceStatus;
            }

            (string roomStatus, string reservStatus, string serviceStatus) GetStatusValues(TaskLocationStatusLogModel status)
            {
                return (status?.RoomStatus ?? "", status?.ReservStatus ?? "", status?.ServiceStatus ?? "");
            }

            Console.WriteLine($"Task item processing completed successfully for hotel: {hotel.HotelName}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing task items for hotel {hotel.HotelName}: {ex.Message}");
        }

        return taskItemList;
    }

    private async Task<List<TaskItemModel>> GetTaskItemListAsync(HotelConfig hotel, DateTime startDate, DateTime endDate, string notcompletedIDs = "", bool useAutoBackup = false)
    {
        try
        {
            Console.WriteLine($"{(string.IsNullOrEmpty(notcompletedIDs) ? "" : "Update Not Completed Task")} Fetching task items for hotel: {hotel.HotelName}");
            var queryConditions = " AND STT.ToDoTime BETWEEN @StartDate AND @EndDate ";
            if (!string.IsNullOrEmpty(notcompletedIDs))
                queryConditions = " AND STI.ID IN ('" + notcompletedIDs.Replace(",", "','") + "') ";

            var taskItems = await ExecuteQueryAsync<TaskItemModel>(hotel.ConnectionString, Queries.QueryTaskItemList,
                new
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    TenantId = hotel.HotelID,
                    TimeZone = GetTimeZoneOffset(hotel.TimeZone)
                }, useAutoBackup, queryConditions);

            Console.WriteLine($"Fetched {taskItems.Count} task items for hotel: {hotel.HotelName}.");
            return taskItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching task items for hotel {hotel.HotelName}: {ex.Message}");
            return new List<TaskItemModel>();
        }
    }

    private async Task<Dictionary<long, List<TaskLocationStatusLogModel>>> GetTaskLocationStatusLogAsync(HotelConfig hotel, string IDs, bool useAutoBackup = false)
    {
        try
        {
            Console.WriteLine($"Fetching task location status logs for hotel: {hotel.HotelName}.");
            var queryConditions = " AND TaskId IN ('" + IDs.Replace(",", "','") + "') ";

            var logs = (await ExecuteQueryAsync<TaskLocationStatusLogModel>(hotel.ConnectionString, Queries.QueryTaskLocationStatusLogs, "", useAutoBackup, queryConditions))
                .GroupBy(x => x.TaskID)
                .ToDictionary(group => group.Key, group => group.OrderBy(x => x.CreationTime).ToList());

            Console.WriteLine($"Fetched {logs.Count} task location status logs for hotel: {hotel.HotelName}.");
            return logs;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching task location status logs for hotel {hotel.HotelName}: {ex.Message}");
            return new Dictionary<long, List<TaskLocationStatusLogModel>>();
        }
    }

    private async Task<Dictionary<string, RoomStatusModel>> GetRoomStatusAsync(HotelConfig hotel, bool useAutoBackup = false)
    {
        try
        {
            Console.WriteLine($"Fetching room statuses for hotel: {hotel.HotelName}.");
            var sql = "SELECT RoomStatusName,SelfCheckStatus, AfterCleanStatusName, AfterCheckStatusName FROM sproomstatus WHERE RoomStatusType = 1;";
            var roomStatuses = await ExecuteQueryAsync<RoomStatusModel>(hotel.ConnectionString, sql, "", useAutoBackup);

            var result = roomStatuses.ToDictionary(item => item.RoomStatusName, item => item);
            Console.WriteLine($"Fetched {result.Count} room statuses for hotel: {hotel.HotelName}.");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching room statuses for hotel {hotel.HotelName}: {ex.Message}");
            return new Dictionary<string, RoomStatusModel>();
        }
    }

    private async Task<List<T>> ExecuteQueryAsync<T>(string connectionString, string query, object parameters, bool useAutoBackup = false, string queryConditions = "")
    {
        try
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var mappedQuery = ReplaceTableNames(query, useAutoBackup, queryConditions);
            var results = (await connection.QueryAsync<T>(mappedQuery, parameters)).ToList();

            Console.WriteLine($"Query executed successfully. Retrieved {results.Count} records.");
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing query: {ex.Message}");
            return new List<T>();
        }
    }

    private string ReplaceTableNames(string sql, bool useAutoBackup, string queryConditions)
    {
        var tableMappings = new Dictionary<string, string>
        {
            { "TaskItems", "sptaskitems" + (useAutoBackup ? "_auto_backup" : "") },
            { "TaskTimers", "sptasktimers" + (useAutoBackup ? "_auto_backup" : "") },
            { "MenuOrders", "spmenuorders" + (useAutoBackup ? "_auto_backup" : "") },
            { "TaskLocationStatusLogs", "sptasklocationstatuslogs" + (useAutoBackup ? "_auto_backup" : "") },
            { "TaskTimeLines", "sptasktimelines" + (useAutoBackup ? "_auto_backup" : "") }
        };

        foreach (var mapping in tableMappings)
        {
            sql = sql.Replace($"{{{mapping.Key}_TableName}}", mapping.Value);
        }

        sql = sql.Replace("{QueryConditions}", queryConditions);
        return sql;
    }

    private int GetTimeZoneOffset(string timeZoneId)
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return (int)timeZoneInfo.BaseUtcOffset.TotalMinutes;
    }
}
