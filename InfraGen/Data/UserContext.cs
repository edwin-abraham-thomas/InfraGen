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
            var user = await GetObjectFromJsonFileAsync<User>(_settingFilePath);
            user.Info = _mapper.Map<Info>(User.Info);
            var updateSuccess = await WriteObjectToJsonFileAsync(user, _settingFilePath);
        }

        public async Task UpdateUserCredentials()
        {
            var user = await GetObjectFromJsonFileAsync<User>(_settingFilePath);
            user.Credentials = _mapper.Map<Credentials>(User.Credentials);
            var updateSuccess = await WriteObjectToJsonFileAsync(user, _settingFilePath);
        }

        private async Task<T> GetObjectFromJsonFileAsync<T>(string path)
        {
            string TString = "";
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                var jToken = await JToken.ReadFromAsync(reader);
                TString = jToken.ToString();
            }

            T dataObject = JsonConvert.DeserializeObject<T>(TString);

            return dataObject;
        }

        private async Task<bool> WriteObjectToJsonFileAsync<T>(T dataObjectUpdate, string path)
        {
            try
            {
                string TObjectUpdateString = JsonConvert.SerializeObject(dataObjectUpdate);
                await File.WriteAllTextAsync(_settingFilePath, TObjectUpdateString);
            }
            catch(Exception ex)
            {
                return false;
            }

            return false;
        }
    }
}
