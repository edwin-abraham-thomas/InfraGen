using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraGen.Models;
using AutoMapper;

namespace InfraGen.Data
{
    public class UserContext
    {
        private readonly string _settingFileName;
        private readonly string _settingFolderName;

        private readonly string _rootUserPath;
        private readonly string _settingFolderPath;
        private readonly string _settingFilePath;
        private readonly IMapper _mapper;

        public UserContext(IMapper mapper)
        {
            _mapper=mapper;
            _rootUserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            _settingFileName = "User.json";
            _settingFolderName = "InfraGen";
            _settingFolderPath = $"{_rootUserPath}\\{_settingFolderName}";
            _settingFilePath = $"{_rootUserPath}\\{_settingFolderName}\\{_settingFileName}";
            InitUserFromLocalSettingFile();
        }
        private void InitUserFromLocalSettingFile()
        {
            EnsureDirectoryAndSettingFileExist();

            string userString = "";

            using (StreamReader file = File.OpenText(_settingFilePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                userString = JToken.ReadFrom(reader).ToString();
            }

            var user = JsonConvert.DeserializeObject<User>(userString);
            User = _mapper.Map<User>(user);
        }
        private void EnsureDirectoryAndSettingFileExist()
        {

            var isFolderPresent = Directory.Exists(_settingFolderPath);
            if (!isFolderPresent)
            {
                Directory.CreateDirectory(_settingFolderPath);
            }

            var isUserFileExist = File.Exists(_settingFilePath);
            if (!isUserFileExist)
            {
                var user = new User();
                var userJson = JsonConvert.SerializeObject(user);
                File.WriteAllText(_settingFilePath, userJson);
            }
        }
        public User User { get; set; }

        public async Task UpdateUserInfo()
        {
            string userString = "";
            using (StreamReader file = File.OpenText(_settingFilePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                userString = JToken.ReadFrom(reader).ToString();
            }

            var user = JsonConvert.DeserializeObject<User>(userString);

            user.Info = _mapper.Map<Info>(User.Info);

            await Task.Delay(5000);
        }
    }
}
