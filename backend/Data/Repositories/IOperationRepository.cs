using OriginSolutions.Entities;
namespace OriginSolutions.Data.Repositories
{
    public interface IOperationRepository<T> where T : OperationBase
    {
        public Task<IEnumerable<T>> GetAllByUBK(string ubk);
        public Task<IEnumerable<T>> GetAllByCard(string card);
        public Task Add(T instance);
    }
}