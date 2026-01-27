using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plms.Identity.Persistence.Context;
using Plms.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using Plms.Identity.Application.Interfaces;

namespace Plms.Identity.Persistence
{
  public static class ServiceRegistration
  {
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
      // DbContext'i SQL Server'a bağlıyoruz
      services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      // --- EKLENECEK SATIR ---
      // Biri IApplicationDbContext isterse, ona yukarıda oluşturduğun AppDbContext'i ver.
      services.AddScoped<IApplicationDbContext>(provider => provider.GetService<AppDbContext>());
      // -----------------------

      // Identity Ayarları
      services.AddIdentity<AppUser, AppRole>(options =>
      {
        // Şifre kuralları (Geliştirme aşamasında gevşek bırakabilirsin)
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;

        // User Ayarları
        options.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders(); // Şifre sıfırlama tokenları vb. için
    }
  }
}