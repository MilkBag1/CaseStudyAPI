namespace CaseStudyAPI.Helpers
{
    public class OrderDetailsHelper
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string? ProductId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int QtyOrdered { get; set; }
        public int QtySold { get; set; }
        public int QtyOnBackOrder { get; set; }
        public string? ProductName { get; set; }
        public string? DateCreated { get; set; }


    }
}
