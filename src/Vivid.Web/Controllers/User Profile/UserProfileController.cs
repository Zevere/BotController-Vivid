using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Controllers
{
    /// <summary>
    /// MVC controller for handling user registrations
    /// </summary>
    [Route("/api/v1/users/{username}")]
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly Ops.IUserProfileService _profileService;

        /// <inheritdoc />
        public UserProfileController(
            Ops.IUserProfileService profileService
        )
        {
            _profileService = profileService;
        }
    }
}
