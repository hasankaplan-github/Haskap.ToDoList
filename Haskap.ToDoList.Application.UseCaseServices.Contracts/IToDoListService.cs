using Haskap.ToDoList.Application.UseCaseServices.Dtos;

namespace Haskap.ToDoList.Application.UseCaseServices.Contracts;

public interface IToDoListService
{
    Task AddToDoList(string name);
    Task DeleteToDoList(Guid toDoListId);
    Task UpdateToDoList(Guid toDoListId, string name);
    Task AddToDoItem(Guid toDoListId, ToDoItemInputDto toDoItemInputDto);
    Task DeleteToDoItem(Guid toDoListId, Guid toDoItemId);
    Task UpdateToDoItem(Guid toDoListId, Guid toDoItemId, ToDoItemInputDto toDoItemInputDto);
    Task MarkToDoListAsCompleted(Guid toDoListId);
    Task MarkToDoItemAsCompleted(Guid toDoListId, Guid toDoItemId);
    Task MarkToDoItemAsNotCompleted(Guid toDoListId, Guid toDoItemId);
    Task<IEnumerable<GetToDoLists_ToDoListOutputDto>> GetToDoLists();
    Task<IEnumerable<GetToDoItems_ToDoItemOutputDto>> GetToDoItems(Guid toDoListId);
}
