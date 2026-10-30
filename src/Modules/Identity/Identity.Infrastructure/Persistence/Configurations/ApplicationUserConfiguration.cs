using Identity.Domain.Parties;
using Identity.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // ApplicationUser.PartyId is not mapped by the base IdentityDbContext,
            // so it needs an explicit value conversion to be stored as a Guid.
            builder
                .Property(u => u.PartyId)
                .HasConversion(id => id.Value, value => new PartyId(value))
                .IsRequired();

            // The user-side FK of the 1:1 User <-> Party relationship.
            // Deleting a Party is blocked while any user still references it,
            // so identity rows are never deleted by a cascade.
            builder
                .HasOne<Party>()
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.PartyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AspNetUsers_Parties_PartyId");
        }
    }
}
