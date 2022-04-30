using Haskap.ToDoList.Application.UseCaseServices.Dtos;

namespace Haskap.ToDoList.Application.UseCaseServices.Contracts;

public interface IToDoListService
{
    Task AddToDoList(Guid ownerUserId, string name);
    Task DeleteToDoList(Guid ownerUserId, Guid toDoListId);
    Task UpdateToDoList(Guid ownerUserId, Guid toDoListId, string name);
    Task AddToDoItem(Guid ownerUserId, Guid toDoListId, ToDoItemInputDto toDoItemInputDto);
    Task DeleteToDoItem(Guid ownerUserId, Guid toDoListId, Guid toDoItemId);
    Task UpdateToDoItem(Guid ownerUserId, Guid toDoListId, Guid toDoItemId, ToDoItemInputDto toDoItemInputDto);
    Task MarkToDoListAsCompleted(Guid ownerUserId, Guid toDoListId);
    Task MarkToDoItemAsCompleted(Guid ownerUserId, Guid toDoListId, Guid toDoItemId);
    Task MarkToDoItemAsNotCompleted(Guid ownerUserId, Guid toDoListId, Guid toDoItemId);
    Task<IEnumerable<GetToDoLists_ToDoListOutputDto>> GetToDoLists(Guid ownerUserId);
    Task<IEnumerable<GetToDoItems_ToDoItemOutputDto>> GetToDoItems(Guid ownerUserId, Guid toDoListId);
}
