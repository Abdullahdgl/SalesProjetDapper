using Microsoft.AspNetCore.Mvc;
using SalesProjetDapper.Web.Dtos.Common;
using SalesProjetDapper.Web.Services.Dashboard;

namespace SalesProjetDapper.Web.Controllers
{
	public class DashboardController : Controller
	{

		private readonly IDashboardService _dashboardService;

		public DashboardController(IDashboardService dashboardService)
		{
			_dashboardService = dashboardService;
		}
		public IActionResult Index() => View();

		[HttpGet]
		public async Task<IActionResult> Data(DateTime? from, DateTime? to, CancellationToken ct)
		{
			var range = Resolve(from, to);
			var payload = await _dashboardService.GetAsync(range, ct);
			return Json(payload);
		}

		internal static DateRange Resolve(DateTime? from, DateTime? to)
		{
			var fallback = DateRange.Last12Months();
			var start = from ?? fallback.From;
			var end = to ?? fallback.To;
			if (end < start)
			{
				(start, end) = (end, start);
			}
			return new DateRange(start.Date, end.Date);
		}
	}
}
