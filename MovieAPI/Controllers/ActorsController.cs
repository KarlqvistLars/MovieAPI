using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.DTOs;
using MovieAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly MovieAPIContext _context;
    public ActorsController(MovieAPIContext context)
    {
        _context = context;
    }

    // GET: api/Actor
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
    {
        return await _context.Actors.ToListAsync();
    }

    // GET: api/Actor/5
    [HttpGet("{actorid:int}")]
    public async Task<ActionResult<Actor>> GetActor(int actorid)
    {
        var actor = await _context.Actors.FindAsync(actorid);
        if (actor == null)
        {
            return NotFound();
        }
        return actor;
    }

    // PUT: api/Actor/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{actorid:int}")]
    public async Task<IActionResult> PutActor(int? actorid, ActorDto actor)
    {
        if (actorid != actor.ActorId) return BadRequest();
        var existingActor = await _context.Actors
            .FirstOrDefaultAsync(a => a.ActorId == actorid);
        if (existingActor == null) return NotFound();
        existingActor.Name = actor.Name;
        existingActor.BirthYear = actor.BirthYear;
        try
        {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException)
        {
            if (!ActorExists(actorid))
            {
                return NotFound();
            } else
            {
                throw;
            }
        }
        return Ok();
    }

    // POST: api/Actor
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Actor>> PostActor(Actor actor)
    {
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetActor", new { actorid = actor.ActorId }, actor);
    }

    // DELETE: api/Actor/5
    [HttpDelete("{actorid}")]
    public async Task<IActionResult> DeleteActor(int? actorid)
    {
        var actor = await _context.Actors.FindAsync(actorid);
        if (actor == null)
        {
            return NotFound();
        }

        _context.Actors.Remove(actor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ActorExists(int? actorid)
    {
        return _context.Actors.Any(e => e.ActorId == actorid);
    }
}
