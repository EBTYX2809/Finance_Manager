﻿using Finance_Manager_Backend.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance_Manager_Backend.DataBase.Configurations;

public class SavingConfiguration : IEntityTypeConfiguration<Saving>
{
    public void Configure(EntityTypeBuilder<Saving> builder)
    {
        builder.ToTable("savings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("name");

        builder.Property(s => s.Goal)
            .IsRequired()
            .HasColumnType("decimal(10,2)")
            .HasDefaultValue(0m)
            .HasColumnName("goal");

        builder.Property(s => s.CurrentAmount)
            .HasDefaultValue(0m)
            .HasColumnType("decimal(10,2)")
            .HasColumnName("current_amount");

        builder.HasOne(s => s.User)
            .WithMany(u => u.Savings)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.UserId)
            .IsRequired()
            .HasColumnName("user_fk");

        builder.HasIndex(s => s.UserId);
    }
}
