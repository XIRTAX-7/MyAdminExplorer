using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MyAdminExplorer.Infrastructure
{
    public static class FolderTreeHelper
    {
        private static readonly object DummyNode = null;

        public static void PopulateDrives(TreeView treeView, RoutedEventHandler onExpanded)
        {
            treeView.Items.Clear();
            foreach (var drive in Directory.GetLogicalDrives())
            {
                var item = CreateDriveItem(drive, onExpanded);
                treeView.Items.Add(item);
            }
        }

        public static void ExpandFolder(TreeViewItem item, RoutedEventHandler onExpanded)
        {
            if (item.Items.Count != 1 || item.Items[0] != DummyNode)
            {
                return;
            }

            item.Items.Clear();
            try
            {
                foreach (var directory in Directory.GetDirectories(item.Tag.ToString()))
                {
                    var subItem = new TreeViewItem
                    {
                        Header = directory.Substring(directory.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                        Tag = directory,
                        FontWeight = FontWeights.Normal
                    };
                    subItem.Items.Add(DummyNode);
                    subItem.Expanded += onExpanded;
                    item.Items.Add(subItem);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }
        }

        public static string BuildSelectedPath(TreeViewItem selectedItem)
        {
            if (selectedItem == null)
            {
                return string.Empty;
            }

            var path = string.Empty;
            var separator = string.Empty;
            var current = selectedItem;

            while (current != null)
            {
                var header = current.Header.ToString();
                if (header.Contains("\\"))
                {
                    separator = string.Empty;
                }

                path = header + separator + path;
                if (current.Parent is TreeView)
                {
                    break;
                }

                current = current.Parent as TreeViewItem;
                separator = @"\";
            }

            return path;
        }

        private static TreeViewItem CreateDriveItem(string drive, RoutedEventHandler onExpanded)
        {
            var item = new TreeViewItem
            {
                Header = drive,
                Tag = drive,
                FontWeight = FontWeights.Normal
            };
            item.Items.Add(DummyNode);
            item.Expanded += onExpanded;
            return item;
        }
    }
}
