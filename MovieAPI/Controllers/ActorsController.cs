using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;

// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }

    // GET: api/actors
    [HttpGet]
    public async Task<ActionResult<ICollection<ActorDto>>> GetActors()
    {
        var actors = await _actorService.GetActors();
        if (actors == null || !actors.Any())
        {
            return NotFound();
        }
        return Ok(actors);
    }

    //    GET: api/actor/5
    [HttpGet("{actorid}")]
    public async Task<ActionResult<ActorDto>> GetActor(int actorid)
    {
        var actor = await _actorService.GetActor(actorid);
        if (actor == null)
        {
            return NotFound();
        }
        return Ok(actor);
    }

    // POST: api/actor
    [HttpPost]
    public async Task<ActionResult<ActorDto>> PostActor(ActorDto actor)
    {
        var result = await _actorService.PostActor(actor);
        return CreatedAtAction(nameof(GetActor), new { actorId = actor.ActorId }, result.Value);
    }

    // PUT: api/actor/5
    [HttpPut("{actorid}")]
    public async Task<IActionResult> PutActor(int actorid, ActorDto actor)
    {
        return await _actorService.PutActor(actorid, actor);
    }

    // DELETE: api/actor/5
    [HttpDelete("{actorid}")]
    public async Task<IActionResult> DeleteActor(int? actorid)
    {
        var result = await _actorService.DeleteActor(actorid);
        return result;
    }
}
