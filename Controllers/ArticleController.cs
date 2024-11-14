using Microsoft.AspNetCore.Mvc;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    public class ArticleController : MyController<Article>
    {
        public ArticleController(NewsdbContext context) : base(context, (context) => context.Articles)
        {
        }
    }
}
