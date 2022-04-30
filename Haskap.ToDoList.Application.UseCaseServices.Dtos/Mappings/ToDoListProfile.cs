using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices.Dtos.Mappings
{
    public class ToDoListProfile : Profile
    {
        public ToDoListProfile()
        {
            CreateMap<Domain.Core.ToDoListAggregate.ToDoList, GetToDoLists_ToDoListOutputDto>();
        }
    }
}
