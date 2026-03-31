using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spring2026_Project3_sostamps.Models;

namespace Spring2026_Project3_sostamps.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<MovieTemp> Movies { get; set; }
    public DbSet<ActorTemp> Actors { get; set; }
    
}