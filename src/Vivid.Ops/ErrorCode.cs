using System.Runtime.Serialization;

namespace Vivid.Ops
{
    /// <summary>
    /// Error code values for operations
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Default error code. Used to indicated no error
        /// </summary>
        Default,

        /// <summary>
        /// Zevere user account doesn't exist
        /// </summary>
        [EnumMember(Value = "e.users.not_found")]
        UserNotFound,

        /// <summary>
        /// Bot not found
        /// </summary>
        [EnumMember(Value = "e.bots.not_found")]
        BotNotFound,

        /// <summary>
        /// Registration already exists
        /// </summary>
        [EnumMember(Value = "e.registrations.duplicate")]
        RegistrationExists,

        /// <summary>
        /// Registration not found
        /// </summary>
        [EnumMember(Value = "e.registrations.not_found")]
        RegistrationNotFound,

        /// <summary>
        /// Zevere GraphQL error
        /// </summary>
        [EnumMember(Value = "e.zv")]
        ZevereApi,
    }
}
