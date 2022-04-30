using Haskap.ToDoList.Domain.Core.ToDoListAggregate;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(x => x.UserName);
            builder.OwnsOne(x => x.Password);
            builder.OwnsOne(x => x.Name);
        }
    }
}
