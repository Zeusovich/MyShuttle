﻿using Microsoft.AspNetCore.Mvc;
using MyShuttle.Data;
using MyShuttle.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyShuttle.API
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public class AnalyticsController : ControllerBase
    {
        private const int DefaultCarrierID = 0;
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICarrierRepository _carrierRepository;
        private readonly IRidesRepository _ridesRepository;

        public AnalyticsController(IDriverRepository driverRepository, IVehicleRepository vehicleRepository, ICarrierRepository carrierRepository, IRidesRepository ridesRepository)
        {
            _driverRepository = driverRepository;

            _vehicleRepository = vehicleRepository;
            _carrierRepository = carrierRepository;
            _ridesRepository = ridesRepository;
        }

        [HttpGet("topdrivers")]
        public async Task<ICollection<Driver>> GetTopDrivers()
        {
            return await _driverRepository.GetTopDriversAsync(DefaultCarrierID, GlobalConfig.TOP_NUMBER);
        }

        [HttpGet("topvehicles")]
        public async Task<ICollection<Vehicle>> GetTopVehicles()
        {
            return await _vehicleRepository.GetTopVehiclesAsync(DefaultCarrierID, GlobalConfig.TOP_NUMBER);
        }

        [HttpGet("summary")]
        public async Task<SummaryAnalyticInfo> GetSummaryInfo()
        {
            return await _carrierRepository.GetAnalyticSummaryInfoAsync(DefaultCarrierID);
        }

        [HttpGet("rides")]
        public async Task<RidesAnalyticInfo> GetRidesEvolution()
        {
            DateTime fromFilter = DateTime.UtcNow.AddDays(-30);

            var labels = new List<int>();
            var to = DateTime.UtcNow;

            var date = fromFilter;
            while (DateTime.Compare(date, to) < 0)
            {
                labels.Add(date.Day);
                date = date.AddDays(1);
            }

            int carrierId = DefaultCarrierID;

            IEnumerable<RideResult> evolution = await _ridesRepository.GetRidesEvolutionAsync(carrierId, fromFilter);

            var values = new int[labels.Count];
            foreach (var item in evolution)
            {
                int index = (int)(item.Date - fromFilter).TotalDays;
                values[index] = item.Value;
            }

            RideGroupInfo rides = new RideGroupInfo()
            {
                Days = labels,
                Values = values
            };

            int ridesCount = await _ridesRepository.LastDaysRidesAsync(carrierId, fromFilter);
            int passengersCount = await _ridesRepository.LastDaysPassengersAsync(carrierId, fromFilter);

            return new RidesAnalyticInfo()
            {
                LastDaysRides = ridesCount,
                LastDaysPassengers = passengersCount,
                RidesEvolution = rides
            };
        }
    }
}