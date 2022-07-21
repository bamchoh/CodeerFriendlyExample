using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using Unity;

namespace CodeerFriendlyExampleTest
{
    [TestClass]
    public class UnitTest1
    {
        WindowsAppFriend _app;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Console.WriteLine("ClassInitialize:" + context.TestName);
        }

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

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
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
        [DataRow("HogeHoge")]
        [DataRow("あいうえお")]
        public void TestMethod3(string text)
        {
            var mainWindow = _app.WaitForIdentifyFromTypeFullName("CodeerFriendlyExample.MainWindow");
            Assert.AreEqual("TEST3", (string)mainWindow.Dynamic().text3.Text);

            OnMouseLeftButtonDown(mainWindow.Dynamic().text3);

            Assert.AreEqual("Test3", (string)mainWindow.Dynamic().text3.Text);

            var originalClass = _app.Type().CodeerFriendlyExample.OriginalClass(text);

            mainWindow.Dynamic().OriginalClass = originalClass;

            OnMouseLeftButtonDown(mainWindow.Dynamic().text3);

            Assert.AreEqual(text, (string)mainWindow.Dynamic().text3.Text);
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

        [TestMethod]
        public void TestMethod5()
        {
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);

            var t = _app.Type(GetType()).GetTypeFromProcessAsm("CodeerFriendlyExample.Interface1");

            var app = _app.Type().System.Windows.Application.Current;

            var impl = _app.Type().Unity.UnityContainerExtensions.Resolve(app.Container, t, null);

            impl.Add(10, 20);

            Assert.AreEqual(30, (int)impl.Value);
        }

        [TestMethod]
        public void TestMethod6()
        {
            _app.LoadAssembly(GetType().Assembly);

            var t = _app.Type(GetType()).GetTypeFromProcessAsm("CodeerFriendlyExampleInterfaces.ExampleInterface");

            var app = _app.Type().System.Windows.Application.Current;

            var impl = _app.Type().Unity.UnityContainerExtensions.Resolve(app.Container, t, null);

            var result = impl.Do1("test");

            Assert.AreEqual("** test **", (string)result);

            var value = impl.Do2(2);

            Assert.AreEqual(4, (int)value);

            var text = impl.Do3("default", 12345);

            Assert.AreEqual("default/12345=0.3",(string)text);

            var obj = _app.Type(GetType()).InvokeGenericMethod(impl, "Do4", "CodeerFriendlyExampleInterfaces.ExampleInterface2");

            Assert.AreEqual("1235", (string)obj.PlusOneString(1234));

            var obj2 = _app.Type(GetType()).InvokeGenericMethod2(impl, "Do5", obj.GetType());

            Assert.AreEqual("1235", (string)obj2.PlusOneString(1234));
        }

        private static object InvokeGenericMethod(dynamic obj, string method, string typeName)
        {
            var t = GetTypeFromProcessAsm(typeName);
            var mi = obj.GetType().GetMethod(method);
            var mRef = mi.MakeGenericMethod(t);
            return mRef.Invoke(obj, null);
        }

        private static object InvokeGenericMethod2(dynamic obj, string method, dynamic t)
        {
            var mi = obj.GetType().GetMethod(method);
            var mRef = mi.MakeGenericMethod(t);
            return mRef.Invoke(obj, new object[] { "test", null, 1 });
        }

        [TestMethod]
        public void TestMethod7()
        {
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);

            var t = _app.Type(GetType()).GetTypeFromProcessAsm("CodeerFriendlyExampleInterfaces.ExampleInterface");

            var app = _app.Type().System.Windows.Application.Current;

            var impl = _app.Type().Unity.UnityContainerExtensions.Resolve(app.Container, t, null);

            t = _app.Type(GetType()).GetTypeFromProcessAsm("CodeerFriendlyExampleInterfaces.ExampleInterface2");

            impl = _app.Type().Unity.UnityContainerExtensions.Resolve(impl.Container, t, null);

            Assert.AreEqual(2, (int)impl.PlusOne(1));
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

        Type GetType(string typeString)
        {
            var domain = _app.Type().System.AppDomain.CurrentDomain;

            Type t = null;
            foreach (var asm in domain.GetAssemblies())
            {
                var assemblyString = asm.FullName;

                // 参照しているアセンブリのどこかにある型を探してタイプを取得する。
                // アセンブリが間違っていると null となる
                t = Type.GetType(string.Format("{0}, {1}", typeString, assemblyString));

                if (t != null)
                    break;
            }
            return t;
        }

        /***
         * Interface1 は CodeerFriendlyExample に定義が存在する
         * CodeerFriendlyExample を参照に加えれば使用できるが
         * 通常、実行するプロセスを参照に加えたくはないので
         * コメントアウト
         ***
        static Interface1 GetInterface1Container(dynamic container)
        {
            return UnityContainerExtensions.Resolve<Interface1>(container);
        }
        */

        static object GetInterface1Container(dynamic container)
        {
            var t = GetTypeFromProcessAsm("CodeerFriendlyExample.Interface1");
            return UnityContainerExtensions.Resolve(container, t, null);
        }

        static Type GetTypeFromProcessAsm(string typeString)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var asmString = asm.FullName;
                var t = Type.GetType(String.Format("{0}, {1}", typeString, asmString));
                if (t != null)
                    return t;
            }
            throw new Exception(string.Format("Type was not found: {0}", typeString));
        }
    }
}
