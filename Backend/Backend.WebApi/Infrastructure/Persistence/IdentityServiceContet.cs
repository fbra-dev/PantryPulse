using Backend.WebApi.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.WebApi.Infrastructure.Persistence;

public class IdentityServiceContext : IdentityDbContext<IdentityUser>
{
    private readonly ConnectionStrings _connectionStrings;

    public IdentityServiceContext(DbContextOptions<IdentityServiceContext> options,IOptions<ConnectionStrings> connectionStrings)
        : base(options)
    {
        _connectionStrings = connectionStrings.Value;
    }   
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(this._connectionStrings.DefaultConnection);

        base.OnConfiguring(optionsBuilder);
    }
}