using Application.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess;

namespace Coin_Api_Info.Extensions;

public static class MigrationExtension
{
    public static async Task ApplyMigration(this IApplicationBuilder builder)
    {
        var localScope = builder.ApplicationServices.CreateScope();

        var dbContext = localScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
