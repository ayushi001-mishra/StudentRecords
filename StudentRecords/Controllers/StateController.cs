using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRecords.Models;

namespace StudentRecords.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly StudentContext _studentContext;
        public StateController(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return await _studentContext.States.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
            var state = await _studentContext.States.FindAsync(id);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
            state.CreatedOn = DateTime.Now;
            state.ModifiedOn = DateTime.Now;
            _studentContext.States.Add(state);
            await _studentContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetState), new { id = state.SId }, state);
        }
    }
}
