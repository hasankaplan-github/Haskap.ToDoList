using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices.Dtos;

public class ToDoListOutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public int ItemCount { get; set; }
}
