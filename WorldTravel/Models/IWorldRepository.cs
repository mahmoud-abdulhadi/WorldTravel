using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldTravel.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByUsername(string username);


        Trip GetTripByName(string name);
        Trip GetUserTripByName(string tripName, string username);


        Stop GetUserStopByName(string stopName,string tripName, string username);

        void AddTrip(Trip trip);

        void AddStop(string tripName, Stop newStop,string username);

        void DeleteTrip(Trip trip);

        void DeleteStop(Stop stop);

        Task<bool>  SaveChangesAsync();
        
    }
}