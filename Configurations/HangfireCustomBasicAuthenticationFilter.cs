namespace HangfireBackgroundtaskSchedulerAPIPOC.Configurations
{
    using Hangfire.Dashboard;
    using Microsoft.Extensions.Configuration;
    using System.Text;

    public class HangfireCustomBasicAuthenticationFilter : IDashboardAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public HangfireCustomBasicAuthenticationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Authorize(DashboardContext context)
        {
            var userName = _configuration["HangfireCredentials:UserName"];
            var password = _configuration["HangfireCredentials:Password"];

            // Ensure that userName and password are not null or empty
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Perform authentication
            var httpContext = context.GetHttpContext();
            var isAuthenticated = AuthenticateUser(httpContext, userName, password);

            return isAuthenticated;
        }

        private bool AuthenticateUser(HttpContext httpContext, string userName, string password)
        {
            // Check if user is authenticated
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // Check if user has the necessary claims
            if (!httpContext.User.Identity.Name.Equals(userName, StringComparison.Ordinal))
            {
                return false;
            }

            // Validate password
            var providedPassword = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(providedPassword))
            {
                return false;
            }

            var encodedCredentials = providedPassword.ToString().Substring("Basic ".Length).Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parsedCredentials = decodedCredentials.Split(':');
            var providedUserName = parsedCredentials[0];

            return providedUserName == userName && providedPassword == password;
        }

        //// for Entity frame work Identity User 
        //public bool Authorize(DashboardContext context)
        //{
        //    var httpContext = context.GetHttpContext();

        //    // Allow all authenticated users to see the Dashboard.
        //    return httpContext.User?.Identity?.IsAuthenticated == true &&
        //                   httpContext.User.HasClaim(ClaimTypes.Role, ApplicationRole.SuperAdmin.ToString());
        //}
    }
}
