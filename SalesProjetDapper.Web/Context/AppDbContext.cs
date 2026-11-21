using Microsoft.Data.SqlClient;
using System.Data;

namespace SalesProjetDapper.Web.Context
{
	public class AppDbContext
	{
		private readonly string _connectionString;
		public AppDbContext(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("SqlConnection")
				?? throw new InvalidOperationException("Connection string 'SqlConnection' not found.");
		}

		public IDbConnection CreateConnection()=> new SqlConnection(_connectionString);

	}
}
