using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using Unity;
using CodeerFriendlyExampleClasses;
using CodeerFriendlyExampleInterfaces;

namespace CodeerFriendlyExample
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public Interface1 InterfaceImpl { get; set; }

        public IUnityContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Container = new UnityContainer();

            Container.RegisterType<Interface1, Class1>();
            Container.RegisterType<ExampleInterface, ExampleClass>();

            InterfaceImpl = UnityContainerExtensions.Resolve<Interface1>(Container);

            base.OnStartup(e);

            var splash = new SplashWindow();
            splash.Show();

            Thread.Sleep(3 * 1000);

            var win = new MainWindow();
            this.MainWindow = win;
            splash.Close();
            win.Show();
        }
    }
}
