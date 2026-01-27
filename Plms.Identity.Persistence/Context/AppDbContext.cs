using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plms.Identity.Domain.Entities;
using System;
using System.Reflection;
using Plms.Identity.Application.Interfaces;

namespace Plms.Identity.Persistence.Context
{
  // DİKKAT: Burada AppUser, AppRole ve Key Tipi olarak Guid veriyoruz.
  public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IApplicationDbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Domain'deki entity'leri DbSet olarak ekliyoruz
    // (Identity tabloları zaten IdentityDbContext içinde var, onları yazmana gerek yok)
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      // Identity tablolarının (AspNetUsers, AspNetRoles vb.) oluşması için bu satır ŞART!
      base.OnModelCreating(builder);

      // Configurations klasöründeki tüm ayarları (RefreshTokenConfiguration vb.) otomatik uygula
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
  }
}