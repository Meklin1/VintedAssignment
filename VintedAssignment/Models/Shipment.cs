namespace VintedExercise.Models
{
    public class Shipment
    {

        public DateTime Date { get; set; }
        public PackageSize Size { get; set; }
        public PostalService Carrier { get; set; }
        public double FullPrice { get; set; }
        public double DiscountAmount { get; set; }
        public bool IsValid { get; set; }
        public string? Line { get; set; }

    }
}
