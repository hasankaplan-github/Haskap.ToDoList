using Haskap.ToDoList.Domain.Core.ToDoListAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext.EntityTypeConfigurations
{
    public class ToDoItemEntityTypeConfiguration : BaseEntityTypeConfiguration<ToDoItem>
    {
        public override void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            base.Configure(builder);
        }
    }
}
