using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissileLauncher
{
    internal class Launcher: ILauncher
    {
        public void LaunchMissile()
        {
            Console.WriteLine("Blast off!");
        }
    }
}
