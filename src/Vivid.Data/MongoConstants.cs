namespace Vivid.Data
{
    /// <summary>
    /// Contains constant values used for the MongoDB
    /// </summary>
    public static class MongoConstants
    {
        /// <summary>
        /// Contains collections information
        /// </summary>
        public static class Collections
        {
            /// <summary>
            /// Contains "bots" collection information
            /// </summary>
            public static class Bots
            {
                /// <summary>
                /// Collection's name
                /// </summary>
                public const string Name = "bots";

                /// <summary>
                /// Collection's Indexes
                /// </summary>
                public static class Indexes
                {
                    /// <summary>
                    /// Index key
                    /// </summary>
                    public const string BotId = "bot_id";
                }
            }

            /// <summary>
            /// Contains "registrations" collection information
            /// </summary>
            public static class Registrations
            {
                /// <summary>
                /// Collection's name
                /// </summary>
                public const string Name = "registrations";

                /// <summary>
                /// Collection's Indexes
                /// </summary>
                public static class Indexes
                {
                    /// <summary>
                    /// Index key
                    /// </summary>
                    public const string BotUsername = "bot_username";
                }
            }
        }
    }
}
