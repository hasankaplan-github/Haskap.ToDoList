using Haskap.DddBase.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices.Contracts;

public interface IAccountService : IUseCaseService
{
    Task<LoginOutputDto> LoginAsync(LoginInputDto input);
}
