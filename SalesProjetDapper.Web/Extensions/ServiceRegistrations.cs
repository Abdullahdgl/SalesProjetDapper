using SalesProjetDapper.Web.Context;
using SalesProjetDapper.Web.Services.Dashboard;
using SalesProjetDapper.Web.Services.Orders;

namespace SalesProjetDapper.Web.Extensions
{
	public static class ServiceRegistrations
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddScoped<AppDbContext>();
			services.AddScoped<IDashboardService, DashboardService>();
			services.AddScoped<IOrderService, OrderService>();
		}
	}
}
