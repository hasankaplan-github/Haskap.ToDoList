using Haskap.DddBase.Application.Contracts;
using Haskap.ToDoList.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.Contracts;

public interface IAccountService : IUseCaseService
{
    Task<LoginOutputDto> LoginAsync(LoginInputDto loginInputDto);
}
