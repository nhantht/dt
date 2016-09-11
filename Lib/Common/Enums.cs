using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Common
{
    public enum Action
    {
        REPORTINVALIDURL = 1,
        REGISTERED = 2,
        ACTIVEREGISTRATION = 3,
        CHANGEDPASSWORD = 4,
        LOCKED = 5,
        UNLOCKED = 6
    }
}
