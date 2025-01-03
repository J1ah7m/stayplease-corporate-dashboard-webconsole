﻿using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace stayplease_corporate_dashboard_webconsole;

public class CorporateDashboardService : ICorporateDashboardService
{
    private readonly string _connectionString;

    public CorporateDashboardService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? "";
    }

    #region Insert/Update TaskItems
    public async Task WriteOrUpdateDataInTaskItem(HotelConfig hotel, List<TaskItemModel> data,
      DateTime? startDate, DateTime? endDate, bool reInsert = false, bool isUpdate = false)
    {
        var operation = $"{(reInsert ? "Re-Insert" : (isUpdate ? "Update" : "Insert"))}";
        if (isUpdate)
        {
            operation += " with Backup";
        }

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            Console.WriteLine($"{operation} started for hotel: {hotel.HotelName}");

            var successCount = 0;

            foreach (var batch in data.Chunk(500))
            {
                try
                {
                    var insertQuery = $@"
                    INSERT INTO TaskItems_{hotel.GroupName} 
                    (
                        TaskCategoryID, TaskCategory, MissionID, TaskID, LocationID, LocationName, 
                        TaskName, TagName, Quantity, CreateTime, ToDoTime, DoingTime, DoneTime, 
                        PauseTime, TaskStage, TimeLimit, TaskType, ParentID, FirstDoingTime, 
                        ScheduleTime, IsGuestRequest, ZoneID, ZoneName, LocationType, TaskStatus, 
                        CreateUserId, CreateUserName, DoneUserId, DoneUserName, IsUrgent, 
                        IsComplaint, PauseReason, RoomStatus_ToDo, ReservStatus_ToDo, 
                        ServiceStatus_ToDo, RoomStatus_Doing, ReservStatus_Doing, 
                        ServiceStatus_Doing, RoomStatus_Done, ReservStatus_Done, 
                        ServiceStatus_Done, HotelName, HotelID, BelongTo, IsDeleted
                    ) 
                    VALUES 
                    (
                        @TaskCategoryID, @TaskCategory, @MissionID, @TaskID, @LocationID, @LocationName, 
                        @TaskName, @TagName, @Quantity, @CreateTime, @ToDoTime, @DoingTime, @DoneTime, 
                        @PauseTime, @TaskStage, @TimeLimit, @TaskType, @ParentID, @FirstDoingTime, 
                        @ScheduleTime, @IsGuestRequest, @ZoneID, @ZoneName, @LocationType, @TaskStatus, 
                        @CreateUserId, @CreateUserName, @DoneUserId, @DoneUserName, @IsUrgent, 
                        @IsComplaint, @PauseReason, @RoomStatus_ToDo, @ReservStatus_ToDo, 
                        @ServiceStatus_ToDo, @RoomStatus_Doing, @ReservStatus_Doing, 
                        @ServiceStatus_Doing, @RoomStatus_Done, @ReservStatus_Done, 
                        @ServiceStatus_Done, @HotelName, @HotelID,  @BelongTo,  @IsDeleted
                    )
                    ON DUPLICATE KEY UPDATE 
                        TaskCategoryID = VALUES(TaskCategoryID), 
                        TaskCategory = VALUES(TaskCategory), 
                        TaskID = VALUES(TaskID), 
                        LocationID = VALUES(LocationID), 
                        LocationName = VALUES(LocationName), 
                        TaskName = VALUES(TaskName), 
                        TagName = VALUES(TagName), 
                        Quantity = VALUES(Quantity), 
                        CreateTime = VALUES(CreateTime), 
                        ToDoTime = VALUES(ToDoTime), 
                        DoingTime = VALUES(DoingTime), 
                        DoneTime = VALUES(DoneTime), 
                        PauseTime = VALUES(PauseTime), 
                        TaskStage = VALUES(TaskStage), 
                        TimeLimit = VALUES(TimeLimit), 
                        TaskType = VALUES(TaskType), 
                        ParentID = VALUES(ParentID), 
                        FirstDoingTime = VALUES(FirstDoingTime), 
                        ScheduleTime = VALUES(ScheduleTime), 
                        IsGuestRequest = VALUES(IsGuestRequest), 
                        ZoneID = VALUES(ZoneID), 
                        ZoneName = VALUES(ZoneName), 
                        LocationType = VALUES(LocationType), 
                        TaskStatus = VALUES(TaskStatus), 
                        CreateUserId = VALUES(CreateUserId), 
                        CreateUserName = VALUES(CreateUserName), 
                        DoneUserId = VALUES(DoneUserId), 
                        DoneUserName = VALUES(DoneUserName), 
                        IsUrgent = VALUES(IsUrgent), 
                        IsComplaint = VALUES(IsComplaint), 
                        PauseReason = VALUES(PauseReason), 
                        RoomStatus_ToDo = VALUES(RoomStatus_ToDo), 
                        ReservStatus_ToDo = VALUES(ReservStatus_ToDo), 
                        ServiceStatus_ToDo = VALUES(ServiceStatus_ToDo), 
                        RoomStatus_Doing = VALUES(RoomStatus_Doing), 
                        ReservStatus_Doing = VALUES(ReservStatus_Doing), 
                        ServiceStatus_Doing = VALUES(ServiceStatus_Doing), 
                        RoomStatus_Done = VALUES(RoomStatus_Done), 
                        ReservStatus_Done = VALUES(ReservStatus_Done), 
                        ServiceStatus_Done = VALUES(ServiceStatus_Done),
                        BelongTo = VALUES(BelongTo),
                        IsDeleted = VALUES(IsDeleted)";

                    using var insertCommand = new MySqlCommand(insertQuery, connection, transaction);

                    foreach (var item in batch)
                    {
                        insertCommand.Parameters.Clear();
                        insertCommand.Parameters.AddWithValue("@TaskCategoryID", item.TaskCategoryID);
                        insertCommand.Parameters.AddWithValue("@TaskCategory", item.TaskCategory);
                        insertCommand.Parameters.AddWithValue("@MissionID", item.MissionID);
                        insertCommand.Parameters.AddWithValue("@TaskID", item.TaskID);
                        insertCommand.Parameters.AddWithValue("@LocationID", item.LocationID);
                        insertCommand.Parameters.AddWithValue("@LocationName", item.LocationName);
                        insertCommand.Parameters.AddWithValue("@TaskName", item.TaskName);
                        insertCommand.Parameters.AddWithValue("@TagName", item.TagName);
                        insertCommand.Parameters.AddWithValue("@Quantity", item.Quantity == 0 ? 1 : item.Quantity);
                        insertCommand.Parameters.AddWithValue("@CreateTime", item.CreateTime);
                        insertCommand.Parameters.AddWithValue("@ToDoTime", item.ToDoTime);
                        insertCommand.Parameters.AddWithValue("@DoingTime", item.DoingTime ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@DoneTime", item.DoneTime ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@PauseTime", item.PauseTime ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@TaskStage", item.TaskStage);
                        insertCommand.Parameters.AddWithValue("@TimeLimit", item.TimeLimit);
                        insertCommand.Parameters.AddWithValue("@TaskType", item.TaskType);
                        insertCommand.Parameters.AddWithValue("@ParentID", item.ParentID);
                        insertCommand.Parameters.AddWithValue("@FirstDoingTime", item.FirstDoingTime ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ScheduleTime", item.ScheduleTime ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@IsGuestRequest", item.IsGuestRequest);
                        insertCommand.Parameters.AddWithValue("@ZoneID", item.ZoneID);
                        insertCommand.Parameters.AddWithValue("@ZoneName", item.ZoneName);
                        insertCommand.Parameters.AddWithValue("@LocationType", item.LocationType);
                        insertCommand.Parameters.AddWithValue("@TaskStatus", item.TaskStatus);
                        insertCommand.Parameters.AddWithValue("@CreateUserId", item.CreateUserId);
                        insertCommand.Parameters.AddWithValue("@CreateUserName", item.CreateUserName);
                        insertCommand.Parameters.AddWithValue("@DoneUserId", item.DoneUserId);
                        insertCommand.Parameters.AddWithValue("@DoneUserName", item.DoneUserName);
                        insertCommand.Parameters.AddWithValue("@IsUrgent", item.IsUrgent);
                        insertCommand.Parameters.AddWithValue("@IsComplaint", item.IsComplaint);
                        insertCommand.Parameters.AddWithValue("@PauseReason", item.PauseReason ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@RoomStatus_ToDo", item.RoomStatus_ToDo ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ReservStatus_ToDo", item.ReservStatus_ToDo ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ServiceStatus_ToDo", item.ServiceStatus_ToDo ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@RoomStatus_Doing", item.RoomStatus_Doing ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ReservStatus_Doing", item.ReservStatus_Doing ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ServiceStatus_Doing", item.ServiceStatus_Doing ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@RoomStatus_Done", item.RoomStatus_Done ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ReservStatus_Done", item.ReservStatus_Done ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@ServiceStatus_Done", item.ServiceStatus_Done ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@HotelName", hotel.HotelName);
                        insertCommand.Parameters.AddWithValue("@HotelID", hotel.HotelID);
                        insertCommand.Parameters.AddWithValue("@BelongTo", item.BelongTo ?? (object)DBNull.Value);
                        insertCommand.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);

                        await insertCommand.ExecuteNonQueryAsync();
                        successCount++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"**Error inserting batch: {ex.Message}");
                }
            }

            await transaction.CommitAsync();

            Console.WriteLine($"{operation} completed successfully for hotel: {hotel.HotelName}. Records processed: {successCount}");

            if (operation == "Insert")
            {
                await InsertSyncLog(new SyncLogModel
                {
                    HotelID = hotel.HotelID,
                    SyncTable = "TaskItems",
                    SyncDate = startDate.Value.ToString("yyyy-MM-dd"),
                    SyncStatus = "Success",
                    ErrorMsg = null
                });
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"**Error during {operation} for hotel {hotel.HotelName}: {ex.Message}");
            try
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Transaction rolled back for hotel: {hotel.HotelName}");
            }
            catch (Exception rollbackEx)
            {
                Console.WriteLine($"**Rollback failed for hotel {hotel.HotelName}: {rollbackEx.Message}");
            }
            throw;
        }
    }

    #endregion

    private async Task InsertSyncLog(SyncLogModel log)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
            INSERT INTO SyncLogs (HotelID, SyncTable, SyncDate, SyncStatus, ErrorMsg)
            VALUES (@HotelID, @SyncTable, @SyncDate, @SyncStatus, @ErrorMsg)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@HotelID", log.HotelID);
            command.Parameters.AddWithValue("@SyncTable", log.SyncTable ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SyncDate", log.SyncDate);
            command.Parameters.AddWithValue("@SyncStatus", log.SyncStatus);
            command.Parameters.AddWithValue("@ErrorMsg", log.ErrorMsg ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"**Error inserting sync log for hotel ID: {log.HotelID}: {ex.Message}");
        }
    }

    public async Task<bool> HasDataInsertedToday(HotelConfig hotel, DateTime syncDate, string syncTableName)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(1) FROM SyncLogs WHERE SyncDate = @SyncDate AND HotelID = @HotelID AND SyncTable = @SyncTable;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@SyncDate", syncDate);
            command.Parameters.AddWithValue("@SyncTable", syncTableName);
            command.Parameters.AddWithValue("@HotelID", hotel.HotelID);

            var result = await command.ExecuteScalarAsync();
            long? count = result as long?;

            return count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"**Error checking data insertion for hotel {hotel.HotelName}: {ex.Message}");
            return false;
        }
    }

    public async Task<List<TaskItemModel>> GetNotCompletedTaskItemAsync(HotelConfig hotel)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = $@"
            SELECT 
                TaskCategoryID, TaskCategory, MissionID, TaskID, LocationID, LocationName, 
                TaskName, TagName, Quantity, CreateTime, ToDoTime, DoingTime, DoneTime, 
                PauseTime, TaskStage, TimeLimit, TaskType, ParentID, FirstDoingTime, 
                ScheduleTime, IsGuestRequest, ZoneID, ZoneName, LocationType, TaskStatus, 
                CreateUserId, CreateUserName, DoneUserId, DoneUserName, IsUrgent, 
                IsComplaint, PauseReason, RoomStatus_ToDo, ReservStatus_ToDo, 
                ServiceStatus_ToDo, RoomStatus_Doing, ReservStatus_Doing, 
                ServiceStatus_Doing, RoomStatus_Done, ReservStatus_Done, 
                ServiceStatus_Done, HotelName, HotelID,BelongTo
            FROM TaskItems_{hotel.GroupName}
            WHERE HotelID = @HotelID AND TaskStage != 10";

            var result = (await connection.QueryAsync<TaskItemModel>(query, new { HotelID = hotel.HotelID })).ToList();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"**Error fetching not completed task items for hotel {hotel.HotelName}: {ex.Message}");
            return new List<TaskItemModel>();
        }
    }

}