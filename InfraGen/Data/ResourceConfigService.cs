using AutoMapper;
using InfraGen.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraGen.Data
{
    public class ResourceConfigService
    {
        private readonly string _resourceConfigFileName;
        private readonly string _resourceConfigFolderName;

        private readonly string _rootUserPath;
        private readonly string _resourceConfigFolderPath;
        private readonly string _resourceConfigFilePath;
        private readonly IMapper _mapper;

        public ResourceConfigService(IMapper mapper)
        {
            _mapper=mapper;
            _rootUserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            _resourceConfigFileName = "ResourceConfig.json";
            _resourceConfigFolderName = "InfraGen";
            _resourceConfigFolderPath = $"{_rootUserPath}\\{_resourceConfigFolderName}";
            _resourceConfigFilePath = $"{_rootUserPath}\\{_resourceConfigFolderName}\\{_resourceConfigFileName}";
            InitUserFromLocalSettingFile();
        }

        private void InitUserFromLocalSettingFile()
        {
            EnsureDirectoryAndSettingFileExist();

            string resourceConfigString = "";

            using (StreamReader file = File.OpenText(_resourceConfigFilePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                resourceConfigString = JToken.ReadFrom(reader).ToString();
            }

            var resourceConfig = JsonConvert.DeserializeObject<ResourceConfig>(resourceConfigString);
            ResourceConfig = _mapper.Map<ResourceConfig>(resourceConfig);
        }
        private void EnsureDirectoryAndSettingFileExist()
        {

            var isFolderPresent = Directory.Exists(_resourceConfigFolderPath);
            if (!isFolderPresent)
            {
                Directory.CreateDirectory(_resourceConfigFolderPath);
            }

            var isUserFileExist = File.Exists(_resourceConfigFilePath);
            if (!isUserFileExist)
            {
                var resourceConfig = new ResourceConfig();
                var resourceConfigJson = JsonConvert.SerializeObject(resourceConfig);
                File.WriteAllText(_resourceConfigFilePath, resourceConfigJson);
            }
        }

        public ResourceConfig ResourceConfig { get; set; }

        public async Task UpdateResourceConfigAsync()
        {
            var resourceConfig = await GetObjectFromJsonFileAsync<ResourceConfig>(_resourceConfigFilePath);
            resourceConfig = _mapper.Map<ResourceConfig>(ResourceConfig);
            var updateSuccess = await WriteObjectToJsonFileAsync(resourceConfig, _resourceConfigFilePath);
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
                await File.WriteAllTextAsync(_resourceConfigFilePath, TObjectUpdateString);
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
    }
}
