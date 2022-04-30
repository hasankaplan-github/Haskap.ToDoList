using Haskap.DddBase.Infrastructure.Data.EfCoreDbContexts.NpgsqlDbContext;
using Haskap.ToDoList.Domain.Core.ToDoListAggregate;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(builder);
    }

    public DbSet<Domain.Core.ToDoListAggregate.ToDoList> ToDoList { get; set; }
    public DbSet<ToDoItem> ToDoItem { get; set; }
    public DbSet<User> User { get; set; }
}