using System;
using System.Threading;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
using Copypasta.Models.Interfaces;
using Copypasta.StateMachine;
using Copypasta.ViewModels.Interfaces;
using PaperClip.Hotkeys.Interfaces;
using PaperClip.Trackers.Interfaces;
using Stateless;

namespace Copypasta.Controllers
{
    public class CopypastaController
    {
        private readonly IClipboardModel _clipboardModel;
        private readonly IClipboardHistoryModel _clipboardHistoryModel;
        private readonly IClipboardBindingsModel _clipboardBindingsModel;
        private readonly IHotkey _ctrlVHotkey;
        private readonly IHotkey _ctrlCHotkey;
        private readonly IHotkey _escHotkey;
        private readonly IKeyTracker _keyTracker;
        private readonly IInputSimulator _inputSimulator;
        private readonly INotificationViewModel _notificationViewModel;
        private readonly StateMachine<CopypastaState, CopypastaTrigger> _copypastaStateMachine;
        private readonly StateMachine<ClipboardWriterState, ClipboardWriterTrigger> _clipboardWriterStateMachine;
        private readonly StateMachine<CopypastaState, CopypastaTrigger>.TriggerWithParameters<Key> _keyPressedTrigger;
        private readonly StateMachine<ClipboardWriterState, ClipboardWriterTrigger>.TriggerWithParameters<IClipboardItemModel> _writingToClipboard;

        private IClipboardItemModel _clipboardSwap;
        private bool _ctrlVHandled;
        private bool _ctrlCHandled;
        private bool _keyPressHandled;
        private bool _modifierPressHandled;
        private bool _escPressHandled;

        public CopypastaController(IClipboardModel clipboardModel, IClipboardHistoryModel clipboardHistoryModel,
            IClipboardBindingsModel clipboardBindingsModel, IHotkey ctrlVHotkey, IHotkey ctrlCHotkey, IHotkey escHotkey, 
            IKeyTracker keyTracker, IInputSimulator inputSimulator, INotificationViewModel notificationViewModel)
        {
            _clipboardModel = clipboardModel;
            _clipboardHistoryModel = clipboardHistoryModel;
            _clipboardBindingsModel = clipboardBindingsModel;
            _ctrlVHotkey = ctrlVHotkey;
            _ctrlCHotkey = ctrlCHotkey;
            _escHotkey = escHotkey;
            _keyTracker = keyTracker;
            _inputSimulator = inputSimulator;
            _notificationViewModel = notificationViewModel;
            _copypastaStateMachine = new StateMachine<CopypastaState, CopypastaTrigger>(CopypastaState.Idle);
            _clipboardWriterStateMachine = new StateMachine<ClipboardWriterState, ClipboardWriterTrigger>(ClipboardWriterState.Idle);
            _keyPressedTrigger = _copypastaStateMachine.SetTriggerParameters<Key>(CopypastaTrigger.KeyPressed);
            _writingToClipboard =
                _clipboardWriterStateMachine.SetTriggerParameters<IClipboardItemModel>(ClipboardWriterTrigger.Write);

            ConfigureCopypastaStateMachine();
            ConfigureEventTriggers();
        }

