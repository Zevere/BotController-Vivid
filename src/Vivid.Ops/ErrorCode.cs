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

        [EnumMember(Value = "e.bots.not_found")]
        BotNotFound,

        [EnumMember(Value = "e.registrations.duplicate")]
        RegistrationExists,
    }
}
