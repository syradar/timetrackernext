namespace TimeTracking.API;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class TimeEntries
    {
        private const string Base = $"{ApiBase}/time-entries";

        public const string Create = Base;

        public const string Get = $"{Base}/{{id:guid}}";

        public const string GetAll = Base;

        public const string Update = $"{Base}/{{id:guid}}";

        public const string Delete = $"{Base}/{{id:guid}}";
    }
}