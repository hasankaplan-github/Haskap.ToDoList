namespace Haskap.ToDoList.Application.Dtos;

public class MarkAsCompleted_ToDoItemInputDto
{
    public Guid ToDoItemId { get; set; }
    public Guid OwnerToDoListId { get; set; }
}
