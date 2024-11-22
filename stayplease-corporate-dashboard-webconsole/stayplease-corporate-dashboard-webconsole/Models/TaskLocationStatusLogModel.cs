namespace stayplease_corporate_dashboard_webconsole;

public class TaskLocationStatusLogModel
{
    public Int64 ID { get; set; }

    public DateTime CreationTime { get; set; }

    public Int64 TaskID { get; set; }

    public Int64 LocationID { get; set; }

    public string? LocationName { get; set; }

    public Int64 TaskStatus { get; set; }

    public string? RoomStatus { get; set; }

    public string? ReservStatus { get; set; }

    public string? ServiceStatus { get; set; }
}
