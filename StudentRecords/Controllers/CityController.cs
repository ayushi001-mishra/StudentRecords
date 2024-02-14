using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRecords.Models;

namespace StudentRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly StudentContext _studentContext;

        public CityController(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }

        [HttpGet("{stateId}")]
        public async Task<ActionResult<IEnumerable<City>>> GetCities(int stateId)
        {
            var cities = await _studentContext.Cities.Where(c => c.StateId == stateId).ToListAsync();
            return Ok(cities);
        }

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

