using Microsoft.EntityFrameworkCore;
 
namespace ActivityCenter.Models
{
    public class ActivityContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ActivityContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get; set;}
        public DbSet<Activity> Activities {get; set;}

        public DbSet<Rsvp> Rsvps {get; set;}
    }
}