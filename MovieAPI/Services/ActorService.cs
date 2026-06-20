using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;
using MovieAPI.Models;

namespace MovieAPI.Services
{
    public class ActorService : IActorService
    {
        private readonly IMovieAPIContext _db;
        public ActorService(IMovieAPIContext db)
        {
            _db = db;
        }

        public async Task<ICollection<ActorDto>> GetActors()
        {
            return await _db.Actors
                .Include(a => a.Movies)
                .Select(a => new ActorDto {
                    ActorId = a.ActorId,
                    Name = a.Name,
                    BirthYear = a.BirthYear,
                    Movies = a.Movies!
                        .Select(m => m.Title)
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<ActorDto?> GetActor(int actorid)
        {
            var actor = await _db.Actors
                .Include(a => a.Movies)
                .FirstOrDefaultAsync(a => a.ActorId == actorid);

            if (actor == null) return null;

            return new ActorDto {
                ActorId = actor.ActorId,
                Name = actor.Name,
                BirthYear = actor.BirthYear,
                Movies = actor.Movies!
                    .Select(m => m.Title)
                    .ToList()
            };
        }

        public async Task<IActionResult> PutActor(int actorid, ActorDto actor)
        {
            if (actorid != actor.ActorId) return new BadRequestResult();
            var existingActor = await _db.Actors
                .FirstOrDefaultAsync(a => a.ActorId == actorid);

            if (existingActor == null) return new NotFoundResult();
            existingActor.Name = actor.Name;
            existingActor.BirthYear = actor.BirthYear;
            existingActor.Movies = null; // Movies are not updated through this endpoint

            try
            {
                await _db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(actorid))
                {
                    return new NotFoundResult();
                } else
                {
                    throw;
                }
            }
            return new OkResult();
        }

        public async Task<ActionResult<ActorDto>> PostActor(ActorDto actorDto)
        {
            if (actorDto == null)
            {
                return new NotFoundResult();
            }
            var actor = new Actor {
                Name = actorDto.Name,
                BirthYear = actorDto.BirthYear,
            };
            _db.Actors.Add(actor);
            await _db.SaveChangesAsync();
            return new ActorDto {
                ActorId = actor.ActorId,
                Name = actor.Name,
                BirthYear = actor.BirthYear
            };
        }

        public async Task<IActionResult> DeleteActor(int? actorid)
        {
            var actor = await _db.Actors.FindAsync(actorid);
            if (actor == null)
            {
                return new NotFoundResult();
            }
            _db.Actors.Remove(actor);
            await _db.SaveChangesAsync();
            return new NoContentResult();
        }

        private bool ActorExists(int? actorid)
        {
            return _db.Actors.Any(e => e.ActorId == actorid);
        }
    }
}

