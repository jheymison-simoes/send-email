using Marques.EFCore.SnakeCase;
using Microsoft.EntityFrameworkCore;
using SendEmail.Business.Models;

namespace SendEmail.Data;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
    }

    public DbSet<LogEmail> LogEmails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
        modelBuilder.ToSnakeCase();
        modelBuilder.HasSequence<long>("LogEmailSequence").StartsAt(1).IncrementsBy(1);
        
        base.OnModelCreating(modelBuilder);
    }
}