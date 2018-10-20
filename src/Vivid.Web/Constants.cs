namespace Vivid.Web
{
    public static class Constants
    {
        public static class ZevereRoutes
        {
            public static class PathParameters
            {
                public const string UserId = "{userId}";

                public const string TaskId = "{taskId}";
            }

            public const string Base = "/zv";

            public const string Login = Base + "/login";

            public const string Logout = Base + "/logout";

            public const string Users = Base + "/users";

            public const string User = Users + "/" + PathParameters.UserId;

            public const string Tasks = Users + "/" + PathParameters.UserId + "/tasks";

            public const string Task = Tasks + "/" + PathParameters.TaskId;
        }

        public const string JsonContentType = "application/json";

        public static class Regexes
        {
            public const string TaskId = @"^(?:[a-z]|[A-Z]|\d|_|\.|-)+$";
        }
    }
}