using MyAdminExplorer.Infrastructure;
using MyAdminExplorer.Models;
using MyAdminExplorer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyAdminExplorer
{
    public partial class EditForbidden : Window
    {
        private readonly List<string> _forbidden;

        public ObservableCollection<string> Forbidden { get; }

        public EditForbidden(List<string> forbidden)
        {
            InitializeComponent();
            _forbidden = forbidden ?? new List<string>();
            Forbidden = new ObservableCollection<string>(_forbidden);
            ForbiddenList.ItemsSource = Forbidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FolderTreeHelper.PopulateDrives(FoldersItem, Folder_Expanded);
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            FolderTreeHelper.ExpandFolder((TreeViewItem)sender, Folder_Expanded);
        }

        private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = FoldersItem.SelectedItem as TreeViewItem;
            if (selectedItem == null)
            {
                return;
            }

            var path = FolderTreeHelper.BuildSelectedPath(selectedItem);
            if (string.IsNullOrEmpty(path) || Forbidden.Any(f => string.Equals(f, path, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            Forbidden.Add(path);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var selected = ForbiddenList.SelectedItem as string;
            if (selected != null)
            {
                Forbidden.Remove(selected);
            }
        }
    }
}
