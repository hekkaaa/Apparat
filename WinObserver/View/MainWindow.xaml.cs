﻿using Apparat.ViewModel;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WinObserver.Model;
using WinObserver.ViewModel;

namespace WinObserver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IApplicationViewModel _applicationViewModel;
        ILogger<MainWindow> _logger;

        public MainWindow(IApplicationViewModel applicationViewModel, ILogger<MainWindow> logger)
        {
            InitializeComponent();
            _logger = logger;
            _logger.LogWarning("Init MainWindows object");
            _applicationViewModel = applicationViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _applicationViewModel;

            _logger.LogWarning("General Windows Start", "Done");
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}"));
            e.Handled = true;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _logger.LogWarning("Application is closed! Goodbye!");
        }

        private void KeyEvents(object sender, KeyEventArgs e)
        { // Drop Collection History Combobox.
            var s = sender as ComboBox;
            s.IsDropDownOpen = false;
        }


        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _logger.LogWarning($"User select item is TreeView: {e.NewValue}");
            try
            {
                HostViewModel obj = (HostViewModel)e.NewValue;
                if (obj != null)
                {
                    _logger.LogWarning($"User select hostname: {obj.HostnameView} | ID: {obj.PublicId}");
                    ApplicationViewModel s1 = DataContext as ApplicationViewModel;
                    s1.SelectedGroup = obj.TracertObject;
                }
            }
            catch (System.InvalidCastException ex)
            {
                _logger.LogError($"Error castObject: HostViewModel {ex.Message}");
            }

        }
    }
}
