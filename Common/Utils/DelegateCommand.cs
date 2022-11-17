using System;
using System.Windows.Input;

namespace StarEyes_GUI.Common.Utils {

    /// <summary>
    /// 指令基础类，执行自定义指令
    /// </summary>
    public class DelegateCommand : ICommand {
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 判断指令是否可执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) {
            if (CanExecuteCommand == null) {
                return true;
            }
            else return CanExecuteCommand(parameter);
        }

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) {
            if (ExecuteCommand == null) {
                return;
            }
            ExecuteCommand(parameter);
        }

        /// <summary>
        /// 定义执行指令的方法委托
        /// </summary>
        public Action<object> ExecuteCommand { get; set; }
        
        /// <summary>
        /// 定义判断指令可执行性的方法委托
        /// </summary>
        public Func<object, bool> CanExecuteCommand { get; set; }

        /// <summary>
        /// DelegateCommand 双参数构造函数
        /// </summary>
        /// <param name="ExecuteCommand"></param>
        /// <param name="CanExecuteCommand"></param>
        public DelegateCommand(Action<object> executeCommand, Func<object, bool> canExecuteCommand) {
            ExecuteCommand = executeCommand;
            CanExecuteCommand = canExecuteCommand;
        }

        /// <summary>
        /// DelegateCommand 单参数构造函数
        /// </summary>
        /// <param name="ExecuteCommand"></param>
        public DelegateCommand(Action<object> executeCommand) : this(executeCommand, null) { }
    }
}
