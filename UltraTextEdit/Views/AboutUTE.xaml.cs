using Microsoft.UI.Xaml.Controls;
using System.Reflection;
using System;

namespace UltraTextEdit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutUTE : Page
    {
        public AboutUTE()
        {
            this.InitializeComponent();
            Assembly assembly = typeof(App).Assembly;
            CompileDateText.Text = "Built " + GetBuildDate(assembly) + " UTC";
        }

        private static DateTime GetBuildDate(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<BuildDateAttribute>();
            return attribute != null ? attribute.DateTime : default(DateTime);
        }
    }
}
