using Microsoft.Extensions.Logging;
using InfraGen.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InfraGen.Mappers;

namespace InfraGen;

public static class MauiProgram
{


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

		builder.Services.AddAutoMapper(opt => opt.AddProfile(new MapperProfile()));

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<UserContext>();
        builder.Services.AddSingleton<ResourceConfigService>();

        return builder.Build();
	}

	
}
