using Org.BouncyCastle.Crypto.Agreement.JPake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StarEyes_GUI.Commands {

    /// <summary>
    /// 操作指令的基础
    /// </summary>
    public class DelegateCommand : ICommand {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object parameter) {
            if (this.CanExecuteCommand == null) {
                return true;
            }
            else return CanExecuteCommand(parameter);
        }

        public void Execute(object parameter) {
            if (this.ExecuteCommand == null) {
                return;
            }
            this.ExecuteCommand(parameter);
        }

        public Action<object>? ExecuteCommand { get; set; }
        public Func<object, bool>? CanExecuteCommand { get; set; }

        public DelegateCommand(Action<object> ExecuteCommand, Func<object, bool> CanExecuteCommand) {
            this.ExecuteCommand = ExecuteCommand;
            this.CanExecuteCommand = CanExecuteCommand;
        }

        public DelegateCommand(Action<object> ExecuteCommand) : this(ExecuteCommand, null) { }
    }
}
