using App_AEE.Services;
using App_AEE.Pages;
using App_AEE.Validations;
namespace App_AEE.Pages.AdminPages;


public partial class HomeAdminPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    private readonly EventosService _eventosService;
    
    public HomeAdminPage(EventosService eventosService, ApiService apiService,IValidator validator)
    {
        InitializeComponent();
        _eventosService = eventosService;
        _apiService = apiService;
        _validator = validator;

    }

    private async void OnCriarEventoTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CriarEventoPage(_eventosService));
    }
    private async void OnAddAdminsTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdicionarAdminPage(_apiService));
    }

    private async void OnEditarEventoTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditarEventosPage(_eventosService));
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }


}
