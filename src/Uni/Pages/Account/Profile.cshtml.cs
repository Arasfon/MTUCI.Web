using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using Uni.Database;
using Uni.Models.Database;

namespace Uni.Pages.Account;

[Authorize]
public class ProfileModel(UniContext uniContext) : PageModel
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

            user = await uniContext.Users
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return RedirectToPage("/Account/Logout");

            IsCurrentUser = true;
        }
        else
        {
            user = await uniContext.Users
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
        UserProfile? userProfile = await uniContext.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (userProfile == null)
            return RedirectToPage("/Account/Profile");

        userProfile.Bio = bio?.Trim()[..Math.Min(bio.Length, 1024)];

        await uniContext.SaveChangesAsync();

        return RedirectToPage("/Account/Profile");
    }
}