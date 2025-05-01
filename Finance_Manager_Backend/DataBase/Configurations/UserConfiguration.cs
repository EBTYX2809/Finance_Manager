using Finance_Manager_Backend.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance_Manager_Backend.DataBase.Configurations;

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

        builder.Property(u => u.Role)
            .IsRequired()
            .HasDefaultValue("User")
            .HasMaxLength(5)
            .HasColumnName("role");

        builder.Property(u => u.Balance)
            .IsRequired()
            .HasColumnType("decimal(12,2)")
            .HasColumnName("balance");

        builder.Property(u => u.PrimaryCurrency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD")
            .HasColumnName("primary_currency");

        builder.Property(u => u.SecondaryCurrency1)
            .IsRequired(false)
            .HasMaxLength(3)
            .HasColumnName("secondary_currency_1");

        builder.Property(u => u.SecondaryCurrency2)
            .IsRequired(false)
            .HasMaxLength(3)
            .HasColumnName("secondary_currency_2");
    }
}
