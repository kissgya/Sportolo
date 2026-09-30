namespace Sportolo.Models
{
    public class Eredmeny
    {
        public int Id { get; set; }
        public string Competition { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime UpdateTime { get; set; }

        public DateTime ResultTime { get; set; }

        public int SportoloId {  get; set; }    
    }
}
