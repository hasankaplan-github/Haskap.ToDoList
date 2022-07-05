using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.Dtos.DataTable;

public class SearchParam
{
    public string Value { get; set; }
    public bool Regex { get; set; }
}
