using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices.Contracts;
public interface IJwtGenerator
{
    string GenerateToken(IEnumerable<Claim> claims);
}
