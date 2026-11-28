using Microsoft.AspNetCore.Mvc;
using SalesProjetDapper.Web.Dtos.Orders;
using SalesProjetDapper.Web.Services.Orders;

namespace SalesProjetDapper.Web.Controllers
{
	public class OrdersController : Controller
	{
		private readonly IOrderService _orderService;

		public OrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public IActionResult Index() => View();

		[HttpGet]
		public async Task<IActionResult> Search([FromQuery] OrderFilter orderFilter, CancellationToken ct)
		{
			var result = await _orderService.SearchAsync(orderFilter, ct);
			return Json(result);
		}


		[HttpGet]
		public async Task<IActionResult> Cities(CancellationToken ct)
		{
			var cities = await _orderService.GetCitiesAsync(ct);
			return Json(cities);
		}

	}
}
