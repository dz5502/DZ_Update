using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DZ_Update_ServerFileManager.Base
{
    public class BaseCommand : ICommand
    {
        public static List<String> FilterStr { get; set; } = new List<string>() { "Move", "Close" };//, "Save", "Close" 

        public Action<object> ExecuteAction;
        public Func<bool> CanExecuteAction { get; set; }

        private BaseCommand filterCommand;
        private Brush selectColor;

        public BaseCommand(Action<object> action)
        {
            ExecuteAction = action;
        }

        public BaseCommand(Action<object> action, Func<bool> action2)
        {
            ExecuteAction = action;
            CanExecuteAction = action2;
        }

        public BaseCommand(BaseCommand filterCommand)
        {
            this.filterCommand = filterCommand;
        }

        public BaseCommand(Brush selectColor)
        {
            this.selectColor = selectColor;
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteAction == null)
                return true;

            return CanExecuteAction.Invoke();
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            bool re = true;
            if (CanExecuteAction != null)
            {
                re = CanExecuteAction.Invoke();
            }

            if (re)
            {
                bool needDo = true;
                foreach (var item in FilterStr)
                {
                    if (ExecuteAction.Method.Name.Contains(item) || (ExecuteAction.Method.Name.Contains(item.ToLower())))
                    {
                        needDo = false;
                        break;
                    }
                }

                if (needDo == false)
                {
                    ExecuteAction(parameter);
                }
                else
                {
                    ExecuteAction(parameter);
                }
            }
        }
    }
}
