using System;
using System.Collections.Generic;

namespace Company.Infrastructure.Context;

public partial class Role
{
    public int Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<Usersinrole> Usersinroles { get; set; } = new List<Usersinrole>();
}
