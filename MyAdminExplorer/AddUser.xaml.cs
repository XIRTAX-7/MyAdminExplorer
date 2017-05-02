using MyAdminExplorer.Model;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private List<string> _forbidden;
        public User User { get; set; }
        public AddUser()
        {
            InitializeComponent();
            _forbidden = new List<string>();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
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
                            Role = byte.Parse(userinstr[2])
                        });
                }
            }
            catch (Exception ex) { }
            var x = users.Select(c => c.Login);
            if (x.Contains(login.Text))
            {
                MessageBox.Show("Such login already exists");
                return;
            }
            int fromTime = 0;
            int toTime = 24;
            if (!int.TryParse(fromtime.Text, out fromTime) || !int.TryParse(totime.Text, out toTime) || fromTime < 0 || toTime > 24)
            {

                MessageBox.Show("The entered time is incorrect");
                return;

            }

            int.TryParse(totime.Text, out toTime);
            if (String.IsNullOrEmpty(login.Text) || String.IsNullOrEmpty(password.Password) || from.SelectedDate == null || to.SelectedDate == null)
                MessageBox.Show("The entered data is incorrect");
            else
            {
                MD5 md5hash = MD5.Create();
                User = new User
                {
                    Login = login.Text,
                    Password = GetMd5.GetMd5Hash(md5hash, password.Password),
                    Role = 0,
                    Even = even.IsChecked.Value,
                    From = from.SelectedDate.Value.AddHours(-12),
                    To = to.SelectedDate.Value.AddHours(-12),
                    FromTime = fromTime,
                    ToTime = toTime,
                    ForbiddenList = _forbidden
                };
                Close();
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditBlockedFolders(object sender, RoutedEventArgs e)
        {
            EditForbidden editForbidden = new EditForbidden(_forbidden);
            editForbidden.ShowDialog();
            _forbidden = editForbidden.Forbidden.ToList();
        }
    }
}
