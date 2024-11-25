using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using StackExchange.Redis;

namespace NewsAPI.Controllers
{
    public class CommentController : MyController<Comment>
    {
        public CommentController(NewsDBContext context, IConnectionMultiplexer muxer, ILogger<CommentController> logger) :
            base(context, muxer, logger, (context) => context.Comments, "comment")
        {
        }
    }
}
