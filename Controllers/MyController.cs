using Microsoft.AspNetCore.Mvc;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class MyController<T> : ControllerBase
    {
        protected readonly NewsdbContext _context;

        public MyController(NewsdbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public abstract Task<IActionResult> Index();
        [HttpGet("{id}")]
        public abstract Task<IActionResult> GetById(int id);
        [HttpPost]
        public abstract Task<IActionResult> Add(T entity);
        [HttpPost]
        public abstract Task<IActionResult> Update(T entity);
        [HttpDelete("{id}")]
        public abstract Task<IActionResult> Delete(int id);
    }
}
