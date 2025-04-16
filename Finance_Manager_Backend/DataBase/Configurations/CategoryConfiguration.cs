using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Finance_Manager_Backend.BusinessLogic.Models;

namespace Finance_Manager_Backend.DataBase.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("name");

        builder.Property(c => c.IsIncome)
            .IsRequired()
            .HasColumnName("is_income");

        builder.Property(c => c.Icon)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("icon_path");

        builder.Property(c => c.ColorForBackground)
            .IsRequired()
            .HasMaxLength(7)
            .HasColumnName("background_color");

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.InnerCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(c => c.ParentCategoryId);
    }
}