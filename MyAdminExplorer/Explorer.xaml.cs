using MyAdminExplorer.Infrastructure;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MyAdminExplorer
{
    public partial class Explorer : Window
    {
        public List<string> Forbidden { get; }

        public Explorer(List<string> forbidden)
        {
            InitializeComponent();
            Forbidden = forbidden ?? new List<string>();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Owner.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FolderTreeHelper.PopulateDrives(FoldersItem, Folder_Expanded);
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (Forbidden != null && Forbidden.Contains(item.Tag.ToString()))
            {
                MessageBox.Show("Folder is forbidden");
                return;
            }

            FolderTreeHelper.ExpandFolder(item, Folder_Expanded);
        }

        private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = FoldersItem.SelectedItem as TreeViewItem;
            if (selectedItem == null)
            {
                return;
            }

            var path = FolderTreeHelper.BuildSelectedPath(selectedItem);
            if (Forbidden != null && Forbidden.Contains(path))
            {
                MessageBox.Show("Folder is forbidden");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
