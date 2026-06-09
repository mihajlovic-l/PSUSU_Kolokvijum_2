namespace Studiji_za_animaciju.Models
{
    public class Show
    {
        public int ShowId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

        // Foreign key
        public int StudioId { get; set; }
        public virtual Studio Studio { get; set; }
    }
}
