using Haskap.DddBase.Application.Contracts;
using Haskap.ToDoList.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.Contracts;

public interface IToDoItemService : IUseCaseService
{
    Task<IEnumerable<ToDoItemOutputDto>> GetToDoItems(Guid toDoListId);
    Task AddToDoItem(ToDoItemInputDto toDoItemInputDto);
    Task DeleteToDoItem(Guid ownerToDoListId, Guid toDoItemId);
    Task UpdateToDoItem(Guid toDoItemId, ToDoItemInputDto toDoItemInputDto);
    Task MarkToDoItemAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto);
    Task MarkToDoItemAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto);
}
