namespace Haskap.ToDoList.Application.UseCaseServices.Dtos;

public class MarkAsNotCompleted_ToDoItemInputDto
{
    public Guid ToDoItemId { get; set; }
    public Guid OwnerToDoListId { get; set; }
}
