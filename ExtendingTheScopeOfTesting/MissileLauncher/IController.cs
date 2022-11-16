using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    public interface IController
    {
        //ControllerState State { get;  }

        void InsertKey();

        void RemoveKey();

        void SubmitUnlockCode(string code);

        void SubmitLaunchCommand();

        void ResetController();
    }
}
