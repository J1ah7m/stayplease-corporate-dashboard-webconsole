namespace stayplease_corporate_dashboard_webconsole;

public class TaskItemModel
{
    public Int64 TaskCategoryID { get; set; }

    public string? TaskCategory { get; set; }

    public int MissionID { get; set; }

    public int TaskID { get; set; }

    public int LocationID { get; set; }

    public string? LocationName { get; set; }

    public string? TaskName { get; set; }

    public string? TagName { get; set; }

    public int Quantity { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime ToDoTime { get; set; }

    public DateTime? DoingTime { get; set; }

    public DateTime? DoneTime { get; set; }

    public DateTime? PauseTime { get; set; }

    public int TaskStage { get; set; }

    public Int64 TimeLimit { get; set; }

    public int TaskType { get; set; }

    public int ParentID { get; set; }

    public DateTime? FirstDoingTime { get; set; }

    public DateTime? ScheduleTime { get; set; }

    public int IsGuestRequest { get; set; }

    public int ZoneID { get; set; }

    public string? ZoneName { get; set; }

    public int LocationType { get; set; }

    public int TaskStatus { get; set; }

    public int CreateUserId { get; set; }

    public string? CreateUserName { get; set; }

    public int DoneUserId { get; set; }

    public string? DoneUserName { get; set; }

    public int IsUrgent { get; set; }

    public int IsDeleted { get; set; }

    public int IsComplaint { get; set; }

    public string? PauseReason { get; set; }

    public string? RoomStatus_ToDo { get; set; }

    public string? ReservStatus_ToDo { get; set; }

    public string? ServiceStatus_ToDo { get; set; }

    public string? RoomStatus_Doing { get; set; }

    public string? ReservStatus_Doing { get; set; }

    public string? ServiceStatus_Doing { get; set; }

    public string? RoomStatus_Done { get; set; }

    public string? ReservStatus_Done { get; set; }

    public string? ServiceStatus_Done { get; set; }

    public string? BelongTo { get; set; }

}
