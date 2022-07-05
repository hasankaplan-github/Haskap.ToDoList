using Haskap.DddBase.Application.Contracts;
using Haskap.ToDoList.Application.Dtos;
using Haskap.ToDoList.Application.Dtos.DataTable;

namespace Haskap.ToDoList.Application.Contracts;

public interface IToDoListService : IUseCaseService
{
    Task AddToDoList(ToDoListInputDto toDoListInputDto);
    Task DeleteToDoList(Guid toDoListId);
    Task UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto);
    Task MarkToDoListAsCompleted(MarkAsCompleted_ToDoListInputDto toDoListDto);
    Task<JqueryDataTableResult> GetToDoLists(JqueryDataTableParam jqueryDataTableParam);
}
