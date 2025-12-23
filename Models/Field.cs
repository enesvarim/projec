using System.ComponentModel.DataAnnotations;

namespace projec.Models
{
    public class Field
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal HourlyRate { get; set; } // Saatlik Ücret

        public bool IsActive { get; set; } = true;
    }
}
