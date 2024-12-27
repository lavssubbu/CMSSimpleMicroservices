namespace CartAPI.Models.DTO
{
    public class AddtoCartDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
