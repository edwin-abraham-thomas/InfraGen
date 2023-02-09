using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraGen.Models;

namespace InfraGen.Data
{
    public class UserContext
    {
        private readonly string _userFilePath;
        public UserContext()
        {
            _userFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            InitUserFromLocalSettingFile();
        }

        private void InitUserFromLocalSettingFile()
        {
            EnsureDirectoryAndSettingFileExist();

            string userString = "";

            using (StreamReader file = File.OpenText($"{_userFilePath}\\infragen\\user.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                userString = JToken.ReadFrom(reader).ToString();
            }

            var user = JsonConvert.DeserializeObject<User>(userString);
            User = new User
            {
                Name = user.Name,
                AdoPat = user.AdoPat
            };
        }

        private void EnsureDirectoryAndSettingFileExist()
        {

            var isFolderPresent = Directory.Exists($"{_userFilePath}\\infragen");
            if (!isFolderPresent)
            {
                Directory.CreateDirectory($"{_userFilePath}\\infragen");
            }

            var isUserFileExist = File.Exists($"{_userFilePath}\\infragen\\user.json");
            if (!isUserFileExist)
            {
                var user = new UserContext();
                var userJson = JsonConvert.SerializeObject(user);
                File.WriteAllText($"{_userFilePath}\\infragen\\user.json", userJson);
            }
        }

        public User User { get; set; }
    }
}
