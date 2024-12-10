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

    public AppShell(ApiService apiService, IValidator validator, EventosService eventosService)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _eventosService = eventosService;

        // Registra rotas para navegação
        Routing.RegisterRoute(nameof(HomeAdminPage), typeof(HomeAdminPage));
        Routing.RegisterRoute(nameof(HomePrincipalPage), typeof(HomePrincipalPage));
        Routing.RegisterRoute(nameof(EventosPage), typeof(EventosPage));
        Routing.RegisterRoute(nameof(EquipesPage), typeof(EquipesPage));

        // Configura a visibilidade das páginas
        ConfigureHomeVisibility();

        // Registra o evento Navigated para tratar navegação após troca de aba
        this.Navigated += OnShellNavigated; // Substituí Shell.Navigated por this.Navigated
    }

    private void ConfigureHomeVisibility()
    {
        // Obtém a role salva nas preferências (admin ou user)
        var role = Preferences.Get("role", string.Empty);

        // Controla a visibilidade das abas de acordo com a role
        HomeAdminTab.IsVisible = role == "admin";
        HomePrincipalTab.IsVisible = role != "admin";
    }

    /// <summary>
    /// Navega para a página HomeAdmin, garantindo a navegação correta.
    /// </summary>
    public async Task NavigateToHomeAdminAsync()
    {
        await this.GoToAsync("//HomeAdmin"); // Navegação absoluta
    }

    /// <summary>
    /// Navega para a página HomePrincipal, garantindo a navegação correta.
    /// </summary>
    public async Task NavigateToHomePrincipalAsync()
    {
        await this.GoToAsync("//HomePrincipal"); // Navegação absoluta
    }
    private async void OnNavigarParaEquipesPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("EquipesPage");
    }

    /// <summary>
    /// Evento disparado após a navegação ser concluída. Garante a reinicialização da página.
    /// </summary>
    private async void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
    {
        // Verifica se a aba "HomeAdmin" ou "HomePrincipal" foi selecionada
        if (e.Current.Location.OriginalString.Contains("HomeAdmin") && e.Source == ShellNavigationSource.ShellSectionChanged)
        {
            // Reinicia a página HomeAdmin
            await this.GoToAsync("///HomeAdmin");
        }
        else if (e.Current.Location.OriginalString.Contains("HomePrincipal") && e.Source == ShellNavigationSource.ShellSectionChanged)
        {
            // Reinicia a página HomePrincipal
            await this.GoToAsync("///HomePrincipal");
        }
    }
    

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Remove o evento para evitar múltiplas assinaturas
        this.Navigated -= OnShellNavigated;
    }
}
