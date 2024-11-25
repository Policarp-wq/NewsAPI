using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using StackExchange.Redis;

namespace NewsAPI.Controllers
{
    public class ArticleController : MyController<Article>
    {
        public ArticleController(NewsDBContext context, IConnectionMultiplexer muxer, ILogger<ArticleController> logger)
            : base(context, muxer, logger, context => context.Articles, "article")

        {
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesWithUsers()
        {
            _logger.LogDebug("Accessing articles with users");
            return new ObjectResult(await _dbset
                .Include(art => art.AuthorUser)
                .AsNoTracking()
                .ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetArticlesPreview()
        {
            _logger.LogDebug("Accessing articles preview");
            return new ObjectResult(await _dbset
                .Include(art => art.AuthorUser)
                .Include(art => art.Tags)
                .Select(art => new
                {
                    art.Id,
                    art.Header,
                    art.PostTime,
                    Author = art.AuthorUser.Fullname,
                    Tags = art.Tags.Select(t => t.Name)
                })
                .AsNoTracking()
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleFullPresentation(int id)
        {
            _logger.LogDebug("Accessing article full presentation! Require big request to db");
            return new ObjectResult(await _dbset
                .Include(art => art.AuthorUser)
                .Include(art => art.Tags)
                .Include(art => art.Comments)
                !.ThenInclude(c => c.AuthorUser)
                .Select(art => new
                {
                    art.Id,
                    art.Header,
                    art.Content,
                    art.PostTime,
                    Author = art.AuthorUser.Fullname,
                    Tags = art.Tags.Select(t => t.Name),
                    Comments = art.Comments.Select(c => new
                    {
                        c.Id,
                        c.Content,
                        c.PostTime,
                        Author = c.AuthorUser.Fullname
                    })
                })
                .AsNoTracking()
                .SingleAsync(art => art.Id == id));
        }

        [HttpGet("{tagId}")]
        public async Task<IActionResult> GetByTag(int tagId)
        {
            return new ObjectResult(await _dbset
                .Include(art => art.Tags)
                .AsNoTracking()
                //.Where(a => a.Tags != null && a.Tags.Any(t => t.Id == tagId))
                .ToListAsync());
        }
    }
}
