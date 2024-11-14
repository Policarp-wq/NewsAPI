using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

namespace NewsAPI.Controllers
{
    public class TagController : MyController<Tag>
    {
        public TagController(NewsDBContext context) : base(context, (context) => context.Tags)
        {
        }
    }
}
