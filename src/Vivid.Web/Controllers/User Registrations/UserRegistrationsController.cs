using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vivid.Data.Abstractions;
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
        private readonly IChatBotRepository _botsRepo;

        /// <inheritdoc />
        public UserRegistrationsController(IChatBotRepository botsRepo)
        {
            _botsRepo = botsRepo;
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
        [Produces(Constants.JsonContentType)]
        [ProducesResponseType(typeof(UserRegistrationsResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> Get([FromRoute] string username)
        {
            var a = User.Identity.Name;
            throw new NotImplementedException();
//            if (string.IsNullOrWhiteSpace(userName))
//                return BadRequest(); // ToDo use an error response generator helper class
//            if (!User.Identity.Name.Equals(userName, StringComparison.OrdinalIgnoreCase))
//                return Forbid();
//
//            IActionResult result = default;
//            UserEntity user;
//            try
//            {
//                user = await _userRegRepo.GetByNameAsync(userName);
//            }
//            catch (EntityNotFoundException)
//            {
//                user = default;
//                result = NotFound(); // ToDo use error class
//            }
//
//            if (user != null)
//            {
//                UserDtoBase dto;
//                string acceptType = Request.Headers["accept"].SingleOrDefault();
//                switch (acceptType)
//                {
//                    case Constants.ZevereContentTypes.User.Full:
//                        dto = (UserFullDto) user;
//                        break;
//                    case Constants.ZevereContentTypes.User.Pretty:
//                        dto = (UserPrettyDto) user;
//                        break;
//                    default:
//                        throw new InvalidOperationException("Invalid Accept type");
//                }
//
//                result = StatusCode((int) HttpStatusCode.OK, dto);
//            }
//
//            return result;
        }

        /// <summary>
        /// Connect the account of a Zevere user to his chat account
        /// </summary>
        /// <remarks>
        /// A Zevere user can connect(register) his account to an account on any of the supported chat platforms.
        /// This operations stores that association.
        /// </remarks>
        /// <param name="model">Arguments for the operation</param>
        /// <response code="201">User registration is complete</response>
        /// <response code="400">There are invalid fields or the username does not exist on Zevere</response>
        [HttpPost]
        [Consumes(Constants.JsonContentType)]
        [ProducesResponseType(typeof(UserRegistration), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> Post([FromBody] NewUserRegistration model)
        {
            throw new NotImplementedException();

//            if (model is null || !TryValidateModel(model))
//            {
//                return StatusCode((int) HttpStatusCode.BadRequest);
//            }
//
//            var user = (UserEntity) model;
//
//            await _userRegRepo.AddAsync(user);
//
//            string contentType = HttpContext.Request.Headers["Accept"].SingleOrDefault()?.ToLowerInvariant();
//            switch (contentType)
//            {
//                case Constants.ZevereContentTypes.Empty:
//                    return NoContent();
//                case Constants.ZevereContentTypes.User.Pretty:
//                    return Created($"{user.DisplayId}", (UserPrettyDto) user);
//                case Constants.ZevereContentTypes.User.Full:
//                    return Created($"{user.DisplayId}", (UserFullDto) user);
//                default:
//                    return BadRequest();
//            }
        }
//
//        [HttpPatch("{userName}")]
//        [Authorize]
//        [Consumes("application/json")]
//        [Produces(
//            Constants.ZevereContentTypes.Empty,
//            Constants.ZevereContentTypes.User.Pretty,
//            Constants.ZevereContentTypes.User.Full
//        )]
//        public async Task<IActionResult> Patch([FromRoute] string userName, [FromBody] JObject patch)
//        {
//            if (string.IsNullOrWhiteSpace(userName))
//                return BadRequest(); // ToDo use an error response generator helper class
//            if (!User.Identity.Name.Equals(userName, StringComparison.OrdinalIgnoreCase))
//                return Forbid();
//            if (patch is default)
//                return BadRequest();
//
//            string[] properties = {"first_name", "last_name"};
//            if (properties.All(p => patch[p]?.Value<string>() is default))
//                return BadRequest();
//            string fName = patch["first_name"]?.Value<string>();
//            string lName = patch["last_name"]?.Value<string>();
//
//            if (new[] {fName, lName}.All(string.IsNullOrWhiteSpace))
//                return BadRequest();
//
//            var user = await _userRegRepo.GetByNameAsync(userName);
//            user.FirstName = string.IsNullOrWhiteSpace(fName) ? user.FirstName : fName;
//            user.LastName = string.IsNullOrWhiteSpace(lName) ? user.LastName : lName;
//            await _userRegRepo.UpdateAsync(user);
//
//            string contentType = HttpContext.Request.Headers["Accept"].SingleOrDefault()?.ToLowerInvariant();
//            switch (contentType)
//            {
//                case Constants.ZevereContentTypes.Empty:
//                    return NoContent();
//                case Constants.ZevereContentTypes.User.Pretty:
//                    return Accepted((UserPrettyDto) user);
//                case Constants.ZevereContentTypes.User.Full:
//                    return Accepted((UserFullDto) user);
//                default:
//                    return BadRequest();
//            }
//        }

//        [HttpDelete("{userName}")]
//        [Authorize]
//        [ProducesResponseType((int) HttpStatusCode.NoContent)]
//        public async Task<IActionResult> Delete(string userName)
//        {
//            if (string.IsNullOrWhiteSpace(userName))
//                return BadRequest(); // ToDo use an error response generator helper class
//            if (!User.Identity.Name.Equals(userName, StringComparison.OrdinalIgnoreCase))
//                return Forbid();
//
//            var user = await _userRegRepo.GetByNameAsync(userName);
//            await _userRegRepo.DeleteAsync(user.Id);
//            return NoContent();
//        }

        /// <summary>
        /// Remove the registration of a current user with the current bot
        /// </summary>
        /// <remarks>
        /// A Zevere user can connect(register) his account to an account on any of the supported chat platforms.
        /// This operations retrieves the associations of an existing Zevere user to the Zevere chat bots.
        /// </remarks>
        /// <param name="username">ID of the Zevere user</param>
        /// <response code="204">Registration is removed</response>
        /// <response code="400">User ID is invalid or does not exist</response>
        /// <response code="404">User has not registered with any of the Zevere chat bots</response>
        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
//        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] string username)
        {
            throw new NotImplementedException();
        }
    }
}
