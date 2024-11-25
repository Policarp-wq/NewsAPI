using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using StackExchange.Redis;

namespace NewsAPI.Controllers
{
    public class UserController : MyController<User>
    {
        public UserController(NewsDBContext context, IConnectionMultiplexer muxer, ILogger<UserController> logger) :
            base(context,muxer, logger, (context) => context.Users, "user")
        {
        }
        //Возвращать надо .Entity!!!
        //public async override Task<IActionResult> Add([FromBody] User entity)
        //{
        //    if(entity == null)
        //    {
        //        return BadRequest();
        //    }
        //    if(_context.Users.Any(e => e.Id == entity.Id))
        //    {
        //        return Conflict();
        //    }
        //    if (!ModelState.IsValid)
        //        return NotFound();
        //    var created = await _context.AddAsync(entity);
        //    await _context.SaveChangesAsync();
        //    return new ObjectResult(created.Entity);
        //}
        //[HttpDelete("{id}")]
        //public async override Task<IActionResult> Delete(int id)
        //{
            
        //}

        //public async override Task<IActionResult> GetById(int id)
        //{
            
        //}

        //public async override Task<IActionResult> Index()
        //{
            
        //}

        //public async override Task<IActionResult> Update(User entity)
        //{
        //    if(_context.Users.Any(e => e.Id == entity.Id))
        //    {
        //        var updated = _context.Users.Update(entity);
        //        await _context.SaveChangesAsync();
        //        return new ObjectResult(updated.Entity);
        //    }
        //    return NotFound();
        //}
    }
}
