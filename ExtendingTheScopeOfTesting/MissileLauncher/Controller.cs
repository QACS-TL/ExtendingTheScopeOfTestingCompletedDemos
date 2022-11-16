using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("MissileLauncher")]

namespace MissileLauncher
{
    public class Controller: IController
    {
        public enum ControllerState
        {
            WAITING_FOR_FIRST_KEY,
            WAITING_FOR_SECOND_KEY,
            WAITING_FOR_UNLOCK_CODE,
            WAITING_FOR_LAUNCH_COMMAND,
            LAUNCHED
        }

        public ControllerState State { get; private set; }

        private ILauncher launcher;

        public Controller():this (new Launcher())
        {

        }

        public Controller(ILauncher launcher)
        {
            this.launcher = launcher;
            State = ControllerState.WAITING_FOR_FIRST_KEY;
        }

        public void InsertKey()
        {
            if (State == ControllerState.WAITING_FOR_FIRST_KEY)
                State = ControllerState.WAITING_FOR_SECOND_KEY;
            else if (State == ControllerState.WAITING_FOR_SECOND_KEY)
                State = ControllerState.WAITING_FOR_UNLOCK_CODE;
        }

        public void RemoveKey()
        {
            if (State == ControllerState.WAITING_FOR_SECOND_KEY)
                State = ControllerState.WAITING_FOR_FIRST_KEY;
            else if (State == ControllerState.WAITING_FOR_UNLOCK_CODE)
                State = ControllerState.WAITING_FOR_SECOND_KEY;
            else if (State == ControllerState.WAITING_FOR_LAUNCH_COMMAND)
                State = ControllerState.WAITING_FOR_SECOND_KEY;
        }

        public void SubmitUnlockCode(string code)
        {
            if (State == ControllerState.WAITING_FOR_UNLOCK_CODE)
            {
                if (code == "1234")
                {
                    State = ControllerState.WAITING_FOR_LAUNCH_COMMAND;
                }
            }
        }

        public void SubmitLaunchCommand()
        {
            if (State == ControllerState.WAITING_FOR_LAUNCH_COMMAND)
            {
                launcher.LaunchMissile();
                State = ControllerState.LAUNCHED;
            }
        }

        public void ResetController()
        {
            State = ControllerState.WAITING_FOR_FIRST_KEY;
        }
    }
}