using System.Data.Entity;
using Studiji_za_animaciju.Models;

namespace Studiji_za_animaciju.Data
{
    public class AnimationContext : DbContext
    {
        public AnimationContext() : base("name=AnimationContext")
        {
        }

        public DbSet<Studio> Studios { get; set; }
        public DbSet<Show> Shows { get; set; }
    }
}
