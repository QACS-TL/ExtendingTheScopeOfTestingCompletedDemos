using FakeItEasy;
using Xunit;
using MissileLauncher;

namespace MissileLauncherTests
{
    public class ControllerTests
    {
        [Fact]
        public void EnsureUnlockCanHappenIfStateIsSetToWAITING_FOR_FIRST_KEY()
        {
            Controller controller = new Controller();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_FIRST_KEY);
        }

        [Fact]
        public void EnsureUnlockCanHappenIfStateIsSetToWAITING_FOR_SECOND_KEY()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_SECOND_KEY);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_UNLOCK_CODEIfTwoKeysAreInserted()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_UNLOCK_CODE);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_SECOND_KEYIfOnlyOneKeyIsInsertedAndCorrectCodeHasBeenEntered()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.SubmitUnlockCode("1234");
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_SECOND_KEY);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_LAUNCH_COMMANDIfTwoKeysAreInsertedAndCorrectCodeHasBeenEntered()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1234");
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_LAUNCH_COMMAND);
        }

        [Fact]
        public void EnsureStateStaysSetToWAITING_FOR_UNLOCK_CODEIfTwoKeysAreInsertedAndInvalidCodeHasBeenEntered()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1235");
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_UNLOCK_CODE);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_SECOND_KEYIfTwoKeysInsertedValidCodeEnteredAndRemoveKeyIsCalled()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1234");
            controller.RemoveKey();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_SECOND_KEY);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_SECOND_KEYIfTwoKeysInsertedAndRemoveKeyIsCalled()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            controller.RemoveKey();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_SECOND_KEY);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_FIRST_KEYIfOneKeyIsInsertedAndRemoveKeyIsCalled()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.RemoveKey();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_FIRST_KEY);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_SECOND_KEYIfOneKeyIsInsertedAndValidUnlockCodeIsEntered()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.SubmitUnlockCode("1234");
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_SECOND_KEY);
        }

        [Fact]
        public void EnsureStateIsSetToWAITING_FOR_FIRST_KEYIfNoKeyIsInsertedAndValidUnlockCodeIsEntered()
        {
            Controller controller = new Controller();
            controller.SubmitUnlockCode("1234");
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_FIRST_KEY);
        }

        [Fact]
        public void EnsureStateStaysSetToWAITING_FOR_FIRST_KEYIfTwoKeysAreInsertedAndAvalidCodeHasBeenEnteredAndTwoKeysAreRemoved()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1235");
            controller.RemoveKey();
            controller.RemoveKey();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_FIRST_KEY);
        }

        [Fact]
        public void EnsureMissileIsLaunchedIfTwoKeysAreInsertedAndAvalidCodeHasBeenEnteredAndSubmitLauuchIsCalled()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1234");
            controller.SubmitLaunchCommand();
            Assert.True(controller.State == Controller.ControllerState.LAUNCHED);
        }

        [Fact]
        public void EnsureMissileIsNotLaunchedIfOnlyTwoKeysAreInsertedAndSubmitLaunchIsCalled()
        {
            Controller controller = new Controller();
            controller.InsertKey();
            controller.InsertKey();
            controller.SubmitLaunchCommand();
            Assert.True(controller.State == Controller.ControllerState.WAITING_FOR_UNLOCK_CODE);
        }

        [Fact]
        public void EnsureSuccessfulLaunchOdMissile()
        {
            ILauncher launcher = A.Fake<ILauncher>();
            Controller controller = new Controller(launcher);

            controller.InsertKey();
            controller.InsertKey();
            controller.RemoveKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1234");
            controller.SubmitLaunchCommand();

            A.CallTo(() => launcher.LaunchMissile()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void EnsureUnSuccessfulLaunchOdMissileWrongUnlockCode()
        {
            ILauncher launcher = A.Fake<ILauncher>();
            Controller controller = new Controller(launcher);

            controller.InsertKey();
            controller.InsertKey();
            controller.RemoveKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1235");
            controller.SubmitLaunchCommand();

            A.CallTo(() => launcher.LaunchMissile()).MustNotHaveHappened();
        }

        [Fact]
        public void EnsureSpecificMethodsHaveBeenInvokedInCorrectSequenceForASuccessfulLaunch()
        {
            //Note this test isn't actually testing anything other than itself
            var controller = A.Fake<IController>();

            controller.InsertKey();
            controller.InsertKey();
            controller.RemoveKey();
            controller.InsertKey();
            controller.SubmitUnlockCode("1234");
            controller.SubmitLaunchCommand();

            A.CallTo(() => controller.InsertKey()).MustHaveHappened()
                .Then(A.CallTo(() => controller.InsertKey()).MustHaveHappened())
                .Then(A.CallTo(() => controller.SubmitUnlockCode("1234")).MustHaveHappened())
                .Then(A.CallTo(() => controller.SubmitLaunchCommand()).MustHaveHappened());
        }
    }
}