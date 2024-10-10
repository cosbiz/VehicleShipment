namespace Domain.Entities
{
    public class MesureUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        // Navigation property for the related ItemData
        public ICollection<ItemData> ItemDataList { get; set; }

        // Foreign key for User (creator of the MeasureUnit)
        public string UserId { get; set; }
        public User User { get; set; }
    }
}