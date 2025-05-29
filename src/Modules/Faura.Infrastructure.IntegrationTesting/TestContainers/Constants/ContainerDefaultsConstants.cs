namespace Faura.Infrastructure.IntegrationTesting.TestContainers.Constants;

public static class ContainerDefaultsConstants
{
    public static class Images
    {
        public const string Mongo = "mongo:6";
        public const string Redis = "redis:7";
        public const string Postgres = "postgres:15-alpine";
        public const string SqlServer = "mcr.microsoft.com/mssql/server:2022-lts";
    }

    public static class Ports
    {
        public const int Mongo = 27017;
        public const int Redis = 6379;
        public const int Postgres = 5432;
        public const int SqlServer = 1433;
    }
}
