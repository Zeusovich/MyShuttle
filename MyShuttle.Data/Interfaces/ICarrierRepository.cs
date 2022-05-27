using MyShuttle.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyShuttle.Data
{
    public interface ICarrierRepository
    {
        Task<int> AddAsync(Carrier carrier);
        Task<SummaryAnalyticInfo> GetAnalyticSummaryInfoAsync(int carrierId);
        Task<Carrier> GetAsync(int carrierId);
        Task<List<Carrier>> GetCarriersAsync(string filter);
        Task UpdateAsync(Carrier carrier);
    }
}
