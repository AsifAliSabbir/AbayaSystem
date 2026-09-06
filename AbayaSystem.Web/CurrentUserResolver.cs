using System.Security.Claims;
using AbayaSystem.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AbayaSystem.Web;

public static class CurrentUserResolver
{
    public static async Task<int?> GetWorkerIdAsync(AuthenticationStateProvider authStateProvider, BoutiqueDbContext dbContext)
    {
        var user = (await authStateProvider.GetAuthenticationStateAsync()).User;
        var claimValues = new[]
        {
            user.FindFirst("UserID")?.Value,
            user.FindFirst("WorkerId")?.Value,
            user.FindFirst(ClaimTypes.NameIdentifier)?.Value
        };

        foreach (var value in claimValues)
        {
            if (int.TryParse(value, out var workerId) && workerId > 0) return workerId;
        }

        var username = user.FindFirst("Username")?.Value;
        if (!string.IsNullOrWhiteSpace(username))
        {
            var workerId = await dbContext.Workers.AsNoTracking()
                .Where(w => w.Username == username)
                .Select(w => (int?)w.WorkerId)
                .FirstOrDefaultAsync();
            if (workerId.HasValue) return workerId.Value;
        }

        var displayName = user.Identity?.Name;
        return string.IsNullOrWhiteSpace(displayName)
            ? null
            : await dbContext.Workers.AsNoTracking()
                .Where(w => w.Name == displayName)
                .Select(w => (int?)w.WorkerId)
                .FirstOrDefaultAsync();
    }
}
