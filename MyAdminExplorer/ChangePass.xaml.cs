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
    /// Логика взаимодействия для ChangePassword.xaml
    /// </summary>
    public partial class ChangePass : Window
    {
        public List<User> Users;
        public User User;
        public ChangePass(User user)
        {
            InitializeComponent();
            User = user;
            List<User> users = new List<User>();
            try
            {
                foreach (string str in File.ReadAllLines(@"users.txt"))
                {
                    if (str == "") continue;
                    var userinstr = str.Split(':');
                    users.Add(new User { Login = userinstr[0], Password = userinstr[1], Role = byte.Parse(userinstr[2]) });
                }
            }
            catch (Exception ex) { }
            Users = users;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Owner.Show();
        }

        private void Change(object sender, RoutedEventArgs e)
        {
            if (password.Password == confirmpassword.Password)
            {
                MD5 md5Hash = MD5.Create();
                Users.Remove(Users.Find((c) => c.Login == User.Login));
                Users.Add(new User { Login = User.Login, Password = GetMd5.GetMd5Hash(md5Hash, password.Password), Role = User.Role });
                using (var sw = new StreamWriter(@"users.txt"))
                {
                    foreach (var item in Users)
                    {
                        sw.WriteLine(item.Login + ":" + item.Password + ":" + item.Role);
                    }
                }
                Close();
            }
            else
                MessageBox.Show("Passwords do not match");
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
