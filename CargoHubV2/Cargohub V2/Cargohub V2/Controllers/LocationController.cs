using Microsoft.AspNetCore.Mvc;
using Cargohub_V2.Models;
using Cargohub_V2.Services;


namespace Cargohub_V2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public IActionResult GetAllLocations()
        {
            var locations = _locationService.GetLocations();
            if (locations == null)
            {
                return NotFound();

            }
            return Ok(locations);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetLocation(int id)
        {
            var location = _locationService.GetLocation(id);
            if (location != null)
            {
                return Ok(location);
            }
            return NotFound();
        }
        public ActionResult<Location> AddLocation([FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest("Location data is null");
            }


            var newLocation = _locationService.AddLocation(location);
            if(newLocation == false)
            {
                return BadRequest("Location data is nul");
            }

            if (newLocation)
            {
                return Created("Location created", newLocation);

            }
            return BadRequest();
        }


        [HttpPut]
        public ActionResult<Location> UpdateLocation(int id, [FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest();
            }

            var updatedLocation = _locationService.updateLocation(id, location);
            if (updatedLocation == null)
            {
                return NotFound();
            }

            return Ok(updatedLocation);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult deleteLocation(int id)
        {
            var location = _locationService.RemoveLocation(id);
            if (location == true)
            {
                return NoContent();
            }
            return NotFound();

        }

    }
}