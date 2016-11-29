using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace MotorXPGUIMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
