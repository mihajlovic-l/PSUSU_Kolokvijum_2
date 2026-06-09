using System.Collections.Generic;

namespace Studiji_za_animaciju.Models
{
    public class Studio
    {
        public int StudioId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        // Navigation property for convenience (not used for persistence here)
        public virtual ICollection<Show> Shows { get; set; }
    }
}
