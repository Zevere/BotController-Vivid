using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    /// <summary>
    /// Represents an operation error
    /// </summary>
    [JsonObject(
        NamingStrategyType = typeof(CamelCaseNamingStrategy),
        ItemNullValueHandling = NullValueHandling.Ignore
    )]
    public class Error
    {
        /// <summary>
        /// Machine-readable error code
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Code { get; set; }

        /// <summary>
        /// Human-readable error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A hint to the user for correcting the error
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// Converts an operation error to an API error
        /// </summary>
        public static explicit operator Error(Ops.Error opsErr) =>
            opsErr == null
                ? null
                : new Error
                {
                    Code = GetCodeValue(opsErr.Code),
                    Message = opsErr.Message,
                    Hint = opsErr.Hint,
                };

        private static string GetCodeValue(Ops.ErrorCode code) =>
            typeof(Ops.ErrorCode)
                .GetFields()
                .Single(field => field.Name == code.ToString())
                .GetCustomAttribute<EnumMemberAttribute>()
                .Value;
    }
}
