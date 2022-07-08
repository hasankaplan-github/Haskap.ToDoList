using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Haskap.DddBase.Domain.Core;

namespace Haskap.ToDoList.Domain.Core.ToDoListAggregate.Exceptions;
public class YouAreNotOwnerOfThisListException : GeneralException
{
    public YouAreNotOwnerOfThisListException()
        : base("You Are Not Owner Of This List", HttpStatusCode.BadRequest)
    {
        
    }
}
