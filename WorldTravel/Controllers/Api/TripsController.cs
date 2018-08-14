using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldTravel.Models;
using WorldTravel.ViewModels;

namespace WorldTravel.Controllers.Api
{
    
    [Route("api/trips")]
    public class TripsController : Controller 
    {
        private IWorldRepository _repo;
        private ILogger<TripsController> _logger;

        public TripsController(IWorldRepository repo,ILogger<TripsController> logger)
        {
            _repo = repo;
            _logger = logger; 
        }
        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {

            try
            {
                var results = _repo.GetTripsByUsername(this.User.Identity.Name);

                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }catch(Exception ex)
            {

                //Todo Logging 
                _logger.LogError($"Failed to get All trips {ex}"); 
        
                return BadRequest("Error Occurred"); 
            }
        }

        [HttpGet("delete/{tripName}")]
        public IActionResult Delete(string tripName)
        {


            Trip trip = _repo.GetUserTripByName(tripName, this.User.Identity.Name);


           

            if (trip == null)
            {

                return NotFound(); 
            }

            _repo.DeleteTrip(trip);

            return Ok();
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {

            if (ModelState.IsValid)
            {
                //Save to the Database 

               

                var newTrip = Mapper.Map<Trip>(theTrip);

                newTrip.UserName = User.Identity.Name; 

                _repo.AddTrip(newTrip);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }else
                {


                    return BadRequest("Failed to save the Trip!"); 
                }
            }

            return BadRequest(ModelState);

        }
    }
}
