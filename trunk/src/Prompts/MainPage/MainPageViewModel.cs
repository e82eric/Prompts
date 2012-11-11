using System.Windows.Input;
using Prompts.Infastructure;

namespace Prompts.MainPage
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ICommand _showHideCommand;
        private string _showHideText;

        public MainPageViewModel()
        {
            _showHideText = "Show";
            _showHideCommand = new RelayCommand(OnShowHide);
        }

        private void OnShowHide()
        {
            IsCollapsed = IsCollapsed == false ? true : false;
        }

        private bool _isCollapsed;
        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set
            {
                _isCollapsed = value;
                RaisePropertyChanged("IsCollapsed");
            }
        }

        public string ShowHideText
        {
            get { return _showHideText; }
            set
            {
                _showHideText = value;
                RaisePropertyChanged("ShowHideText");
            }
        }

        public ICommand ShowHideCommand { get { return _showHideCommand; } }
    }
}
