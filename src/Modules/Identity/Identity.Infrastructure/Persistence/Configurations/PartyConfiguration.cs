using Identity.Domain.Parties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations
{
    internal class PartyConfiguration : IEntityTypeConfiguration<Party>
    {
        public void Configure(EntityTypeBuilder<Party> builder)
        {
            builder.ToTable("Parties");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasConversion(id => id.Value, value => PartyId.From(value))
                .HasColumnName("PartyId")
                .ValueGeneratedNever();

            builder
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(p => p.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .HasMany(p => p.PartyOrganizations)
                .WithOne(po => po.Party)
                .HasForeignKey(po => po.PartyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
