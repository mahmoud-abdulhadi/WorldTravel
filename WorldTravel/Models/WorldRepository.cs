using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldTravel.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<IWorldRepository> _logger;

        public WorldRepository(WorldContext context,ILogger<IWorldRepository> logger)
        {
            _context = context;

            _logger = logger; 
        }

        public void AddStop(string tripName, Stop newStop,string username)
        {
            var trip = GetUserTripByName(tripName, username); 

            if(trip != null)
            {

                trip.Stops.Add(newStop);

                _context.Stops.Add(newStop); 
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip); 
        }

        public void DeleteTrip(Trip trip)
        {
            _context.Trips.Remove(trip);

            _context.SaveChanges();


        }

        public void DeleteStop(Stop stop)
        {
            _context.Stops.Remove(stop);

            _context.SaveChanges();

        }

        public IEnumerable<Trip> GetAllTrips()
        {

            _logger.LogInformation("Getting All the trips form the Database"); 
            return _context.Trips.ToList(); 
        }

        public Trip GetTripByName(string name)
        {
            return _context.Trips
                    .Include(t => t.Stops)
                    .Where(t => t.Name == name)
                    .FirstOrDefault();
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            return _context
                    .Trips
                    .Include(t => t.Stops)
                    .Where(t => t.UserName == name).ToList();
        }

        public Stop GetUserStopByName(string stopName,string tripName, string username)
        {
            Trip trip = GetUserTripByName(tripName, username);

            return trip.Stops.Where(t => t.Name == stopName).FirstOrDefault();
                   
        }

        public Trip GetUserTripByName(string tripName, string name)
        {
            return _context.Trips
                   .Include(t => t.Stops)
                   .Where(t => t.Name == tripName && t.UserName == name)
                   .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0; 
        }
    }
}
