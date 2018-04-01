namespace Copypasta.StateMachine
{
    public enum CopypastaTrigger
    {
        ClipboardUpdated,
        CtrlVPressed,
        CtrlCPressed,
        KeyPressed,
        ModifierPressed,
        EscPressed,
        Timeout,
        ClipboardBound,
        NoDataOnKey,
        SavedDataToClipboard
    }
}
