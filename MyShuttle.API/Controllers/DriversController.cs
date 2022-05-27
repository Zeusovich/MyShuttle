using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyShuttle.Data;
using MyShuttle.Model;

namespace MyShuttle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class DriversController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;
        private const int DefaultCarrierID = 0;

        public DriversController(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        [HttpGet]
        public async Task<Driver> Get(int id)
        {
            return await _driverRepository.GetAsync(DefaultCarrierID, id);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Driver>> Get(string filter, int pageSize, int pageCount)
        {
            if (string.IsNullOrEmpty(filter))
                filter = string.Empty;
            return await _driverRepository.GetDriversAsync(DefaultCarrierID, filter, pageSize, pageCount);
        }

        [HttpGet("filter")]
        public async Task<IEnumerable<Driver>> GetDriversFilter()
        {
            return await _driverRepository.GetDriversFilterAsync(DefaultCarrierID);
        }

        [HttpGet("count")]
        public async Task<int> GetCount(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                filter = string.Empty;
            return await _driverRepository.GetCountAsync(DefaultCarrierID, filter);
        }

        [HttpPost]
        public async Task<int> Post([FromBody] Driver driver)
        {
            driver.CarrierId = DefaultCarrierID;
            return await _driverRepository.AddAsync(driver);
        }

        [HttpPut]
        public async Task Put([FromBody] Driver driver)
        {
            driver.CarrierId = DefaultCarrierID;
            await _driverRepository.UpdateAsync(driver);
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await _driverRepository.DeleteAsync(id);
        }
    }
}
