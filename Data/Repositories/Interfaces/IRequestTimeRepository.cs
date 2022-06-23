using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IRequestTimeRepository
    {
        void AddTime(RequestTime newDate);
        void ClearTable();
        List<string> GetAllTime();
    }
}