using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.ToDoList.Domain.Core.UserAggregate.Exceptions;
public class UserNotFoundException : GeneralException
{
    public UserNotFoundException()
        : base("__L__UserNotFound__L__", HttpStatusCode.NotFound)
    {
    }
}