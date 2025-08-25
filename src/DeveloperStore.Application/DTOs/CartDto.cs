namespace DeveloperStore.Application.DTOs
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartItemDto> Products { get; set; }
    }
}