using Microsoft.Win32;
using System;
using System.Windows.Controls;
using System.Windows;
using System.IO;

namespace FlowingFiles.Behaviors
{
    public class OpenFileBehavior : System.Windows.Interactivity.Behavior<Button>
    {
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(OpenFileBehavior));

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public static readonly DependencyProperty FileBytesProperty =
            DependencyProperty.Register("FileBytes", typeof(byte[]), typeof(OpenFileBehavior));

        public byte[] FileBytes
        {
            get { return (byte[])GetValue(FileBytesProperty); }
            set { SetValue(FileBytesProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(OpenFileBehavior), new PropertyMetadata("All Files|*.*"));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.Click += OpenFile;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OpenFile;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = Filter,
                Title = "Select a file"
            };

            if (openFileDialog.ShowDialog() == true)
                try
                {
                    FileName = openFileDialog.FileName;
                    FileBytes = File.ReadAllBytes(openFileDialog.FileName);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
    }
}