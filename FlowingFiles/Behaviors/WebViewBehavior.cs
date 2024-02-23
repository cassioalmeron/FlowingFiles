using System;
using System.Windows.Interactivity;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;

namespace FlowingFiles.Behaviors
{
    public class WebViewBehavior : Behavior<WebView2>
    {
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(WebViewBehavior), new PropertyMetadata(null, OnUrlChanged));

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebViewBehavior behavior && e.NewValue is string newUrl)
            {
                string uri = "file:///" + newUrl.Replace('\\', '/');
                behavior.AssociatedObject.Source = new Uri(uri);
            }
        }
    }
}