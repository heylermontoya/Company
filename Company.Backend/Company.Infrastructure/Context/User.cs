using System;
using System.Collections.Generic;

namespace Company.Infrastructure.Context;

public partial class User
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Usersinrole> Usersinroles { get; set; } = new List<Usersinrole>();
}
