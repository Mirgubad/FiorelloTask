namespace FiorelloTaskFronToBack.ViewModels.Basket
{
    public class BasketListitemViewModel
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public int StockQuantity { get; set; }

        public string Photoname { get; set; }
        public string Title { get; set; }

    }
}
