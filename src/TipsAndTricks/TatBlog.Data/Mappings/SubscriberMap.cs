using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
  public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
  {
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
      builder.ToTable("Subscribers");

      builder.HasKey(s => s.Id);

      builder.Property(s => s.Email)
        .IsRequired()
        .HasMaxLength(150);

      builder.Property(s => s.SubscribedDate)
        .HasColumnType("datetime");

      builder.Property(s => s.UnsubscribedDate)
        .HasColumnType("datetime");

      builder.Property(s => s.UnsubscribedCausal)
        .HasMaxLength(500);

      builder.Property(s => s.FlagIsBlockSubByAdmin)
        .HasDefaultValue(false);

      builder.Property(s => s.AdminNotes)
        .HasMaxLength(500);
    }
  }
}
