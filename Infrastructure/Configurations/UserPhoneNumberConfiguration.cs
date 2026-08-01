using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserPhoneNumberConfiguration : IEntityTypeConfiguration<UserPhoneNumber>
    {
        public void Configure(EntityTypeBuilder<UserPhoneNumber> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_UserPhoneNumbers");
            builder.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();

            builder.HasOne(e => e.User)
                   .WithMany(u => u.PhoneNumbers)
                   .HasForeignKey(e => e.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_UserPhoneNumbers_Users_ApplicationUserId");
        }
    }
}
