namespace Vivid.Web.Options
{
    /// <summary>
    /// Contains application settings for connecting to a data store
    /// </summary>
    public class DataOptions
    {
        /// <summary>
        /// MongoDB connection string 
        /// </summary>
        public string Connection { get; set; }
    }
}