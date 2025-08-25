namespace DeveloperStore.Application.DTOs
{
    public class CreateCartDto
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartItemDto> Products { get; set; }
    }
}