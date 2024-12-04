namespace App_AEE.Pages;
using App_AEE.Validations;
public partial class ExibirUsuarioPage : ContentPage
{
    public ExibirUsuarioPage()
    {
        InitializeComponent();
    }
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private void btndeslogar_Clicked(object sender, EventArgs e)
    {
        var tokenStorage = new TokenStorage();
        tokenStorage.RemoverToken(); // Remove o token armazenado

        // Navega para a tela de login (você pode ajustar o nome da tela conforme o nome da sua página)
        Application.Current.MainPage = new NavigationPage(new LoginUsuarioPage(_apiService, _validator));

    }
}
