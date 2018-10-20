using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Vivid.Web.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Error
    {
        [JsonProperty(Required = Required.Always)]
        public string Code { get; set; }

        public string Message { get; set; }

        public string Hint { get; set; }

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
