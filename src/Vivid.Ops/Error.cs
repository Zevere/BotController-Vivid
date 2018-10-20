using System.ComponentModel.DataAnnotations;

namespace Vivid.Ops
{
    /// <summary>
    /// Represents an error occured in operation
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Error code. One of values in <see cref="ErrorCode"/>
        /// </summary>
        [Required]
        public ErrorCode Code { get; }

        /// <summary>
        /// Optional error message in a user-friendly format
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Optional hint message to fix the issue in a user-friendly format
        /// </summary>
        public string Hint { get; }

        /// <summary>
        /// Initialize an instance of <see cref="Error"/>
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        /// <param name="hint">Hint message to fix the issue</param>
        public Error(ErrorCode code, string message = null, string hint = null)
        {
            Code = code;
            Message = message;
            Hint = hint;
        }
    }
}
