namespace stayplease_corporate_dashboard_webconsole;

public class SyncLogModel
{
    public int ID { get; set; }

    public int HotelID { get; set; }

    public string? SyncTable { get; set; }

    public string? SyncDate { get; set; }

    public string? SyncStatus { get; set; }

    public string? ErrorMsg { get; set; }
}
