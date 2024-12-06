using App_AEE.Validations;
using App_AEE.Services;
using App_AEE.Pages;
using App_AEE.Pages.AdminPages;

namespace App_AEE;

public partial class AppShell : Shell
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private readonly EventosService _eventosService;

    // Use injeção de dependência para fornecer as instâncias dos serviços
    public AppShell(ApiService apiService, IValidator validator, EventosService eventosService)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _eventosService = eventosService;

        // Registra rotas para navegação
        Routing.RegisterRoute("HomeAdmin", typeof(HomeAdminPage));
        Routing.RegisterRoute("HomePrincipal", typeof(HomePrincipalPage));

        // Configura a visibilidade das páginas
        ConfigureHomeVisibility();
    }

    private void ConfigureHomeVisibility()
    {
        var role = Preferences.Get("role", string.Empty);

        // Controla a visibilidade das abas com base na role
        if (role == "admin")
        {
            HomeAdminTab.IsVisible = true;
            HomePrincipalTab.IsVisible = false;
        }
        else
        {
            HomeAdminTab.IsVisible = false;
            HomePrincipalTab.IsVisible = true;
        }
    }

    // Alteração para garantir que as páginas sejam instanciadas com DI
    public void NavigateToHomeAdmin()
    {
        // Navega para HomeAdminPage, garantindo a injeção de dependência
        Shell.Current.GoToAsync("HomeAdmin");
    }

    public void NavigateToHomePrincipal()
    {
        // Navega para HomePrincipalPage
        Shell.Current.GoToAsync("HomePrincipal");
    }
}
