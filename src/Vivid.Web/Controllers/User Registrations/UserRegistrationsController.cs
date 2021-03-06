﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Vivid.Web.Models;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Controllers
{
    /// <summary>
    /// MVC controller for handling user registrations
    /// </summary>
    [Route("/api/v1/user-registrations")]
    [Authorize]
    public class UserRegistrationsController : Controller
    {
        private readonly Ops.IRegistrationService _registrationService;

        /// <inheritdoc />
        public UserRegistrationsController(Ops.IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        /// <summary>
        /// Get registrations for an user with Zevere chat bots
        /// </summary>
        /// <remarks>
        /// A Zevere user can connect(register) his account to an account on any of the supported chat platforms.
        /// This operations retrieves the associations of an existing Zevere user to the Zevere chat bots.
        /// </remarks>
        /// <param name="username">ID of the Zevere user</param>
        /// <response code="200">User registrations found</response>
        /// <response code="400">User ID is invalid or does not exist</response>
        /// <response code="404">User has not registered with any of the Zevere chat bots</response>
        [HttpGet("{username}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserRegistrationsResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> Get([FromRoute] string username)
        {
            var operationResult = await _registrationService.GetUserRegistrationsAsync(
                username,
                HttpContext.RequestAborted
            ).ConfigureAwait(false);

            if (operationResult.Error != null)
            {
                var statusCode = operationResult.Error.Code == Ops.ErrorCode.RegistrationNotFound ? 404 : 400;
                return StatusCode(statusCode, (Error) operationResult.Error);
            }

            var result = new UserRegistrationsResponse
            {
                Username = username,
                Registrations = operationResult.Registrations
                    .Select(r => (UserRegistration) r)
                    .ToArray()
            };

            return StatusCode(200, result);
        }

        /// <summary>
        /// Connect the account of a Zevere user to his chat account
        /// </summary>
        /// <remarks>
        /// A Zevere user can connect(register) his account to an account on any of the supported chat platforms.
        /// This operations stores that association.
        /// </remarks>
        /// <param name="newReg">Arguments for the operation</param>
        /// <response code="201">User registration is complete</response>
        /// <response code="400">There are invalid fields or the username does not exist on Zevere</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserRegistration), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> Post([FromBody] NewUserRegistration newReg)
        {
            if (!ModelState.IsValid)
            {
                string message = ModelState.First(pair =>
                        pair.Value.ValidationState == ModelValidationState.Invalid
                    )
                    .Value.Errors[0].ErrorMessage;
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = null;
                }

                return StatusCode(400, new Error
                {
                    Code = Enums.ErrorCodes.Validation,
                    Message = message
                });
            }

            var operationError = await _registrationService.RegisterUserAsync(
                User.Identity.Name,
                newReg.Username,
                newReg.ChatUserId,
                HttpContext.RequestAborted
            ).ConfigureAwait(false);

            if (operationError != null)
            {
                return StatusCode(400, (Error) operationError);
            }

            string chatPlatform = User.Claims.Single(claim => claim.Type == "platform").Value;
            var result = new UserRegistration
            {
                Platform = chatPlatform,
                BotId = User.Identity.Name,
                ChatUserId = newReg.ChatUserId,
            };
            return StatusCode(201, result);
        }

        /// <summary>
        /// Remove the registration of a current user with the current bot
        /// </summary>
        /// <remarks>
        /// A Zevere user can connect(register) his account to an account on any of the supported chat platforms.
        /// This operations retrieves the associations of an existing Zevere user to the Zevere chat bots.
        /// </remarks>
        /// <param name="username">ID of the Zevere user</param>
        /// <response code="204">Registration is deleted</response>
        /// <response code="400">User ID is invalid or does not exist</response>
        /// <response code="404">User has not registered with any of the Zevere chat bots</response>
        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> Delete([FromRoute] string username)
        {
            string botName = User.Identity.Name;
            var operationError = await _registrationService
                .DeleteUserRegistrationAsync(botName, username, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (operationError != null)
            {
                int statusCode = operationError.Code == Ops.ErrorCode.RegistrationNotFound ? 404 : 400;
                return StatusCode(statusCode, (Error) operationError);
            }

            return StatusCode(204);
        }
    }
}
