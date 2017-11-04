using System.Threading.Tasks;

namespace BoltJwt.Domain.Model.Abstractions
{
    public interface IRepository<T>
    {
        T Add(T item);
        void Update(T item);
        Task<T> GetAsync(int id);
    }
}