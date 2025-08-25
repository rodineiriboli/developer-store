namespace DeveloperStore.Application.DTOs
{
    public class UpdateCartDto
    {
        public DateTime Date { get; set; }
        public List<CartItemDto> Products { get; set; }
    }
}