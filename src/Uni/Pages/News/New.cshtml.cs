using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Uni.Authorization;

namespace Uni.Pages.News;

[Authorize(Roles = RoleNames.Admin)]
public class NewModel : PageModel
{
    public void OnGet()
    {

    }
}
