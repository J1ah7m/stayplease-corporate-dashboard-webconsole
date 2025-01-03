namespace stayplease_corporate_dashboard_webconsole;

public static class Queries
{
    public const string QueryTaskItemList = @"
        SELECT 
            STI.ID AS MissionID,
            OU.ID AS TaskCategoryID,
            OU.Code AS TaskCategoryCode,
            OU.DisplayName AS TaskCategory,
            TM.Description AS TaskName,
            STI.MetricId AS TaskID,
            STI.Stage AS TaskStage,
            SL.Name AS LocationName,
            DATE_ADD(STT.CreationTime, INTERVAL @TimeZone MINUTE) AS CreateTime,
            DATE_ADD(STT.ToDoTime, INTERVAL @TimeZone MINUTE) AS ToDoTime,
            DATE_ADD(STT.DoingTime, INTERVAL @TimeZone MINUTE) AS DoingTime,
            DATE_ADD(STT.DoneTime, INTERVAL @TimeZone MINUTE) AS DoneTime,
            DATE_ADD(STT.PauseTime, INTERVAL @TimeZone MINUTE) AS PauseTime,
            DATE_ADD(STTL.CreationTime, INTERVAL @TimeZone MINUTE) AS FirstDoingTime,
            DATE_ADD(STT.ScheduleTime, INTERVAL @TimeZone MINUTE) AS ScheduleTime,
            SZ.Name AS ZoneName,
            STI.LocationId,
            SZ.Id AS ZoneID,
            (CASE STI.MetricId 
                WHEN STC.RoomAssignTaskID THEN 1 
                WHEN STC.RedoRoomAssnTaskID THEN 2 
                WHEN STC.InspectTaskID THEN 3 
                WHEN STC.TuchupInspectTaskId THEN 4 
                WHEN STC.ConfirmTaskID THEN 5 
                WHEN STC.CallBackTaskID THEN 6  
                WHEN STC.MakeUpRoomTaskID THEN 7 
                WHEN STC.TurnDownServiceTaskID THEN 8 
                WHEN STC.CheckOutTaskID THEN 9 
                WHEN STC.InspectTurnDownId THEN 11 
                ELSE (CASE OU.Id WHEN STC.EngDepartmentId THEN 10 ELSE 0 END) 
             END) AS TaskType,
            STI.ParentId,
            STI.IsGuestRequest,
            SL.Type AS LocationType,
            IFNULL(SERD.TimeLimit,0) AS TimeLimit,
            STI.IsDeleted,STI.Tags AS TagName,STI.Quantity,STI.TaskStatus,STI.CreatorUserId AS CreateUserId, STI.AssigneeId AS DoneUserId,
            CONCAT(U.Name,' ',U.Surname) AS CreateUserName,CONCAT(CU.Name,' ',CU.Surname) AS DoneUserName,
            STI.Priority AS IsUrgent,STI.IsIncident AS IsComplaint,STT.PauseReasion AS PauseReason
        FROM {TaskItems_TableName} STI 
        JOIN sptaskmetrics TM ON TM.Id = STI.MetricId 
        JOIN splocations SL ON SL.Id = STI.LocationId 
        LEFT JOIN spzones SZ ON SZ.Id = SL.SPZoneId 
        JOIN {TaskTimers_TableName} STT ON STT.TaskId = STI.Id 
        JOIN organizationunits OU ON OU.Id = TM.OrganizationUnitId  
        JOIN sptenantconfigs STC ON STC.TenantId = STI.TenantId 
        LEFT JOIN (
            SELECT MIN(CreationTime) AS CreationTime, TaskId 
            FROM {TaskTimeLines_TableName} 
            WHERE Stage = 5 
            GROUP BY TaskId 
            ORDER BY CreationTime ASC
        ) AS STTL ON STTL.TaskId = STI.Id 
        LEFT JOIN (
            SELECT EscalationRuleId, MIN(TimeLimit) AS TimeLimit
            FROM spescalationruledetails
            GROUP BY EscalationRuleId
        ) SERD ON SERD.EscalationRuleId = TM.EscalationRuleId
        LEFT JOIN users U ON U.Id = STI.AssigneeId
        LEFT JOIN users CU ON CU.Id = STI.CreatorUserId
        WHERE  STI.TenantId = @TenantId  {QueryConditions};";

    public const string QueryIncidentItemList = @"
        SELECT 
            IFNULL(SII.MetricId,0) AS IncidentId,
            SII.Description AS IncidentName,
            SII.Status AS IncidentStatus, 
            SII.CreationTime AS CreateTime,
            SICR.CreationTime AS CloseTime,
            IFNULL(SICR.Cost, SII.Cost) AS Cost,
            SL.SpZoneID AS ZoneID
        FROM spincidentitems SII 
        JOIN sptenantconfigs STC ON STC.TenantId = SII.TenantId 
        LEFT JOIN spincidentcloserecords SICR ON SII.Id = SICR.IncidentId 
        LEFT JOIN splocations SL ON SL.Id = SII.LocationId 
        WHERE 
            SII.TenantId = @TenantId 
            AND DATE_ADD(SII.CreationTime, INTERVAL @TimeZone MINUTE) BETWEEN @StartDate AND @EndDate 
            AND SII.IsDeleted = 0 
            AND SII.Type <> 20;
    ";

    public const string QueryTaskLocationStatusLogs = @" SELECT ID, CreationTime, TaskId, LocationId, LocationName, TaskStatus, RoomStatus, ReservStatus,ServiceStatus 
            FROM {TaskLocationStatusLogs_TableName} WHERE 1=1 {QueryConditions};";
}
