using App_AEE.Services;

namespace App_AEE.Pages;

public partial class HomePrincipalPage : ContentPage
{
    private readonly EventosService _eventosService;

    public HomePrincipalPage(EventosService eventosService)
    {
        InitializeComponent();
        _eventosService = eventosService;
    }

    private async void OnInscreverEventoTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EventosPage));
    }
    private async void OnVerEventoTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VerEventosPage(_eventosService));
    }

}
