namespace Vivid.Data.Mongo
{
    public static class MongoConstants
    {
        public static class Database
        {
            public const string Test = "borzoo-test-mongo";
        }

        public static class Collections
        {
            public static class Bots
            {
                public const string Name = "bots";

                public static class Indexes
                {
                    public const string BotId = "bot_id";
                }
            }

            public static class Users
            {
                public const string Name = "users";

                public static class Indexes
                {
                    public const string Username = "username";
                }
            }

            public static class Registrations
            {
                public const string Name = "registrations";

                public static class Indexes
                {
                    public const string OwnerListName = "owner_list-name";
                }
            }

            public static class TaskItems
            {
                public const string Name = "task-items";

                public static class Indexes
                {
                    public const string ListTaskName = "list_task-name";
                }
            }
        }
    }
}
