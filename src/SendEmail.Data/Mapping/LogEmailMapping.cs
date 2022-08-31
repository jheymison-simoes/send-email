using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SendEmail.Business.Models;

namespace SendEmail.Data.Mapping;

public class LogEmailMapping : IEntityTypeConfiguration<LogEmail>
{
    public void Configure(EntityTypeBuilder<LogEmail> builder)
    {
        builder.ToTable("log_email");
        builder.HasKey(le => le.Id);
        builder.Property(le => le.Id).HasColumnOrder(0);
        builder.Property(le => le.Code).HasColumnOrder(1).HasDefaultValueSql("nextval('\"LogEmailSequence\"')");
        builder.Property(le => le.CreatedAt).HasColumnOrder(2);
        builder.Property(le => le.Email).IsRequired().HasColumnOrder(3);
        builder.Property(le => le.Subject).HasColumnOrder(4);
        builder.Property(le => le.Message).IsRequired().HasMaxLength(2048).HasColumnOrder(5);
    }
}