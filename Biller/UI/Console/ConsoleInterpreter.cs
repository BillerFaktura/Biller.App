using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.Console
{
    public class ConsoleInterpreter
    {
        private ViewModel.MainWindowViewModel vm;

        public ConsoleInterpreter(ViewModel.MainWindowViewModel mainVM)
        {
            vm = mainVM;
        }

        public void InterpreteCommand(string command, int index = 0)
        {
            var commands = command.Split(new Char[] { ' ' });
            switch (commands[index].ToLower())
            {
                case "show":
                    Show(commands, index + 1);
                    break;
                case "exit":
                    
                    break;
            }
        }

        private void Show(string[] commands, int index)
        {
            if (index >= commands.Length)
                return;

            switch(commands[index].ToLower())
            {
                case "notification":
                    Notification(commands, index + 1);
                    break;
            }
        }

        /// <summary>
        /// Format: show notification [error]
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="index"></param>
        private void Notification(string[] commands, int index)
        {
            if (index >= commands.Length)
                return;

            if (commands[index] == "error")
            {
                string title = string.Empty;
                string text = string.Empty;
                string imageUri = string.Empty;
                if (index + 1 < commands.Length)
                {
                    index += 1;
                    title = commands[index];
                }
                if (index + 1 < commands.Length)
                {
                    index += 1;
                    text = commands[index];
                }
                if (index + 1 < commands.Length)
                {
                    index += 1;
                    imageUri = commands[index];
                }
                vm.NotificationManager.ShowNotification(new Biller.Controls.Notification.ErrorNotification() { Title = title, Message = text, ImageUrl = imageUri });
            }
            else
            {

            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
