using MyAdminExplorer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;

namespace MyAdminExplorer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            var users = new List<User>();
            if (login.Text == "admin" && password.Password == "admin")
            {
                AdminMain adminMain = new AdminMain(
                    new User
                    {
                        Login = login.Text,
                        Password = password.Password,
                        Role = 1
                    });
                adminMain.Show();
                Close();
                return;
            }
            try
            {
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
            bool found = false;
            foreach (var item in users)
            {
                if (item.Login == login.Text)
                {
                    MD5 md5Hash = MD5.Create();
                    if (item.Password == GetMd5.GetMd5Hash(md5Hash, password.Password))
                    {
                        found = true;
                       if (DateTime.Now.Day % 2 != 0 && item.Even)
                        {
                            MessageBox.Show("Today is an odd day. \nYour day is tomorrow");
                            return;
                        }
                        else if (DateTime.Now.Day % 2 == 0 && !item.Even)
                        {
                            MessageBox.Show("Today is an even day. \nYour day is tomorrow");
                            return;
                        }
                        if (item.To.Day * 86400 + item.ToTime * 3600 < DateTime.Now.Day * 86400 + DateTime.Now.Hour * 3600 + DateTime.Now.Second)
                        {
                            MessageBox.Show(string.Format("Access is denied.\n Your terms from {0}/{1}/{2}  {3}:00 to {4}/{5}/{6} {7}:00",
                                item.From.Day, item.From.Month, item.From.Year, item.FromTime, item.To.Day, item.To.Month, item.To.Year, item.ToTime));
                            return;
                        }
                        if (item.Role == 0)
                        {
                            Menu menu = new Menu(item, false);
                            menu.Show();
                        }
                        if (item.Role == 1)
                        {
                            AdminMain adminMain = new AdminMain(item);
                            adminMain.Show();
                        }
                        Close();
                    }
                }
            }

            if (!found)
                MessageBox.Show("Incorrect Login or Password");
        }
    }
}
