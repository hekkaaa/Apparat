using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IChartLossRepository
    {
        int AddHostname(Loss newHost);
        void ClearTable();
        Task<List<Loss>> GetAllHostInfo();
        Loss GetHostById(int id);
        void UpdateLoss(Loss newValue);
    }
}