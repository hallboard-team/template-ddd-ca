using Ca.Infrastructure.Modules.Auth.Postgres.Models;
using Ca.Infrastructure.Persistence.EFCore.Postgres.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ca.Infrastructure.Persistence.EFCore.Postgres.Configurations;

internal sealed class AppUserPostgresConfig : IEntityTypeConfiguration<AppUserPostgres>
{
    /// <summary>
    ///     Extended configs from Persistence.EFCore.Postgres.Extensions.ModelBuilderExtensions.cs
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<AppUserPostgres> builder)
    {
        builder.HasKey(appUser => appUser.Id);
    }
}