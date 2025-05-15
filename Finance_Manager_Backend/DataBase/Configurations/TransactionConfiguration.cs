using Finance_Manager_Backend.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance_Manager_Backend.DataBase.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("name");

        builder.Property(t => t.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)")
            .HasDefaultValue(0m)
            .HasColumnName("price");

        builder.Property(t => t.Date)
            .IsRequired()
            .HasColumnName("date");

        builder.Ignore(t => t.Photo);

        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.CategoryId)
            .IsRequired()
            .HasColumnName("category_fk");

        builder.HasOne(t => t.InnerCategory)
            .WithMany()
            .HasForeignKey(t => t.InnerCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.InnerCategoryId)
            .IsRequired(false)
            .HasColumnName("inner_category_fk");

        builder.HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.UserId)
            .IsRequired()
            .HasColumnName("user_fk");

        builder.HasIndex(t => t.UserId);
    }
}
