using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Data
{
    public interface IRestaurantsService
    {
        Task<IList<Restaurant>> GetRestaurantsInRange(double latitude,double longitude, int radius);
    }
}