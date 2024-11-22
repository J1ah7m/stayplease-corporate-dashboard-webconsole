namespace stayplease_corporate_dashboard_webconsole;

public class HotelConfiguration
{
    public List<HotelConfig> Hotels { get; } = new()
    {
        //new HotelConfig
        //{
        //    HotelID = 2,
        //    HotelName = "TUH",
        //    ConnectionString = "Data Source=stayplease-hkdb.c732m3pc2dld.ap-east-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_tuh;  UserID=admin; Password=18ETUwqX2; Character Set=utf8;SslMode=None;",
        //    TimeZone = "China Standard Time",
        //    GroupName = "Swire"
        //},
        //new HotelConfig
        //{
        //    HotelID = 50,
        //    HotelName = "TMH",
        //    ConnectionString = "Data Source=52.80.11.247; Port=3306; Database=staypleasev3_hskp_tmh; User ID=admin; Password=18ETUwqX2!@#; Character Set=utf8mb4;Old Guids=true;",
        //    TimeZone = "China Standard Time",
        //    GroupName = "Swire"
        //},
        new HotelConfig
        {
            HotelID = 60,
            HotelName = "PRCKUL",
            ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prckul; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4;",
            TimeZone = "Singapore Standard Time",
            GroupName = "PPHG"
        }
        ,new HotelConfig
        {
            HotelID = 61,
            HotelName = "PRSYP",
            ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prsyp; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
            TimeZone = "AUS Eastern Standard Time",
            GroupName = "PPHG"
        }
    };
}
