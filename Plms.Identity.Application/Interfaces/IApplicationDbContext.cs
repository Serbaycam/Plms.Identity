using Microsoft.EntityFrameworkCore;
using Plms.Identity.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Plms.Identity.Application.Interfaces
{
  public interface IApplicationDbContext
  {
    DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}