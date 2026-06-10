using System;
using System.IO;
using System.Reflection;

namespace MyAdminExplorer.Infrastructure
{
    public static class AppPaths
    {
        public static string ApplicationDirectory =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppDomain.CurrentDomain.BaseDirectory;

        public static string UsersFilePath => Path.Combine(ApplicationDirectory, "users.txt");

        public static string SampleUsersFilePath => Path.Combine(ApplicationDirectory, "samples", "users.txt.example");

        public static void EnsureUsersFile()
        {
            if (File.Exists(UsersFilePath))
            {
                return;
            }

            var samplePath = SampleUsersFilePath;
            if (File.Exists(samplePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(UsersFilePath));
                File.Copy(samplePath, UsersFilePath);
                return;
            }

            throw new FileNotFoundException(
                "users.txt not found and sample file is missing. Expected sample at: " + samplePath);
        }
    }
}
