using Finance_Manager_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance_Manager_Backend.Configurations;

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

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("password_hash");

        builder.Property(u => u.Salt)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("salt");

        builder.Property(u => u.Balance)
            .IsRequired()
            .HasColumnType("decimal(12,2)")
            .HasColumnName("balance");       
    }
}
