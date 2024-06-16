using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DataAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
    public DbSet<CryptocurrencyPrice> CryptocurrencyPrices { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cryptocurrency>().
            HasOne(a => a.Price).
            WithOne(a=>a.Cryptocurrency).
            HasForeignKey<CryptocurrencyPrice>(a=>a.CryptocurrencyId).
            OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
