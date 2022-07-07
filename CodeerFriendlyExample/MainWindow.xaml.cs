using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeerFriendlyExample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public OriginalClass OriginalClass { get; set; }

        public SecureStringClass SecureString {get; set; }

        public MainWindow()
        {
            InitializeComponent();

            OriginalClass = new OriginalClass("Test3");

            SecureString = new SecureStringClass("Password");
        }

        private void text_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            text2.Text = "Clicked!!";
        }

        private void text3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            text3.Text = OriginalClass.OriginalName;
        }
    }
}
