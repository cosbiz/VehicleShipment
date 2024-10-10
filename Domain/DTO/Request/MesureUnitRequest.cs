namespace Domain.DTO.Request
{
    public class MesureUnitRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string UserId { get; set; }  // Only the UserId is needed
    }
}
