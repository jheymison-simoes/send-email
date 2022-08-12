using Marques.EFCore.SnakeCase;
using Microsoft.EntityFrameworkCore;

namespace SendEmail.Data;

public class SqlContext : DbContext
{
    public SqlContext()
    {
    }
    
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
        
        modelBuilder.ToSnakeCase();
        
        modelBuilder.HasSequence<long>("StateSequence").StartsAt(1).IncrementsBy(1);
    }
}