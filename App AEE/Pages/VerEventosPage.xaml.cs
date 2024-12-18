using App_AEE.Services;

namespace App_AEE.Pages;

public partial class VerEventosPage : ContentPage
{
    private readonly EventosService _eventoService;
	public VerEventosPage(EventosService eventoService)
	{
        _eventoService = eventoService;
        InitializeComponent();


	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ListarEventos();
    }

    private async Task ListarEventos()
    {
        // Chama o serviço para listar os eventos existentes
        var eventos = await _eventoService.ListarEventos();

        if (eventos != null && eventos.Any())
        {
            // Mostra a lista de eventos
            eventosCollectionView.ItemsSource = eventos;
        }

    }
}