using Haskap.DddBase.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Application.UseCaseServices.Dtos.DataTable;

namespace Haskap.ToDoList.Application.UseCaseServices.Contracts;

public interface IToDoListService : IUseCaseService
{
    Task AddToDoList(ToDoListInputDto toDoListInputDto);
    Task DeleteToDoList(Guid toDoListId);
    Task UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto);
    Task MarkToDoListAsCompleted(MarkAsCompleted_ToDoListInputDto toDoListDto);
    Task<JqueryDataTableResult> GetToDoLists(JqueryDataTableParam jqueryDataTableParam);
}
