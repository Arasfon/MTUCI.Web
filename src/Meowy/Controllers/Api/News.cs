using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using Meowy.Authorization;
using Meowy.Database;

namespace Meowy.Controllers.Api;

[Route("/api/[controller]")]
[ApiController]
public class News(
    MeowyContext dbContext
    ) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetArticle(long id)
    {
        Models.Database.News? news = await dbContext.News.FindAsync(id);

        if (news is null)
            return NotFound();

        return Ok(new { news });
    }

    [Route("last")]
    [HttpGet]
    public async Task<IActionResult> GetLastArticles(int count)
    {
        if (count is < 0 or > 5)
            return BadRequest();

        List<Models.Database.News> newsList = await dbContext.News.OrderByDescending(x => x.Date).Take(count).ToListAsync();
        return Ok(new { news = newsList });
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> CreateArticle([FromBody] string news)
    {
        Models.Database.News newsObj = new()
        {
            Title = "Новость",
            AuthorId = Int64.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Date = DateTimeOffset.UtcNow.UtcDateTime,
            Content = news
        };
        dbContext.News.Add(newsObj);
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch
        {
            return BadRequest();
        }

        return Created($"/news/{newsObj.Id}", null);
    }

    [HttpPut]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> UpdateArticle([FromQuery] long id, [FromBody] string news)
    {
        try
        {
            await dbContext.News.Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.Content, news));
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }
}
