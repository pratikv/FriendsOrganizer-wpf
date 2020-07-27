using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FriendsOrganizer.UI.View.Services
{
    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }

    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);
        Task ShowInfoDialogAsync(string text);
    }

    public class MessageDialogService : IMessageDialogService
    {
        public async Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title)
        {
            var metroWindow = App.Current.MainWindow as MetroWindow;
            var result = await metroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative ? MessageDialogResult.Ok : MessageDialogResult.Cancel;
        }

        public async Task ShowInfoDialogAsync(string text)
        {
            var metroWindow = App.Current.MainWindow as MetroWindow;
            await metroWindow.ShowMessageAsync("Info", text, MessageDialogStyle.Affirmative);
        }
    }
}
