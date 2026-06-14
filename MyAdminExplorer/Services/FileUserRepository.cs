using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MyAdminExplorer.Infrastructure;
using MyAdminExplorer.Models;

namespace MyAdminExplorer.Services
{
    public class FileUserRepository
    {
        private readonly string _filePath;

        public FileUserRepository()
            : this(AppPaths.UsersFilePath)
        {
        }

        public FileUserRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IReadOnlyList<User> LoadAll()
        {
            if (!File.Exists(_filePath))
            {
                return Array.Empty<User>();
            }

            var users = new List<User>();
            foreach (var line in File.ReadAllLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var user = TryParseLine(line);
                if (user != null)
                {
                    users.Add(user);
                }
            }

            return users;
        }

        public User FindByLogin(string login)
        {
            return LoadAll().FirstOrDefault(u =>
                string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveAll(IEnumerable<User> users)
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = new StreamWriter(_filePath, false))
            {
                foreach (var user in users)
                {
                    writer.WriteLine(Serialize(user));
                }
            }
        }

        public void UpdatePassword(string login, string passwordHash, byte role, User sourceUser = null)
        {
            var users = LoadAll().ToList();
            var index = users.FindIndex(u =>
                string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
            {
                users[index].Password = passwordHash;
            }
            else if (sourceUser != null)
            {
                users.Add(new User
                {
                    Login = login,
                    Password = passwordHash,
                    Role = role,
                    Even = sourceUser.Even,
                    From = sourceUser.From,
                    To = sourceUser.To,
                    FromTime = sourceUser.FromTime,
                    ToTime = sourceUser.ToTime,
                    ForbiddenList = new List<string>(sourceUser.ForbiddenList ?? new List<string>())
                });
            }
            else
            {
                users.Add(new User { Login = login, Password = passwordHash, Role = role });
            }

            SaveAll(users);
        }

        public static User TryParseLine(string line)
        {
            var parts = line.Split('|');
            if (parts.Length < 9)
            {
                return null;
            }

            try
            {
                return new User
                {
                    Login = parts[0],
                    Password = parts[1],
                    Role = byte.Parse(parts[2]),
                    Even = bool.Parse(parts[3]),
                    From = DateTime.Parse(parts[4], CultureInfo.InvariantCulture),
                    To = DateTime.Parse(parts[5], CultureInfo.InvariantCulture),
                    FromTime = int.Parse(parts[6]),
                    ToTime = int.Parse(parts[7]),
                    ForbiddenList = string.IsNullOrEmpty(parts[8])
                        ? new List<string>()
                        : parts[8].Split('?').Where(p => !string.IsNullOrEmpty(p)).ToList()
                };
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static string Serialize(User user)
        {
            var forbidden = user.ForbiddenList == null || user.ForbiddenList.Count == 0
                ? string.Empty
                : string.Join("?", user.ForbiddenList);

            return string.Join("|",
                user.Login,
                user.Password,
                user.Role,
                user.Even,
                user.From.ToString(CultureInfo.InvariantCulture),
                user.To.ToString(CultureInfo.InvariantCulture),
                user.FromTime,
                user.ToTime,
                forbidden);
        }
    }
}
