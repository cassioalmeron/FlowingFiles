using System.Windows.Input;

namespace FlowingFiles.MVVM
{
    public class FileViewModel : ViewModelBase
    {
        public FileViewModel()
        {
            CleanFileNameCommand = new RelayCommand(CleanFileName, x => Status == FileStatusEnum.Filled);
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                SetProperty(ref _fileName, value, () => FileName);
                OnPropertyChanged(() => Status);
            }
        }

        private OptionViewModel _option;
        public OptionViewModel Option
        {
            get { return _option; }
            set { SetProperty(ref _option, value, () => Option); }
        }

        public FileStatusEnum Status
        {
            get
            {
                if (!string.IsNullOrEmpty(_fileName))
                    return FileStatusEnum.Filled;
                if (Option.Required)
                    return FileStatusEnum.EmptyRequired;
                return FileStatusEnum.EmptyNotRequided;
            }
        }

        public ICommand CleanFileNameCommand { get; private set; }

        private void CleanFileName()
        {
            FileName = "";
        }
    }
}