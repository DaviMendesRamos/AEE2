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

    private async void OnInscreverEventoTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventosPage(_eventosService));
    }
}
