using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext.EntityTypeConfigurations
{
    public class ToDoListEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.Core.ToDoListAggregate.ToDoList>
    {
        public override void Configure(EntityTypeBuilder<Domain.Core.ToDoListAggregate.ToDoList> builder)
        {
            base.Configure(builder);

            builder.HasMany(x => x.ToDoItems)
                .WithOne()
                .HasForeignKey(x => x.OwnerToDoListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
