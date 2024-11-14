using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    public class UserController : MyController<User>
    {
        public UserController(NewsdbContext context) : base(context)
        {
        }
        //Возвращать надо .Entity!!!
        public async override Task<IActionResult> Add([FromBody] User entity)
        {
            if(entity == null)
            {
                return BadRequest();
            }
            if(_context.Users.Any(e => e.Id == entity.Id))
            {
                return Conflict();
            }
            if (!ModelState.IsValid)
                return NotFound();
            var created = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new ObjectResult(created.Entity);
        }
        [HttpDelete("{id}")]
        public async override Task<IActionResult> Delete(int id)
        {
            if (!_context.Users.Any(e => e.Id == id))
            {
                return NotFound();
            }
            var user = _context.Users.Remove(
                await _context.Users.SingleAsync(u => u.Id == id));
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async override Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        public async override Task<IActionResult> Index()
        {
            return new ObjectResult(await _context.Users.AsNoTracking().ToListAsync());
        }

        public async override Task<IActionResult> Update(User entity)
        {
            if(_context.Users.Any(e => e.Id == entity.Id))
            {
                var updated = _context.Users.Update(entity);
                await _context.SaveChangesAsync();
                return new ObjectResult(updated.Entity);
            }
            return NotFound();
        }
    }
}
