using core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using System.Security.Claims;

namespace API.Extentions
{
    public static class ClaimsPrincipleExtentions
    {
        public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var userToReturn = await userManager.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Email == user.GetUserEmail());
            return userToReturn == null ? throw new AuthenticationException("User not found.") : userToReturn;
        }
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email)
                ?? throw new AuthenticationException("User email not found in claims.");
            return email;
        }
    }
}
