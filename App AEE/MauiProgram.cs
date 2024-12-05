using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using App_AEE.Services;
using App_AEE.Pages;
using App_AEE.Validations;

namespace App_AEE
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseMauiCommunityToolkitCore().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("arial.ttf", "arial");
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif 
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<ApiService>();
			builder.Services.AddSingleton<IValidator,Validator>();
			builder.Services.AddTransient<LoginUsuarioPage>();
            builder.Services.AddSingleton<EventosService>();
            builder.Services.AddTransient<EventosPage>();
            builder.Services.AddTransient<HomePrincipalPage>();

            return builder.Build();
        }
    }
}