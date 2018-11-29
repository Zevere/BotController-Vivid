using Newtonsoft.Json.Linq;

namespace Zevere.Client
{
    /// <summary>
    /// Contains extension methods on <see cref="JToken"/>
    /// </summary>
    public static class JTokenExtensions
    {
        /// <summary>
        /// Gets a value using the provided <paramref name="path"/> or return <paramref name="defaultValue"/>
        /// </summary>
        /// <param name="j"><see cref="JToken"/> instance</param>
        /// <param name="path">JSON path to look for the value</param>
        /// <param name="defaultValue">Default value</param>
        /// <typeparam name="TValue">Type of the value expected</typeparam>
        /// <returns>Value in the specified path, otherwise the <paramref name="defaultValue"/></returns>
        public static TValue Value<TValue>(this JToken j, string path, TValue defaultValue)
        {
            string[] paths = path.Split('.');

            foreach (var key in paths)
            {
                if (j.HasValues)
                {
                    j = j[key];
                }
                else
                {
                    return defaultValue;
                }
            }

            return j.Value<TValue>();
        }
    }
}
