namespace FlowingFiles.MVVM
{
    public class OptionViewModel : ViewModelBase
    {
        public string Description { get; set; }
        public string Path { get; set; }
        public bool Required { get; set; }
    }
}