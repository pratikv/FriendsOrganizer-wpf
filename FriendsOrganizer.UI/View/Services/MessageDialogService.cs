using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FriendsOrganizer.UI.View.Services
{
    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }

    public interface IMessageDialogService
    {
        MessageDialogResult ShowOkCancelDialog(string text, string title);
        void ShowInfoDialog(string text);
    }

    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK ? MessageDialogResult.Ok : MessageDialogResult.Cancel;
        }

        public void ShowInfoDialog(string text)
        {
            MessageBox.Show(text);
        }
    }
}
