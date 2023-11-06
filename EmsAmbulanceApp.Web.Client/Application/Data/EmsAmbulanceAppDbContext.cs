using EmsAmbulanceApp.Web.Client.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.Intrinsics.X86;

namespace EmsAmbulanceApp.Web.Client.Application.Data;

public class EmsAmbulanceAppDbContext : IdentityDbContext
{
    public EmsAmbulanceAppDbContext(DbContextOptions<EmsAmbulanceAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Entities.Client> Clients { get; set; }
    public DbSet<AmbulanceRequest> AmbulanceRequests { get; set; }
    public DbSet<TripStatus> TripStatuses { get; set; }
    public DbSet<OtpEntry> OtpEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<Domain.Entities.Client>(b =>
            {
                b.HasMany<TripStatus>().WithOne().HasForeignKey(ts => ts.ClientId);
                b.HasMany<OtpEntry>().WithOne().HasForeignKey(ts => ts.ClientId);
                b.HasMany<AmbulanceRequest>().WithOne().HasForeignKey(ts => ts.ClientId);
            });

        builder.Entity<IdentityUserClaim<string>>(b =>
        {
            // Primary key
            b.HasKey(uc => uc.Id);

            // Maps to the AspNetUserClaims table
            b.ToTable("AspNetUserClaims");
        });

        builder.Entity<IdentityUserLogin<string>>(b =>
        {
            // Composite primary key consisting of the LoginProvider and the key to use
            // with that provider
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            // Limit the size of the composite key columns due to common DB restrictions
            b.Property(l => l.LoginProvider).HasMaxLength(128);
            b.Property(l => l.ProviderKey).HasMaxLength(128);

            // Maps to the AspNetUserLogins table
            b.ToTable("AspNetUserLogins");
        });

        builder.Entity<IdentityUserToken<string>>(b =>
        {
            // Composite primary key consisting of the UserId, LoginProvider and Name
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            // Limit the size of the composite key columns due to common DB restrictions
            b.Property(t => t.LoginProvider).HasMaxLength(256);
            b.Property(t => t.Name).HasMaxLength(256);

            // Maps to the AspNetUserTokens table
            b.ToTable("AspNetUserTokens");
        });

        builder.Entity<IdentityRole<string>>(b =>
        {
            // Primary key
            b.HasKey(r => r.Id);

            // Index for "normalized" role name to allow efficient lookups
            b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();

            // Maps to the AspNetRoles table
            b.ToTable("AspNetRoles");

            // A concurrency token for use with the optimistic concurrency checking
            b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            b.Property(u => u.Name).HasMaxLength(256);
            b.Property(u => u.NormalizedName).HasMaxLength(256);

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each Role can have many entries in the UserRole join table
            b.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany<IdentityRoleClaim<string>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });

        builder.Entity<IdentityRoleClaim<string>>(b =>
        {
            // Primary key
            b.HasKey(rc => rc.Id);

            // Maps to the AspNetRoleClaims table
            b.ToTable("AspNetRoleClaims");
        });

        builder.Entity<IdentityUserRole<string>>(b =>
        {
            // Primary key
            b.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            b.ToTable("AspNetUserRoles");
        });
    }
}
