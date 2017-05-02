using MyAdminExplorer.Model;
using Syncfusion.Data.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для ControlUser.xaml
    /// </summary>
    public partial class ControlUser : Window
    {
        public ObservableCollection<User> Users { get; set; }
        public ControlUser()
        {
            InitializeComponent();
            var users = new List<User>();
            try
            {
                var temp = File.ReadAllLines(@"users.txt");
                foreach (string str in File.ReadAllLines(@"users.txt"))
                {
                    if (str == "") continue;
                    var userinstr = str.Split('|');
                    users.Add(
                        new User
                        {
                            Login = userinstr[0],
                            Password = userinstr[1],
                            Role = byte.Parse(userinstr[2]),
                            Even = bool.Parse(userinstr[3]),
                            From = DateTime.Parse(userinstr[4]),
                            To = DateTime.Parse(userinstr[5]),
                            FromTime = int.Parse(userinstr[6]),
                            ToTime = int.Parse(userinstr[7]),
                            ForbiddenList = userinstr[8].Split('?').ToList()
                        });
                }
            }
            catch (Exception ex) { }
            Users = new ObservableCollection<User>(users);
            dataGrid.ItemsSource = Users;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            MD5 md5Hash = MD5.Create();
            bool incorrect = false;
            StringBuilder result = new StringBuilder();
            using (var sw = new StreamWriter(@"users.txt"))
            {
                foreach (var item in dataGrid.ItemsSource)
                {
                    result.Clear();
                    if (((User)item).Login == "" || ((User)item).Password == "")
                    {
                        incorrect = true;
                        continue;
                    }
                    result.Append(((User)item).Login + "|" + ((User)item).Password + "|" + ((User)item).Role + "|"
                                  + ((User)item).Even + "|" + ((User)item).From + "|" + ((User)item).To
                                  + "|" + ((User)item).FromTime + "|" + ((User)item).ToTime + "|");
                    result.Append(((User)item).ForbiddenList[0]);
                    for (int i = 1; i < ((User)item).ForbiddenList.Count; i++)
                    {
                        result.Append("?" + ((User)item).ForbiddenList[i]);
                    }
                    sw.WriteLine(result);
                }
                if (incorrect)
                    MessageBox.Show("Not all data is saved because some were incorrect");
                else
                    MessageBox.Show("All data was saved");
            }
        }

        private void EditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            MD5 md5Hash = MD5.Create();
            var x = dataGrid.ItemsSource.ToList<User>().ToList().Select(c => c.Login);
            if (e.Column.Header.ToString() == "Login")
            {
                if (x.Contains(((TextBox)e.EditingElement).Text))
                {
                    MessageBox.Show("Such login already exists");
                    ((TextBox)e.EditingElement).Text = "";
                    e.Cancel = true;
                    return;
                }
                return;
            }
            else if (e.Column.Header.ToString() == "Password")
                ((TextBox)e.EditingElement).Text = GetMd5.GetMd5Hash(md5Hash, ((TextBox)e.EditingElement).Text);
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            Users.Remove((User)dataGrid.SelectedItem);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            AddUser adduser = new AddUser();
            adduser.Owner = this;
            adduser.ShowDialog();

            if (adduser.User != null)
                Users.Add(adduser.User);
        }

        private void Edit_Folders(object sender, RoutedEventArgs e)
        {
            EditForbidden editForbidden = new EditForbidden(((User)dataGrid.SelectedItem).ForbiddenList);
            editForbidden.ShowDialog();
            ((User)dataGrid.SelectedItem).ForbiddenList = editForbidden.Forbidden.ToList();
        }
    }
}

