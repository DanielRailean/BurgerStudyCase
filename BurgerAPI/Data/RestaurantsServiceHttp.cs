using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Data
{
    public class RestaurantsServiceHttp: IRestaurantsService
    {
        public async Task<IList<Restaurant>> GetRestaurantsInRange(double latitude, double longitude, int radius)
        {
            return new List<Restaurant>();
        }
    }
}