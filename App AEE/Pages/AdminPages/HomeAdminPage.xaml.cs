using App_AEE.Services;
using App_AEE.Pages;
namespace App_AEE.Pages.AdminPages;

public partial class HomeAdminPage : ContentPage
{
    private readonly EventosService _eventosService;
    
    public HomeAdminPage(EventosService eventosService)
    {
        InitializeComponent();
        _eventosService = eventosService;
    }

    private async void OnInscreverEventoTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventosPage(_eventosService));
    }
}
