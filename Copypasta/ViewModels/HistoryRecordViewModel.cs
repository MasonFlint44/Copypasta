﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Copypasta.Annotations;
using Copypasta.Models;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels
{
    public class HistoryRecordViewModel : IHistoryRecordViewModel
    {
        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(Key));
            }
        }

        private string _clipboardText;
        public string ClipboardText
        {
            get => _clipboardText;
            set
            {
                _clipboardText = value;
                OnPropertyChanged(nameof(ClipboardText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryRecordViewModel(HistoryRecordModel historyRecord)
        {
            historyRecord.Subscribe(notification =>
            {
                Key = notification.Key.ToString();
                ClipboardText = GetText(notification.ClipboardData);
            });
        }

        public static string GetText(ClipboardDataModel clipboardData)
        {
            if (!clipboardData.ClipboardData.TryGetValue(DataFormats.UnicodeText.ToLower(), out var stream))
            {
                return string.Empty;
            }
            return Encoding.Unicode.GetString(stream.ToArray()).TrimEnd('\0');
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
