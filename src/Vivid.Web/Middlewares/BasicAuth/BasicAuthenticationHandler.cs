﻿//using System;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Vivid.Data.Abstractions;
//
//namespace Vivid.Web.Middlewares.BasicAuth
//{
//    class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    {
//        private new const string Scheme = "Basic";
//
////        private readonly IUserRegistrationRepository _userRegistrationRepo;
//
//        public BasicAuthenticationHandler(
////            IUserRegistrationRepository userRegistrationRepo,
//            IOptionsMonitor<AuthenticationSchemeOptions> options,
//            ILoggerFactory logger,
//            UrlEncoder encoder,
//            ISystemClock clock
//        )
//            : base(options, logger, encoder, clock)
//        {
////            _userRegistrationRepo = userRegistrationRepo;
//        }
//
//        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            string authorizationHeader = Request.Headers["Authorization"];
//            if (string.IsNullOrEmpty(authorizationHeader))
//            {
//                return AuthenticateResult.NoResult();
//            }
//
//            if (!authorizationHeader.StartsWith(Scheme + ' ', StringComparison.OrdinalIgnoreCase))
//            {
//                return AuthenticateResult.NoResult();
//            }
//
//            string token =
//                Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeader.Substring(Scheme.Length + 1)));
//
//            if (string.IsNullOrEmpty(token))
//            {
//                const string noToken = "No token";
//                Logger.LogInformation(noToken);
//                return AuthenticateResult.Fail(noToken);
//            }
//
//            try
//            {
//                var user = await _userRegistrationRepo.GetByTokenAsync(token);
//User
//                var ticket = new AuthenticationTicket(
//                    new ClaimsPrincipal(new[]
//                    {
//                        new ClaimsIdentity(
//                            new BasicAuthIdentity(user.DisplayId),
//                            new[] {new Claim("token", token),}
//                        )
//                    }),
//                    Scheme
//                );
//                return AuthenticateResult.Success(ticket);
//            }
//            catch (EntityNotFoundException)
//            {
//                const string message = "invalid token";
//                Logger.LogInformation(message);
//                return AuthenticateResult.Fail(message);
//            }
//        }
//    }
//}
