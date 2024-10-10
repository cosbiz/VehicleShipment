using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ItemData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Prevent EF from auto-generating it
        public int Code { get; set; } = 10000;  // Starting default value
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int RefereceNumber { get; set; }

        // Foreign key for MeasureUnit
        public int MesureUnitId { get; set; }
        public MesureUnit MesureUnit { get; set; }

        // Foreign key for User (creator of the item)
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
