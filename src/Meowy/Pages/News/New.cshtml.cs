using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Meowy.Authorization;

namespace Meowy.Pages.News;

[Authorize(Roles = RoleNames.Admin)]
public class NewModel : PageModel
{
    public void OnGet()
    {

    }
}
