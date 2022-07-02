using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Providers;
public interface IJwtProvider
{
    string GenerateToken(IEnumerable<Claim> claims);
}
