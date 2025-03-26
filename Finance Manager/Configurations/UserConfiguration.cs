using Finance_Manager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance_Manager.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("email");

        builder.Property(u => u.Password)
            .IsRequired()
            .HasColumnName("password");

        builder.Property(u => u.Balance)
            .IsRequired()
            .HasColumnType("decimal(12,2)")
            .HasColumnName("balance");       
    }
}
