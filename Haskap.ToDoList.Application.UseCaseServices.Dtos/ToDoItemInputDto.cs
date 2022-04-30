namespace Haskap.ToDoList.Application.UseCaseServices.Dtos;

public class ToDoItemInputDto
{
    public string Content { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
}
