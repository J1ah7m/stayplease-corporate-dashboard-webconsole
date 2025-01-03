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
            OtherHotelName = "PPSKUL",
            GroupName = "PPHG"
        },
        new HotelConfig
        {
            HotelID = 61,
            HotelName = "PRSYP",
            ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prsyp; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
            TimeZone = "AUS Eastern Standard Time",
            GroupName = "PPHG"
        }
        //,
        //new HotelConfig
        //{
        //    HotelID = 63,
        //    HotelName = "PRSSIN",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prssin; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 65,
        //    HotelName = "PRPEN",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prpen; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 82,
        //    HotelName = "PRSYD",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prsyd; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "AUS Eastern Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 93,
        //    HotelName = "PRSPS",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prsps; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 97,
        //    HotelName = "PRLGK",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prlgk; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 105,
        //    HotelName = "PPSSIN",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_ppssin; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 108,
        //    HotelName = "PPPER",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_ppper; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 111,
        //    HotelName = "PPSOR",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_ppsor; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 117,
        //    HotelName = "PPSSBR",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_ppssbr; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},        
        //new HotelConfig
        //{
        //    HotelID = 121,
        //    HotelName = "PPSIN",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_ppsin; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 134,
        //    HotelName = "PRSMB",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prsmb; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 143,
        //    HotelName = "PRSJKT",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prsjkt; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "SE Asia Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 144,
        //    HotelName = "PRMLK",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_prmlk; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "Singapore Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 160,
        //    HotelName = "PPHAN",
        //    ConnectionString = "Data Source=sp-rds-sgp1.cr0z6rjtgupv.ap-southeast-1.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_pphan; User ID=admin; Password=32CpaG!#$123; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "SE Asia Standard Time",
        //    GroupName = "PPHG"
        //},
        //new HotelConfig
        //{
        //    HotelID = 165,
        //    HotelName = "PPLON",
        //    ConnectionString = "Data Source=mhr-eur.cjmpxcqecwcp.eu-west-3.rds.amazonaws.com; Port=3306; Database=staypleasev3_hskp_pplon; User ID=admin; Password=64CpaG!#$567; ConnectionTimeout=20; sslMode=None;Pooling=False;Character Set=utf8mb4",
        //    TimeZone = "GMT Standard Time",
        //    GroupName = "PPHG"
        //}
    };
}
