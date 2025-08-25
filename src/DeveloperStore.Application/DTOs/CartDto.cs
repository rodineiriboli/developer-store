namespace DeveloperStore.Application.DTOs
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartItemDto> Products { get; set; }
    }
}