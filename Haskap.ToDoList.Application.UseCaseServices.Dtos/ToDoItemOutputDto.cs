using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices.Dtos;

public class ToDoItemOutputDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsOverDue { get; set; }
    public Guid OwnerToDoListId { get; set; }
    public DateTime DateAdded { get; set; }
}
