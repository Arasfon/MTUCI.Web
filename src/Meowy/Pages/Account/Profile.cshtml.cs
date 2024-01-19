using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using Meowy.Database;
using Meowy.Models.Database;

namespace Meowy.Pages.Account;

[Authorize]
public class ProfileModel(MeowyContext meowyContext) : PageModel
{
    public User ShownUser { get; set; } = null!;
    public bool IsCurrentUser { get; set; }

    [BindProperty(Name="edit", SupportsGet = true)]
    public bool IsEditing { get; set; } = false;

    public async Task<IActionResult> OnGet([FromRoute] long? userId)
    {
        long currentUserId = Int64.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (userId == currentUserId)
            userId = null;

        User? user;

        if (userId is null)
        {
            userId = currentUserId;

            user = await meowyContext.Users
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return RedirectToPage("/Account/Logout");

            IsCurrentUser = true;
        }
        else
        {
            user = await meowyContext.Users
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return NotFound();
        }

        ShownUser = user;

        return Page();
    }

    public async Task<IActionResult> OnPost(string? bio)
    {
        long userId = Int64.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        UserProfile? userProfile = await meowyContext.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (userProfile == null)
            return RedirectToPage("/Account/Profile");

        userProfile.Bio = bio?.Trim()[..Math.Min(bio.Length, 1024)];

        await meowyContext.SaveChangesAsync();

        return RedirectToPage("/Account/Profile");
    }
}