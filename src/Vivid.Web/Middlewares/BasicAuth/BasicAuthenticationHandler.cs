using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vivid.Data;
using Vivid.Data.Entities;

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

            string authValue;
            try
            {
                authValue = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authorizationHeader.Substring(Scheme.Length + 1))
                );
            }
            catch (FormatException)
            {
                Logger.LogInformation("Invalid token format");
                return AuthenticateResult.Fail("invalid");
            }
            // ToDo catch (Exception e)

            if (string.IsNullOrEmpty(authValue))
            {
                // ToDo
                const string noToken = "No token";
                Logger.LogInformation(noToken);
                return AuthenticateResult.Fail(noToken);
            }

            int separatorIndex = authValue.IndexOf(":", StringComparison.Ordinal);
            if (separatorIndex < 1)
            {
                Logger.LogInformation("Invalid token format");
                return AuthenticateResult.Fail("invalid");
            }

            string botName = authValue.Substring(0, separatorIndex);
            if (authValue.Length < botName.Length + 2)
            {
                // too short for the token value
                Logger.LogInformation("Invalid token format");
                return AuthenticateResult.Fail("invalid");
            }

            string botToken = authValue.Substring(separatorIndex + 1);

            // ToDo catch all
            ChatBot bot = await _botsRepo.GetByNameAsync(botName)
                .ConfigureAwait(false);
            if (bot == null)
            {
                // ToDo
                const string message = "invalid token";
                Logger.LogInformation(message);
                return AuthenticateResult.Fail(message);
            }

            if (bot.Token != botToken)
            {
                const string message = "invalid token";
                Logger.LogInformation(message);
                return AuthenticateResult.Fail(message);
            }

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(
                        new BasicAuthIdentity(bot.Name),
                        new[]
                        {
                            new Claim("id", bot.Id),
                            new Claim("token", authValue),
                            new Claim("platform", bot.Platform),
                        }
                    ),
                }),
                Scheme
            );
            return AuthenticateResult.Success(ticket);
        }
    }
}
