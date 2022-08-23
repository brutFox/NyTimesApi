using NyTimesSharedModels.Models;

namespace NyTimesServices.Contracts
{
    public interface INyTimesTopNewsServices
    {
        Task<Root> GetDataFromNyTimesAsync(string section);

        Task SaveNyTimesDataSync(Root data);
    }
}
