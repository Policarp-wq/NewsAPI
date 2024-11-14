using Microsoft.AspNetCore.Mvc;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    public class ArticleController : MyController<Article>
    {
        public ArticleController(NewsdbContext context) : base(context)
        {
        }

        public async override Task<IActionResult> Add(Article entity)
        {
            throw new NotImplementedException();
        }

        public async override Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async override Task<IActionResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async override Task<IActionResult> Index()
        {
            throw new NotImplementedException();
        }

        public async override Task<IActionResult> Update(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}
