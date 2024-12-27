namespace CartAPI.Models.DTO
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<CartItemDto> Items { get; set; }
    }
}
