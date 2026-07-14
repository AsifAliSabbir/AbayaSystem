using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AbayaSystem.Core;

namespace AbayaSystem.Infrastructure
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly BoutiqueDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(BoutiqueDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var worker = await _context.Workers
                .FirstOrDefaultAsync(w => w.Username.ToLower() == username.ToLower().Trim());

            if (worker == null) return false;

            // ⚠️ Simple verification for development. 
            // In full production, use: BCrypt.Net.BCrypt.Verify(password, worker.PasswordHash)
            if (worker.PasswordHash != password) return false;

            // Create security stamps (Claims) detailing who this person is and what they can touch
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, worker.Name),
                new Claim("WorkerId", worker.WorkerId.ToString()),
                new Claim("Roles", worker.AssignedRoles.ToString()) // e.g. "Salesman, QualityChecker"
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return true;
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}