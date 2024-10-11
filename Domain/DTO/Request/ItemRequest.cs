namespace Domain.DTO.Request
{
    public class ItemRequest
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int RefereceNumber { get; set; }
        public int MesureUnitId { get; set; }
        public string UserId { get; set; }
    }
}
