using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stayplease_corporate_dashboard_webconsole.Services;

public interface IAggregationCoordinatorService
{
    Task CoordinateTaskProcessingAsync(HotelConfig hotel, DateTime startDate, DateTime endDate, bool useAutoBackup);

    Task CoordinateNotCompletedTaskProcessingAsync(HotelConfig hotel);

    Task<bool> HasDataInsertedToday(HotelConfig hotel, DateTime syncDate, string syncTableName);

    List<HotelConfig> GetHotels();
}
