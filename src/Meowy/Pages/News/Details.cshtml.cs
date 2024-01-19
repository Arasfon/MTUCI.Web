using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Meowy.Authorization;

namespace Meowy.Pages.News;

public class IndexModel : PageModel
{
    public bool IsEditing { get; set; }

    public IActionResult OnGet([FromRoute] long id, [FromQuery] bool edit = false)
    {
        if (!edit)
            return Page();

        if (User.Identity?.IsAuthenticated != true)
        {
            return Redirect(Request.Scheme + "://" + Request.Host + "/account/login?returnUrl=" +
                Uri.EscapeDataString(Request.Path + Request.QueryString.Value));
        }

        if (User.IsInRole(RoleNames.Admin) != true)
            return Forbid();

        IsEditing = edit;

        return Page();
    }
}
