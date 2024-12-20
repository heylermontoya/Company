using Company.Infrastructure;

namespace Company.Domain.Tests.DataBuilder
{
    public class ProductBuilder
    {
        private int _productId;
        private string _productName;
        private int _inventory;
        private decimal _price;
        private bool _isDeleted;
        private ICollection<Transaction> _transactions;

        public ProductBuilder()
        {
            _productId = 0; // Default ID
            _productName = "Default Product Name";
            _inventory = 0; // Default inventory
            _price = 0.0m; // Default price
            _isDeleted = false; // Default to not deleted
            _transactions = new List<Transaction>(); // Empty transaction list by default
        }

        public Product Build()
        {
            return new Product
            {
                Productid = _productId,
                Productname = _productName,
                Inventory = _inventory,
                Price = _price,
                Isdeleted = _isDeleted,
                Transactions = _transactions
            };
        }

        public ProductBuilder WithProductId(int productId)
        {
            _productId = productId;
            return this;
        }

        public ProductBuilder WithProductName(string productName)
        {
            _productName = productName;
            return this;
        }

        public ProductBuilder WithInventory(int inventory)
        {
            _inventory = inventory;
            return this;
        }

        public ProductBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public ProductBuilder WithIsDeleted(bool isDeleted)
        {
            _isDeleted = isDeleted;
            return this;
        }

        public ProductBuilder WithTransactions(ICollection<Transaction> transactions)
        {
            _transactions = transactions;
            return this;
        }
    }
}
