using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    public class CommentController : MyController<Comment>
    {
        public CommentController(NewsDBContext context) : base(context, (context) => context.Comments)
        {
        }
    }
}
