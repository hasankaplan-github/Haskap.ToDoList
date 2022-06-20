using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Providers;
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
