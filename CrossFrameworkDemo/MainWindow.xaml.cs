using CrossFrameworkLibrary;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CrossFrameworkDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.btnReference.Click += AccessByReference;
            this.btnReflection.Click += AccessByReflection;
            this.btnDllImport.Click += AccessByDllImport;
        }

        public void AccessByReference(object sender, RoutedEventArgs e)
        {
            try
            {
                AppendText(AccessTest.GetPublicMessage());

                //AccessTest.GetInternalMessage(); //无法调用

                try
                {
                    AccessTest.RemotingTest(); // 涉及.NET Framework独有API(AppDomain.CurrentDomain.SetupInformation)，调用失败
                }
                catch (Exception ex)
                {
                    AppendText($"Remoting Called Failed By: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                AppendText($"Error occurred during testing: {ex.Message}");
            }
            finally
            {
                if (this.btnDllImport.IsEnabled)
                {
                    this.btnDllImport.IsEnabled = false;
                }
            }
        }

        public void AccessByReflection(object sender, RoutedEventArgs e)
        {
            InvokeMethodByReflection("GetPublicMessage"); // 正常调用

            InvokeMethodByReflection("GetInternalMessage"); // 正常调用

            InvokeMethodByReflection("RemotingTest"); // 涉及.NET Framework独有API，调用失败

            if (this.btnDllImport.IsEnabled)
            {
                this.btnDllImport.IsEnabled = false;
            }
        }

        public void AccessByDllImport(object sender, RoutedEventArgs e)
        {
            AppendText(NativeMethods.GetPublicMessage()); // 正常调用

            AppendText(NativeMethods.GetInternalMessage()); // 正常调用

            try
            {
                AppendText(NativeMethods.RemotingTest()); // 正常调用
            }
            catch (Exception ex)
            {
                AppendText("Remoting Called Failed: " + ex);
            }
            finally
            {
                if (this.btnReference.IsEnabled)
                {
                    this.btnReference.IsEnabled = false;
                }

                if (this.btnReflection.IsEnabled)
                {
                    this.btnReflection.IsEnabled = false;
                }
            }
        }

        private void InvokeMethodByReflection(string methodName)
        {
            try
            {
                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CrossFrameworkLibrary.dll");

                if (!File.Exists(dllPath))
                {
                    AppendText($"DLL file not found: {dllPath}");
                    return;
                }

                Assembly appAssembly = Assembly.LoadFrom(dllPath);

                Type type = appAssembly.GetType("CrossFrameworkLibrary.AccessTest");

                if (type == null)
                {
                    AppendText($"Type AccessTest not found");
                    return;
                }

                // include public, non-public, static methods
                MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.Static);

                if (methodInfo == null)
                {
                    AppendText($"Method {methodName} not found");
                    return;
                }

                var res = methodInfo.Invoke(null, new object[] { });

                if (res != null)
                {
                    AppendText($"Reflection Call {methodName} Success: {res}");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    AppendText($"Reflection Call Error: {ex.InnerException}");
                }
                else
                {
                    AppendText($"Reflection Call Error: {ex}");
                }
            }
        }

        private void AppendText(string message)
        {
            this.txtResult.AppendText($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} : {message}\n");
            this.txtResult.AppendText("\n");
            this.scrollViewer.ScrollToEnd();
        }
    }
}