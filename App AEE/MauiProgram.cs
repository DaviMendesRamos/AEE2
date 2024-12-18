using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using App_AEE.Services;
using App_AEE.Pages;
using App_AEE.Validations;
using App_AEE.Pages.AdminPages;

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
			builder.Services.AddSingleton<EventosService>();
            builder.Services.AddTransient<LoginUsuarioPage>();
            builder.Services.AddTransient<EventosPage>();
            builder.Services.AddTransient<HomePrincipalPage>();
            builder.Services.AddTransient<ExibirUsuarioPage>();
            builder.Services.AddTransient<HomeAdminPage>();
            builder.Services.AddTransient<CriarEquipePage>();
            builder.Services.AddTransient<EquipesPage>();// Ou Singleton se preferir uma única instância
            builder.Services.AddTransient<CriarEventoPage>();
            builder.Services.AddTransient<EditarEventosPage>();
            builder.Services.AddTransient<VerEventosPage>();
            builder.Services.AddTransient<ConsultarEventoPage>();
            return builder.Build();
        }
    }
}