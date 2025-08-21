namespace DeveloperStore.Application.DTOs
{
    public class CreateSaleDto
    {
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public CustomerInfoDto Customer { get; set; }
        public BranchInfoDto Branch { get; set; }
        public List<SaleItemDto> Items { get; set; }
    }
}