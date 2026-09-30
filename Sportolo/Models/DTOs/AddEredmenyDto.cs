namespace Sportolo.Models.DTOs
{
    public class AddEredmenyDto
    {
        
        public string Competition { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int SportoloId { get; set; }
    }
}
