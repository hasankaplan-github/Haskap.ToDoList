using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Haskap.DddBase.Domain.Core;

namespace Haskap.ToDoList.Domain.Core.UserAggregate.Exceptions;
public class UnauthorizedAccessException : GeneralException
{
    public UnauthorizedAccessException()
        : base("__L__UnauthorizedAccess__L__", HttpStatusCode.Unauthorized)
    {
    }
}
