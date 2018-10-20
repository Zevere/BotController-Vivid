using Newtonsoft.Json.Linq;

namespace Zevere.Client
{
    public static class JTokenExtensions
    {
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
