using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviate.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);            
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Phone).HasMaxLength(20).IsRequired(false);
            builder.Property(u => u.Role).HasConversion<int>().IsRequired();
            builder.Property(u => u.RegistrationDate).IsRequired();
            builder.Property(u => u.UpdatedDate).IsRequired();
            
            builder.HasIndex(u => u.Email).IsUnique();

            //builder.HasMany(u => u.Bookings)
            //    .WithOne(b => b.User)
            //    .HasForeignKey(b => b.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
