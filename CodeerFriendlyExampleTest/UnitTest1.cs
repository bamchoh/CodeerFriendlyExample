using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using CodeerFriendlyExample;
using Unity;

namespace CodeerFriendlyExampleTest
{
    [TestClass]
    public class UnitTest1
    {
        WindowsAppFriend _app;

        [TestInitialize]
        public void TestInitialize()
        {
            var execPath = @"C:\Users\bamch\source\repos\CodeerFriendlyExample\CodeerFriendlyExample\bin\Debug\CodeerFriendlyExample.exe";
            var process = Process.Start(execPath);
            while(process.MainWindowHandle == IntPtr.Zero)
            {
                process.Refresh();
                Thread.Sleep(10);
            }

            var prevHND = process.MainWindowHandle;
            while (process.MainWindowHandle == prevHND)
            {
                process.Refresh();
                Thread.Sleep(10);
            }

            _app = new WindowsAppFriend(process);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var process = Process.GetProcessById(_app.ProcessId);
            process.CloseMainWindow();
            _app.Dispose();
            process.WaitForExit();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var app = _app.Type<Application>().Current;

            var win = app.MainWindow;

            Assert.AreEqual("TEST", (string)win.text.Text);

            Assert.AreEqual("StaticString", (string)_app.Type("CodeerFriendlyExample.StaticClass").StaticMethod());

            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);

            var container = app.Container;

            var impl = _app.Type(GetType()).GetInterface1Container(container);

            impl.Add(10, 20);

            Assert.AreEqual(30, (int)impl.Value);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mainWindow = _app.WaitForIdentifyFromTypeFullName("CodeerFriendlyExample.MainWindow");
            Assert.AreEqual("TEST2", (string)mainWindow.Dynamic().text2.Text);

            OnMouseLeftButtonDown(mainWindow.Dynamic().text);

            Assert.AreEqual("Clicked!!", (string)mainWindow.Dynamic().text2.Text);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var mainWindow = _app.WaitForIdentifyFromTypeFullName("CodeerFriendlyExample.MainWindow");
            Assert.AreEqual("TEST3", (string)mainWindow.Dynamic().text3.Text);

            OnMouseLeftButtonDown(mainWindow.Dynamic().text3);

            Assert.AreEqual("Test3", (string)mainWindow.Dynamic().text3.Text);

            var originalClass = _app.Type().CodeerFriendlyExample.OriginalClass("HogeHoge");

            mainWindow.Dynamic().OriginalClass = originalClass;

            OnMouseLeftButtonDown(mainWindow.Dynamic().text3);

            Assert.AreEqual("HogeHoge", (string)mainWindow.Dynamic().text3.Text);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var mainWindow = _app.WaitForIdentifyFromTypeFullName("CodeerFriendlyExample.MainWindow");
            Assert.AreEqual(true, (bool)mainWindow.Dynamic().SecureString.HasPassword);
            var secStr = mainWindow.Dynamic().SecureString.Password;
            var marshalClass = _app.Type("System.Runtime.InteropServices.Marshal");
            Assert.AreEqual("Password", (string)marshalClass.PtrToStringUni(marshalClass.SecureStringToGlobalAllocUnicode(secStr)));
        }

        void OnMouseLeftButtonDown(dynamic uielement)
        {
            var args = _app.Type().System.Windows.Input.MouseButtonEventArgs(
                _app.Type("System.Windows.Input.Mouse").PrimaryDevice,
                0,
                _app.Type("System.Windows.Input.MouseButton").Left);

            args.RoutedEvent = _app.Type("System.Windows.Input.Mouse").MouseDownEvent;
            args.Source = uielement;

            uielement.RaiseEvent(args);
        }

        static Interface1 GetInterface1Container(dynamic container)
        {
            return UnityContainerExtensions.Resolve<Interface1>(container);
        }
    }
}
