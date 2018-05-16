using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
using Copypasta.Domain.Interfaces;
using Copypasta.Models;
using Copypasta.StateMachine;
using PaperClip.Hotkeys.Interfaces;
using PaperClip.Trackers.Interfaces;
using Stateless;

namespace Copypasta.Controllers
{
    public class CopypastaController
    {
        private readonly IClipboard _clipboard;
        private readonly IClipboardHistoryManager _clipboardHistoryManager;
        private readonly IClipboardBindingManager _clipboardBindingManager;
        private readonly IHotkey _ctrlVHotkey;
        private readonly IHotkey _ctrlCHotkey;
        private readonly IHotkey _escHotkey;
        private readonly IKeyTracker _keyTracker;
        private readonly IInputSimulator _inputSimulator;
        private readonly StateMachine<CopypastaState, CopypastaTrigger> _copypastaStateMachine;
        private readonly StateMachine<CopypastaState, CopypastaTrigger>.TriggerWithParameters<Key> _keyPressedTrigger;

        private ClipboardDataModel _clipboardSwap;
        private HistoryRecordModel _historyRecordSwap;
        private bool _ctrlVHandled;
        private bool _ctrlCHandled;
        private bool _keyPressHandled;
        private bool _modifierPressHandled;
        private bool _escPressHandled;

        public CopypastaController(IClipboard clipboard, IClipboardHistoryManager clipboardHistoryManager,
            IClipboardBindingManager clipboardBindingManager, IHotkey ctrlVHotkey, IHotkey ctrlCHotkey, IHotkey escHotkey,
        {
            _clipboard = clipboard;
            _clipboardHistoryManager = clipboardHistoryManager;
            _clipboardBindingManager = clipboardBindingManager;
            _ctrlVHotkey = ctrlVHotkey;
            _ctrlCHotkey = ctrlCHotkey;
            _escHotkey = escHotkey;
            _keyTracker = keyTracker;
            _inputSimulator = inputSimulator;
            _notificationDispatcher = notificationDispatcher;
            _copypastaStateMachine = new StateMachine<CopypastaState, CopypastaTrigger>(CopypastaState.Idle);
            _keyPressedTrigger = _copypastaStateMachine.SetTriggerParameters<Key>(CopypastaTrigger.KeyPressed);

            ConfigureCopypastaStateMachine();
            ConfigureEventTriggers();
        }

        private void ConfigureEventTriggers()
        {
            _clipboard.Subscribe(notification =>
            {
                Console.WriteLine("ClipboardUpdated");
                _copypastaStateMachine.Fire(CopypastaTrigger.ClipboardUpdated);
            });
            _notificationDispatcher.Subscribe(notification =>
            {
                if (notification.Notification != null) { return; }

                Console.WriteLine("NotificationTimeout");
                _copypastaStateMachine.Fire(CopypastaTrigger.Timeout);
            });
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
                if (_escHotkey.IsPressed) { return; }

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
            _clipboardBindingManager.Subscribe(clipboardItem =>
            {
                Console.WriteLine("BindingAdded");
                _copypastaStateMachine.Fire(CopypastaTrigger.ClipboardBound);
            });
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
                .OnEntry(transition =>
                {
                    _ctrlCHandled = false;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    _notificationDispatcher.CloseNotification();

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
                    var clipboardData = _clipboard.ClipboardData;
                    _historyRecordSwap = _clipboardHistoryManager.AddHistoryRecord(Key.None, clipboardData);

                    _notificationDispatcher.ShowNotification(new CopyingNotificationModel());

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.BindingClipboardToKey)
                .Permit(CopypastaTrigger.ClipboardUpdated, CopypastaState.Copying)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.Pasting)
                .Ignore(CopypastaTrigger.EscPressed)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Ignore(CopypastaTrigger.Timeout)
                .Permit(CopypastaTrigger.ClipboardBound, CopypastaState.ShowingClipboardBinding)
                .OnEntryFrom(_keyPressedTrigger, key =>
                {
                    _ctrlCHandled = false;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    // Bind clipboard to key
                    _clipboardBindingManager.AddBinding(key, _historyRecordSwap.ClipboardData);

                    // Update clipboard history
                    _historyRecordSwap.Key = key;

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.ShowingClipboardBinding)
                .Permit(CopypastaTrigger.ClipboardUpdated, CopypastaState.Copying)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Permit(CopypastaTrigger.CtrlVPressed, CopypastaState.Pasting)
                .Permit(CopypastaTrigger.EscPressed, CopypastaState.Idle)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Permit(CopypastaTrigger.Timeout, CopypastaState.Idle)
                .OnEntry(() =>
                {
                    _ctrlCHandled = false;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    _notificationDispatcher.ShowNotification(new BoundNotificationModel
                    {
                        Key = _historyRecordSwap.Key,
                        ClipboardData = _historyRecordSwap.ClipboardData
                    });

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
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

                    _notificationDispatcher.ShowNotification(new PastingNotificationModel());

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.MovingDataToClipboard)
                .Permit(CopypastaTrigger.ClipboardUpdated, CopypastaState.SimulatingCtrlV)
                .Ignore(CopypastaTrigger.CtrlCPressed)
                .Ignore(CopypastaTrigger.CtrlVPressed)
                .Ignore(CopypastaTrigger.EscPressed)
                .Ignore(CopypastaTrigger.KeyPressed)
                .Ignore(CopypastaTrigger.ModifierPressed)
                .Ignore(CopypastaTrigger.Timeout)
                .Permit(CopypastaTrigger.NoDataOnKey, CopypastaState.Idle)
                .OnEntryFrom(_keyPressedTrigger, key =>
                {
                    _ctrlCHandled = true;
                    _ctrlVHandled = true;
                    _escPressHandled = false;
                    _keyPressHandled = false;
                    _modifierPressHandled = false;

                    var clipboardItem = _clipboardBindingManager.GetBindingData(key);

                    if (clipboardItem == null)
                    {
                        _copypastaStateMachine.Fire(CopypastaTrigger.NoDataOnKey);
                        return;
                    }

                    // Save clipboard to swap - to restore previous data
                    _clipboardSwap = _clipboard.ClipboardData;

                    // Write to clipboard
                    _clipboard.ClipboardData = clipboardItem;

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.SimulatingCtrlV)
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

                    // TODO: This can be removed once support for delayed clipboard rendering is added
                    // Wait for programs to paste data once Ctrl+V is pressed
                    Thread.Sleep(100);

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });

            _copypastaStateMachine.Configure(CopypastaState.RestoringClipboard)
                .Permit(CopypastaTrigger.ClipboardUpdated, CopypastaState.Idle)
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

                    // Restore previous data
                    _clipboard.ClipboardData = _clipboardSwap;

                    Console.WriteLine($"State={_copypastaStateMachine.State}");
                });
        }
    }
}
