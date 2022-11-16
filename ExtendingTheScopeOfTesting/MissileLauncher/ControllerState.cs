using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    public enum ControllerState
    {
        WAITING_FOR_FIRST_KEY,
        WAITING_FOR_SECOND_KEY,
        WAITING_FOR_UNLOCK_CODE,
        WAITING_FOR_LAUNCH_COMMAND,
        LAUNCHED
    }
}
