using System.Threading.Tasks;

namespace BoltJwt.Model.Abstractions
{
    public interface IRepository<T>
    {
        T Add(T role);
        void Update(T role);
        Task<T> GetAsync(int id);
    }
}