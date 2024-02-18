using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRecords.Models;

namespace StudentRecords.Controllers
{
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly StudentContext _studentContext;
        public CityController(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }

        [Route("api/City")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetStates()
        {
            return await _studentContext.Cities.ToListAsync();
        }

        [Route("api/City/{id}")]
        [HttpGet]
        public async Task<ActionResult<City>> GetState(int id)
        {
            var city = await _studentContext.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        [Route("api/City/FromState/{stateId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities(int stateId)
        {
            var cities = await _studentContext.Cities.Where(c => c.StateId == stateId).ToListAsync();
            return Ok(cities);
        }

        [Route("api/City")]
        [HttpPost]
        public async Task<ActionResult<City>> CreateCity( City newCity)
        {
            if (newCity == null)
            {
                return BadRequest("City data is null.");
            }
            newCity.CreatedOn = DateTime.Now; ;
            newCity.ModifiedOn = DateTime.Now;
            _studentContext.Cities.Add(newCity);
            await _studentContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCities), new { stateId = newCity.StateId }, newCity);
        }
    }
}

