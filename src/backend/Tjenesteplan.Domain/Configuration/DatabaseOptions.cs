namespace Tjenesteplan.Api.Configuration
{
    public class DatabaseOptions
    {
        private const string AdminPasswordReplacementValue = "{sqlAdministratorLoginPassword}";
        public string DefaultConnection { get; set; }
        public string SqlAdministratorLoginPassword { get; set; }

        public string ConnectionString =>
            DefaultConnection.Replace(AdminPasswordReplacementValue, SqlAdministratorLoginPassword);
    }
}