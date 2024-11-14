using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    public class ArticleController : MyController<Article>
    {
        public ArticleController(NewsDBContext context) : base(context, context => context.Articles)
        
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesWithUsers()
        {
            return new ObjectResult(await _dbset
                .Include(art => art.AuthorUser)
                .AsNoTracking()
                .ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetArticlesPreview()
        {
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
