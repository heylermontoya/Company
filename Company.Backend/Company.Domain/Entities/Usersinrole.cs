using System;
using System.Collections.Generic;

namespace Company.Infrastructure;

public partial class Usersinrole
{
    public int Userid { get; set; }

    public int Roleid { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
