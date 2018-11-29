using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vivid.Web.Models;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Controllers
{
    /// <summary>
    /// MVC controller for handling user profiles
    /// </summary>
    [Route("/api/v1/operations/getUserProfile")]
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly Ops.IRegistrationService _registrationService;
        private readonly Ops.IUserProfileService _profileService;

        /// <inheritdoc />
        public UserProfileController(
            Ops.IRegistrationService registrationService,
            Ops.IUserProfileService profileService
        )
        {
            _registrationService = registrationService;
            _profileService = profileService;
        }

        /// <summary>
        /// Get the profile of a Zevere user account
        /// </summary>
        /// <remarks>
        /// User must have been registered with this bot first.
        /// </remarks>
        /// <param name="username">ID of the Zevere user</param>
        /// <response code="200">User profile found</response>
        /// <response code="400">Username is invalid or does not exist</response>
        /// <response code="403">User is not registered with this chat bot</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserProfile), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 403)]
        public async Task<IActionResult> Get([FromQuery] string username)
        {
            IActionResult result;

            if (string.IsNullOrWhiteSpace(username))
            {
                result = StatusCode(400, new Error
                {
                    Code = Enums.ErrorCodes.Validation,
                    Message = "Username cannot be empty"
                });
            }
            else
            {
                string botName = User.Identity.Name;
                var operationResult = await _registrationService.GetUserRegistrationForBotAsync(
                    botName,
                    username,
                    HttpContext.RequestAborted
                ).ConfigureAwait(false);

                if (operationResult.Error is null)
                {
                    var userOpResult = await _profileService.GetUserAsync(username, HttpContext.RequestAborted)
                        .ConfigureAwait(false);

                    if (userOpResult.Error is null)
                    {
                        var user = (UserProfile) userOpResult.User;
                        result = Ok(user);
                    }
                    else
                    {
                        // user is already registered by this bot and the user profile must exist
                        var apiError = (Error) userOpResult.Error;
                        result = StatusCode(500, apiError);
                    }
                }
                else
                {
                    var apiError = (Error) operationResult.Error;
                    int statusCode = 400;

                    if (operationResult.Error.Code == Ops.ErrorCode.RegistrationNotFound)
                    {
                        apiError.Hint = "Register the user with this bot first.";
                        statusCode = 403;
                    }

                    result = StatusCode(statusCode, apiError);
                }
            }

            return result;
        }
    }
}
