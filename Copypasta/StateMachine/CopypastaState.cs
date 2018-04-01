namespace Copypasta.StateMachine
{
    public enum CopypastaState
    {
        Idle,
        Copying,
        Pasting,
        MovingDataToClipboard,
        SimulatingCtrlV,
        BindingClipboardToKey,
        RestoringClipboard,
        ClearingClipboard
    }
}
