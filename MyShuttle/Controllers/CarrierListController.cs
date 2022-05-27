using Microsoft.AspNetCore.Mvc;
using MyShuttle.Data;
using MyShuttle.Web.Models;

namespace MyShuttle.Web.Controllers
{
    public class CarrierListController : Controller
    {
        private readonly ICarrierRepository _carrierRepository;

        public CarrierListController(ICarrierRepository carrierRepository)
        {
            _carrierRepository = carrierRepository;
        }

        public async Task<IActionResult> Index(SearchViewModel searchVM)
        {
            string searchString = searchVM?.SearchString;
            var carriers = await _carrierRepository.GetCarriersAsync(searchString);
            var model = new CarrierListViewModel(carriers);

            return View("Index", model);
        }
    }
}
