using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using StackExchange.Redis;

namespace NewsAPI.Controllers
{
    public class TagController : MyController<Tag>
    {
        public TagController(NewsDBContext context, IConnectionMultiplexer muxer, ILogger<TagController> logger) :
            base(context, muxer, logger, (context) => context.Tags, "tag")
        {
        }
    }
}
