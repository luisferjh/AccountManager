using Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manager.Data.Mapping
{
    public class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(x => x.WebAccountName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.UserAccount).HasMaxLength(50);
            builder.Property(x => x.Password).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();

            builder.ToTable("Account")
                .HasKey(p => p.IdAccount);
        }
    }
}
