
using xUnitTesting.Data.Entities;

namespace xUnitTesting.Services;

public interface ITodoService
{
    Task<List<Todo>> GetAllAsync();
    Task SaveAsync(Todo newTodo);
}