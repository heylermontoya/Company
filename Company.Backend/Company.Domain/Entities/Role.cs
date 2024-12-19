using System;
using System.Collections.Generic;

namespace Company.Infrastructure;

public partial class Role
{
    public int Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public bool Cancreatetransaction { get; set; }

    public bool Candeletetransaction { get; set; }

    public virtual ICollection<Usersinrole> Usersinroles { get; set; } = new List<Usersinrole>();
}
