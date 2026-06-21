using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;

namespace MovieAPI.Interfaces
{
    public interface IActorService
    {
        public Task<ICollection<ActorDto>> GetActors();
        public Task<ActorDto?> GetActor(int actorid);
        public Task<IActionResult> PutActor(int actorid, ActorDto actor);
        public Task<ActionResult<ActorDto>> PostActor(ActorDto actorDto);
        public Task<IActionResult> DeleteActor(int? actorid);
    }
}