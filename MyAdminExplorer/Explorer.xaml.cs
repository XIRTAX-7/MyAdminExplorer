using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace MyAdminExplorer
{
    /// <summary>
    /// Логика взаимодействия для Explorer.xaml
    /// </summary>
    public partial class Explorer : Window
    {
        private object dummyNode = null;
        

        public List<string> Forbidden;

        public Explorer(List<string> forbidden)
        {
            InitializeComponent();
            Forbidden = forbidden;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Owner.Show();
        }
        public string SelectedImagePath { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(Folder_Expanded);
                FoldersItem.Items.Add(item);
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (Forbidden != null && Forbidden.Contains(item.Tag))
            {
                MessageBox.Show("Folder is forbidden");
                return;
            }

            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(Folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }

        private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = (TreeView) sender;
            TreeViewItem temp = ((TreeViewItem) tree.SelectedItem);

            if (temp == null)
                return;
            SelectedImagePath = "";
            string temp2 = "";
            while (true)
            {
                var temp1 = temp.Header.ToString();
                if (temp1.Contains(@"\"))
                {
                    temp2 = "";
                }
                SelectedImagePath = temp1 + temp2 + SelectedImagePath;
                if (temp.Parent.GetType() == typeof(TreeView))
                {
                    break;
                }
                temp = ((TreeViewItem) temp.Parent);
                temp2 = @"\";
            }

            if (Forbidden != null && Forbidden.Contains(SelectedImagePath))
                MessageBox.Show("Folder is forbidden");
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

