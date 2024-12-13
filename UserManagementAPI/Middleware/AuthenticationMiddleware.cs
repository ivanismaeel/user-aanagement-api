using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization header missing");
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();

            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token");
                    return;
                }

                // You can add more token validation logic here, such as checking token expiration

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Authentication error: {ex.Message}");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }
}
