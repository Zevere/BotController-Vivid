using System.Threading.Tasks;
using GraphQL.Types;
using Vivid.Data.Abstractions.Entities;
using Vivid.GraphQL.Models;

namespace Vivid.GraphQL
{
    public interface IQueryResolver
    {
        Task<UserDto> CreateUserAsync(ResolveFieldContext<object> context);
        
        Task<UserDto> GetUserAsync(ResolveFieldContext<object> context);
        
        Task<TaskList> CreateTaskListAsync(ResolveFieldContext<object> context);
        
        Task<TaskList[]> GetTaskListsForUserAsync(ResolveFieldContext<UserDto> context);
        
        Task<TaskItemDto> CreateTaskItemAsync(ResolveFieldContext<object> context);
        
        Task<TaskItemDto[]> GetTaskItemsForListAsync(ResolveFieldContext<TaskList> context);
    }
}