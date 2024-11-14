using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class MyController<T> : ControllerBase where T : DBEntry
    {
        protected readonly NewsDBContext _context;
        private readonly Func<NewsDBContext, DbSet<T>> _getDBset;
        protected DbSet<T> _dbset => _getDBset(_context);

        public MyController(NewsDBContext context, Func<NewsDBContext, DbSet<T>> getDBset)
        {
            _context = context;
            _getDBset = getDBset;
        }
        [HttpGet]
        public async virtual Task<IActionResult> Index()
        {
            return new ObjectResult(await _dbset.AsNoTracking().ToListAsync());
        }
        [HttpGet("{id}")]
        public async virtual Task<IActionResult> GetById(int id)
        {
            var entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return new ObjectResult(entity);
        }
        [HttpPost]
        public async virtual Task<IActionResult> Add([FromBody]T entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }
            if (_dbset.Any(e => e.Id == entity.Id))
            {
                return Conflict();
            }
            var created = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
        [HttpPost]
        public async virtual Task<IActionResult> Update([FromBody]T entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_dbset.Any(e => e.Id == entity.Id))
            {
                return NotFound();
            }
                var updated = _dbset.Update(entity);
                await _context.SaveChangesAsync();
                return NoContent();
        }
        [HttpDelete("{id}")]
        public async virtual Task<IActionResult> Delete(int id)
        {
            var entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
