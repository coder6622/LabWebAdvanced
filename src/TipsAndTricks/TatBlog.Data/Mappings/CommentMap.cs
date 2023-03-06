﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
  public class CommentMap : IEntityTypeConfiguration<Comment>
  {
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
      builder.ToTable("Comments");

      builder.HasKey(c => c.Id);

      builder.Property(c => c.Content)
        .IsRequired()
        .HasMaxLength(1500);

      builder.Property(c => c.Feedback)
        .IsRequired(false)
        .HasMaxLength(500);

      builder.Property(c => c.NameUserComment)
        .HasMaxLength(100);

      builder.Property(c => c.Email)
        .IsRequired()
        .HasMaxLength(150);

      builder.Property(c => c.CommentedDate)
        .HasColumnType("datetime");

      builder.Property(c => c.IsApproved)
        .IsRequired()
        .HasDefaultValue(false);

      builder.HasOne(c => c.Post)
        .WithMany(p => p.Comments)
        .HasForeignKey(c => c.PostId)
        .HasConstraintName("FK_Comments_Posts")
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
