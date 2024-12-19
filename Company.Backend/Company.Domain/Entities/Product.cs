using System;
using System.Collections.Generic;

namespace Company.Infrastructure;

public partial class Product
{
    public int Productid { get; set; }

    public string Productname { get; set; } = null!;

    public int Inventory { get; set; }

    public decimal Price { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
