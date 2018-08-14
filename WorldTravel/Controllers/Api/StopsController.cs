using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldTravel.Models;
using WorldTravel.services;
using WorldTravel.ViewModels;

namespace WorldTravel.Controllers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private IWorldRepository _repo;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repo,
            ILogger<StopsController> logger,
            GeoCoordsService coordsService)
        {
            _repo = repo;
            _logger = logger;
            _coordsService = coordsService;

        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repo.GetUserTripByName(tripName,User.Identity.Name);


                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList())); 


            }catch(Exception ex)
            {

                _logger.LogError("Failed to get stops: {0}", ex); 
            }

            return BadRequest("Failed to get stops");
        }

        [HttpGet("delete/{stopName}")]
        public IActionResult Delete(string tripName,string stopName)
        {

            Stop stop = _repo.GetUserStopByName(stopName, tripName, this.User.Identity.Name);



            if(stop == null)
            {

                return NotFound();

            }

            _repo.DeleteStop(stop);


            return Ok();

        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName,[FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    //lookup Geocodes 

                    var result = await _coordsService.GetCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);

                    }
                    else
                    {

                        newStop.Latitude = result.Latitude;

                        newStop.Longitude = result.Longitude; 
                        //Save to the database 
                        _repo.AddStop(tripName, newStop,User.Identity.Name);

                        if (await _repo.SaveChangesAsync())
                        {

                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }

            }
            catch (Exception ex)
            {


                _logger.LogError("Failed to save new Stop : {0} ", ex);


            }
            return BadRequest("Failed to save new Stop");
        }

    }
}