        private void ConfigureEventTriggers()
        {
            _clipboardModel.ClipboardUpdated += (sender, args) =>
            {
                Console.WriteLine("ClipboardUpdated");
                _copypastaStateMachine.Fire(CopypastaTrigger.ClipboardUpdated);
                _clipboardWriterStateMachine.Fire(ClipboardWriterTrigger.ClipboardUpdated);
            };
            _notificationViewModel.NotificationTimeoutEvent += (sender, args) =>
            {
                Console.WriteLine("NotificationTimeout");
                _copypastaStateMachine.Fire(CopypastaTrigger.Timeout);
            };
            _ctrlVHotkey.HotkeyPressed += (sender, args) =>
            {
                Console.WriteLine($"CtrlVPressed: Handled={_ctrlVHandled}");
                args.Handled = _ctrlVHandled;
                _copypastaStateMachine.Fire(CopypastaTrigger.CtrlVPressed);
            };
            _ctrlCHotkey.HotkeyPressed += (sender, args) =>
            {
                Console.WriteLine($"CtrlCPressed: Handled={_ctrlCHandled}");
                args.Handled = _ctrlCHandled;
                _copypastaStateMachine.Fire(CopypastaTrigger.CtrlCPressed);
            };
            _keyTracker.KeyPressed += (sender, args) =>
            {
                if (_keyTracker.Modifiers != ModifierKeys.None) { return; }
                if(_escHotkey.IsPressed) { return; }

                Console.WriteLine($"KeyPressed: Key={args.Key}, Handled={_keyPressHandled}");
                args.Handled = _keyPressHandled;
                _copypastaStateMachine.Fire(_keyPressedTrigger, args.Key);
            };
            _keyTracker.KeyPressed += (sender, args) =>
            {
                if (_keyTracker.Modifiers == ModifierKeys.None) { return; }
                if (_ctrlCHotkey.IsPressed || _ctrlVHotkey.IsPressed) { return; }

                Console.WriteLine($"ModifierPressed: Key={args.Key}, Handled={_modifierPressHandled}");
                args.Handled = _modifierPressHandled;
                _copypastaStateMachine.Fire(CopypastaTrigger.ModifierPressed);
            };
            _escHotkey.HotkeyPressed += (sender, args) =>
            {
                Console.WriteLine($"EscPressed: Handled={_escPressHandled}");
                args.Handled = _escPressHandled;
                _copypastaStateMachine.Fire(CopypastaTrigger.EscPressed);
            };
        }

        private void ConfigureCopypastaStateMachine()
        {
            _copypastaStateMachine.Configure(CopypastaState.Idle)
                .Permit(CopypastaTrigger.ClipboardUpdated, CopypastaState.Copying)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.Pasting)
                .Ignore(CopypastaTrigger.EscPressed)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Ignore(CopypastaTrigger.Timeout)
                .OnEntry(() =>
                {
                    _ctrlCHandled = false;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    _notificationViewModel.HideNotification();

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.Copying)
                .PermitReentry(CopypastaTrigger.ClipboardUpdated)
                .Permit(CopypastaTrigger.CtrlCPressed, CopypastaState.Idle)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.Pasting)
                .Permit(CopypastaTrigger.EscPressed, CopypastaState.Idle)
                .Permit(CopypastaTrigger.KeyPressed, CopypastaState.BindingClipboardToKey)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Permit(CopypastaTrigger.Timeout, CopypastaState.Idle)
                .OnEntry(() =>
                {
                    _ctrlCHandled = true;
                    _ctrlVHandled = true;
                    _escPressHandled = true;
                    _keyPressHandled = true;
                    _modifierPressHandled = false;

                    // Update clipboard history
                    var clipboardItem = _clipboardModel.ClipboardData;
                    _clipboardHistoryModel.History.Add(clipboardItem);

                    _notificationViewModel.ShowNotification(CopypastaState.Copying);

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.BindingClipboardToKey)
                .SubstateOf(CopypastaState.Copying)
                .Permit(CopypastaTrigger.ClipboardUpdated, CopypastaState.Copying)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.Pasting)
                .Ignore(CopypastaTrigger.EscPressed)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Ignore(CopypastaTrigger.Timeout)
                .Permit(CopypastaTrigger.ClipboardBound, CopypastaState.Idle)
                .OnEntryFrom(_keyPressedTrigger, key =>
                {
                    _ctrlCHandled = false;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    // Bind clipboard to key
                    var clipboardItem = _clipboardHistoryModel.History[0];
                    _clipboardBindingsModel.AddBinding(key, clipboardItem);

                    Console.WriteLine($"State={_copypastaStateMachine.State}");

                    // TODO: ClipboardBindingsModel should fire event for this trigger
                    _copypastaStateMachine.Fire(CopypastaTrigger.ClipboardBound);
                });

