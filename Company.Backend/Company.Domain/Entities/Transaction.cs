using System;
using System.Collections.Generic;

namespace Company.Infrastructure;

public partial class Transaction
{
    public int Transactionid { get; set; }

    public int Productid { get; set; }

    public int Userid { get; set; }

    public int Quantity { get; set; }

    public bool Isdeleted { get; set; }

    public DateTime? Transactiondate { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
