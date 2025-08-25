namespace DeveloperStore.Application.DTOs
{
    public class SaleItemDto
    {
        public ProductInfoDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}