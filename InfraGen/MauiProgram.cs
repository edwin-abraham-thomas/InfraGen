using Microsoft.Extensions.Logging;
using InfraGen.Data;
using InfraGen.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfraGen;

public static class MauiProgram
{

	private static string _userFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

        EnsureDirectoryAndSettingFileExist();
        InitUserFromLocalSettingFile();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<WeatherForecastService>();

		return builder.Build();
	}

	private static User InitUserFromLocalSettingFile()
	{
        using (StreamReader file = File.OpenText($"{_userFilePath}\\infragen\\user.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
		{
            var userString = JToken.ReadFrom(reader).ToString();
			var user = JsonConvert.DeserializeObject<User>(userString);
        }

        return null;
    }

	private static void EnsureDirectoryAndSettingFileExist()
	{

		var isFolderPresent = Directory.Exists($"{_userFilePath}\\infragen");
		if(!isFolderPresent)
		{
			Directory.CreateDirectory($"{_userFilePath}\\infragen");
		}

		var isUserFileExist = File.Exists($"{_userFilePath}\\infragen\\user.json");
		if(!isUserFileExist)
		{
            var user = new User();
            var userJson = JsonConvert.SerializeObject(user);
            File.WriteAllText($"{_userFilePath}\\infragen\\user.json", userJson);
        }
    }
}
