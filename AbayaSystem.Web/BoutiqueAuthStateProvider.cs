using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace AbayaSystem.Web
{
    public class UserSession
    {
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class BoutiqueAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private ClaimsPrincipal _currentUser;

        public BoutiqueAuthStateProvider()
        {
            // By default, anyone booting the app starts as an unauthenticated guest
            _currentUser = _anonymous;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // 🚨 NO JAVASCRIPT CALLS HERE: Instantly returns memory state. 
            // This is what stops the prerendering loop crash dead in its tracks.
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public void MarkUserAsAuthenticated(UserSession userSession)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.Name),
                new Claim("Username", userSession.Username),
                new Claim(ClaimTypes.Role, userSession.Role) // 🔑 Binds directly to @attribute [Authorize(Roles = "...")]
            };

            var identity = new ClaimsIdentity(claims, "BoutiqueCircuitAuth");
            _currentUser = new ClaimsPrincipal(identity);

            // Notify Blazor's UI to re-evaluate protected routes instantly
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public void MarkUserAsLoggedOut()
        {
            _currentUser = _anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}