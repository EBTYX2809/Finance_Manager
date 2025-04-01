using Finance_Manager_Backend.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.Configurations;

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