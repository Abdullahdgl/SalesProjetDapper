using SalesProjetDapper.Web.Context;

namespace SalesProjetDapper.Web.Extensions
{
	public static class ServiceRegistrations
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddScoped<AppDbContext>();
		}
	}
}
