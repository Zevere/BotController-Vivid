using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vivid.Data.Abstractions;
using Vivid.Data.Abstractions.Entities;

namespace Vivid.Web.Middlewares.BasicAuth
{
    class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private new const string Scheme = "Basic";

        private readonly IChatBotRepository _botsRepo;

        public BasicAuthenticationHandler(
            IChatBotRepository botsRepo,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        )
            : base(options, logger, encoder, clock)
        {
            _botsRepo = botsRepo;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            if (!authorizationHeader.StartsWith(Scheme + ' ', StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            string token;
            try
            {
                token = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authorizationHeader.Substring(Scheme.Length + 1))
                );
            }
            catch (FormatException)
            {
                Logger.LogInformation("Invalid token format");
                return AuthenticateResult.Fail("invalid");
            }
            // ToDo catch (Exception e)

            if (string.IsNullOrEmpty(token))
            {
                // ToDo
                const string noToken = "No token";
                Logger.LogInformation(noToken);
                return AuthenticateResult.Fail(noToken);
            }

            ChatBot bot;
            try
            {
                bot = await _botsRepo.GetByTokenAsync(token);
            }
            catch (EntityNotFoundException)
            {
                // ToDo
                const string message = "invalid token";
                Logger.LogInformation(message);
                return AuthenticateResult.Fail(message);
            }
            // ToDo catch all

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(
                        new BasicAuthIdentity(bot.Name),
                        new[]
                        {
                            new Claim("token", token),
                        }
                    ),
                }),
                Scheme
            );
            return AuthenticateResult.Success(ticket);
        }
    }
}
