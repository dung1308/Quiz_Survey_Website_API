namespace survey_quiz_app.Core;

public interface IGenericRepository<T,TID> where T : class
{
    Task<IEnumerable<T>> All();
     Task<T?> GetById(TID id);
    Task<bool> Add(T entity);
    Task<bool> Update(T entity);
    Task<bool> Delete(T entity);
}