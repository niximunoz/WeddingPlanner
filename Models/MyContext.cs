#pragma warning disable CS8618

using Microsoft.EntityFrameworkCore;
namespace WeddingPlanner.Models;


public class MyContext : DbContext 
{   

    public DbSet<User> Users { get; set; } 
    public DbSet<Wedding> Weddings { get; set; } 
    public DbSet<Attendance> Assistances { get; set; } 
    public MyContext(DbContextOptions options) : base(options) { }    


    
}
