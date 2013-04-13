using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prompts.Prompting.Controls
{
    public class DoubleClickContentControl : ContentControl
    {
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); } 
            set { SetValue(CommandProperty, value); }
        }

        public DependencyProperty CommandProperty
            = DependencyProperty.Register(
                "Command"
                , typeof (ICommand)
                , typeof (DoubleClickContentControl)
                , null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public DependencyProperty CommandParameterProperty
            = DependencyProperty.Register(
                "CommandParameter"
                , typeof (object)
                , typeof (DoubleClickContentControl)
                , null);

        private DateTime _lastLeftClick;

        public DoubleClickContentControl()
        {
            MouseLeftButtonUp += DoubleClickContentControlMouseLeftButtonUp;
            _lastLeftClick = DateTime.MinValue;
        }

        void DoubleClickContentControlMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clickTime = DateTime.Now;
            var span = clickTime - _lastLeftClick;

            if(span.TotalMilliseconds <= 300)
            {
                Command.Execute(CommandParameter);
            }

            _lastLeftClick = clickTime;
        }
    }
 
}
