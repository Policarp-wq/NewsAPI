using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using NewsAPI.Services;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class MyController<T> : ControllerBase where T : DBEntry
    {
        protected readonly NewsDBContext _context;
        protected readonly IDatabase _redis;
        protected readonly ILogger<MyController<T>> _logger;
        private readonly Func<NewsDBContext, DbSet<T>> _getDBset;
        protected DbSet<T> _dbset => _getDBset(_context);
        protected readonly string _key;

        public MyController(NewsDBContext context, IConnectionMultiplexer muxer, ILogger<MyController<T>> logger,
            Func<NewsDBContext, DbSet<T>> getDBset, string key)
        {
            _context = context;
            _getDBset = getDBset;
            _logger = logger;
            _redis = muxer.GetDatabase();
            _key = key;
        }

        [HttpGet]
        public async virtual Task<IActionResult> Index()
        {
            _logger.LogDebug($"Indexing all in {HttpContext.Request.Path}");
            return new ObjectResult(await _dbset.AsNoTracking().ToListAsync());
        }
        [HttpGet("{id}")]
        public async virtual Task<IActionResult> GetById(int id)
        {
            var cached = await RedisHandler.GetFromRedisAsync(_redis, _key, id);
            T? entity;
            if (cached.HasValue)
            {
                entity = JsonConvert.DeserializeObject<T>(cached.ToString());
                _logger.LogInformation($"Got key {_key}:{id} from Redis");
            }
            else
            {
                entity = await _dbset.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning($"Not found in db {id} {HttpContext.Request.Path}");
                    return NotFound();
                }
                _logger.LogDebug($"Added to redis {id} {HttpContext.Request.Path}");
                await RedisHandler.AddToRedisAsync(_redis, _key, entity);
            }
            return new ObjectResult(entity);
        }
        [HttpPost]
        public async virtual Task<IActionResult> Add([FromBody] T entity)
        {
            if (entity == null)
            {
                _logger.LogWarning($"Attempt to add null entity {HttpContext.Request.Path}");
                return BadRequest();
            }
            if (_dbset.Any(e => e.Id == entity.Id))
            {
                _logger.LogWarning($"Entity already exists in db with id {entity.Id}");
                return Conflict();
            }
            var created = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            await RedisHandler.AddToRedisAsync(_redis, _key, entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
        [HttpPost]
        public async virtual Task<IActionResult> Update([FromBody] T entity)
        {
            if (entity == null)
            {
                _logger.LogWarning($"Attempt to add null entity {HttpContext.Request.Path}");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Attempt to update entity with invalid model state {HttpContext.Request.Path}." +
                    $" Entity: {JsonConvert.SerializeObject(entity)}");
                return BadRequest(ModelState);
            }

            if (!_dbset.Any(e => e.Id == entity.Id))
            {
                _logger.LogWarning($"Not found in db {entity.Id} {HttpContext.Request.Path}");
                return NotFound();
            }
            var updated = _dbset.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogDebug($"Updated entity in redis: {_key}:{entity.Id}");
            await RedisHandler.AddToRedisAsync(_redis, _key, entity); 
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async virtual Task<IActionResult> Delete(int id)
        {
            var entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning($"Attempt to deleted entity with id that not in db {id} in {HttpContext.Request.Path}");
                return NotFound();
            }
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
            await RedisHandler.DeleteFromRedis(_redis, _key, id);
            _logger.LogDebug($"Deleted {entity.Id} in {HttpContext.Request.Path}");
            return NoContent();
        }
    }
}