            _copypastaStateMachine.Configure(CopypastaState.Pasting)
                .Permit(CopypastaTrigger.ClipboardUpdated, CopypastaState.Copying)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.Idle)
                .Permit(CopypastaTrigger.EscPressed, CopypastaState.Idle)
                .Permit(CopypastaTrigger.KeyPressed, CopypastaState.MovingDataToClipboard)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Permit(CopypastaTrigger.Timeout, CopypastaState.Idle)
                .OnEntry(() =>
                {
                    _ctrlCHandled = false;
                    _ctrlVHandled = false;
                    _escPressHandled = true;
                    _keyPressHandled = true;
                    _modifierPressHandled = false;

                    _notificationViewModel.ShowNotification(CopypastaState.Pasting);

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.MovingDataToClipboard)
                .SubstateOf(CopypastaState.Pasting)
                .Ignore(CopypastaTrigger.ClipboardUpdated)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Ignore(CopypastaTrigger.CtrlVPressed)
                .Ignore(CopypastaTrigger.EscPressed)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Ignore(CopypastaTrigger.Timeout)
                .Permit(CopypastaTrigger.NoDataOnKey, CopypastaState.Idle)
                .Permit(CopypastaTrigger.SavedDataToClipboard, CopypastaState.SimulatingCtrlV)
                .OnEntryFrom(_keyPressedTrigger, key =>
                {
                    _ctrlCHandled = true;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    var clipboardItem = _clipboardBindingsModel.GetData(key);

                    if (clipboardItem == null)
                    {
                        _copypastaStateMachine.Fire(CopypastaTrigger.NoDataOnKey);
                        return;
                    }

                    // Save clipboard to swap - to be restored
                    _clipboardSwap = _clipboardModel.ClipboardData;

                    Console.WriteLine($"State={_copypastaStateMachine.State}");

                    // Write to clipboard
                    _clipboardWriterStateMachine.Fire(_writingToClipboard, clipboardItem);
                });

            _copypastaStateMachine.Configure(CopypastaState.SimulatingCtrlV)
                .SubstateOf(CopypastaState.Pasting)
                .Ignore(CopypastaTrigger.ClipboardUpdated)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.RestoringClipboard)
                .Ignore(CopypastaTrigger.EscPressed)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Ignore(CopypastaTrigger.Timeout)
                .OnEntry(() =>
                {
                    _ctrlCHandled = true;
                    _ctrlVHandled = false;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    // Simulate key up for V key. This prevents interfering with simulated Ctrl+V when pasting from V key.
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_V);
                    // Simulate Ctrl+V
                    _inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);

                    // Wait for programs to paste data once Ctrl+V is pressed
                    Thread.Sleep(10);

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.RestoringClipboard)
                .SubstateOf(CopypastaState.Pasting)
                .Ignore(CopypastaTrigger.ClipboardUpdated)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.Pasting)
                .Ignore(CopypastaTrigger.EscPressed)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Ignore(CopypastaTrigger.Timeout)
                .Permit(CopypastaTrigger.SavedDataToClipboard, CopypastaState.Idle)
                .OnEntry(() =>
                {
                    _ctrlCHandled = false;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    Console.WriteLine($"State={_copypastaStateMachine.State}");

                    // Restore clipboard data
                    _clipboardWriterStateMachine.Fire(_writingToClipboard, _clipboardSwap);
                });

            _clipboardWriterStateMachine.Configure(ClipboardWriterState.Idle)
                .Permit(ClipboardWriterTrigger.Write, ClipboardWriterState.ClearingClipboard)
                .Ignore(ClipboardWriterTrigger.ClipboardUpdated);

            _clipboardWriterStateMachine.Configure(ClipboardWriterState.ClearingClipboard)
                .Permit(ClipboardWriterTrigger.ClipboardUpdated, ClipboardWriterState.WritingClipboard)
                .OnEntryFrom(_writingToClipboard, clipboardItem =>
                {
                    Console.WriteLine($"Clipboard={clipboardItem.ClipboardData.GetData(typeof(string))}");
                    _clipboardModel.ClipboardData = clipboardItem;
                });

            _clipboardWriterStateMachine.Configure(ClipboardWriterState.WritingClipboard)
                .Permit(ClipboardWriterTrigger.ClipboardUpdated, ClipboardWriterState.Idle)
                .OnExit(() =>
                {
                    _copypastaStateMachine.Fire(CopypastaTrigger.SavedDataToClipboard);
                });
        }
    }
}
