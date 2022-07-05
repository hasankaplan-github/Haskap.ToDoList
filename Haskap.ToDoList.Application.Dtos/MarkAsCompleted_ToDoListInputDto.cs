using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.Dtos;

public class MarkAsCompleted_ToDoListInputDto
{
    public Guid ToDoListId { get; set; }
}
