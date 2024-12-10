namespace App_AEE.Pages;

public partial class EquipesPage : ContentPage
{
	private readonly ApiService _apiService;
	public EquipesPage(ApiService apiService)
	{
		InitializeComponent();
		_apiService = apiService;
        
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ListarEquipes();
    }

    private async void CriarEquipeClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new CriarEquipePage(_apiService));
    }

    private async Task ListarEquipes()
    {
        // Chama o serviço para listar os eventos existentes
        var equipes = await _apiService.ListarEquipesDoUsuario();

        if (equipes != null && equipes.Any())
        {
            // Mostra a lista de eventos
            equipesCollectionView.ItemsSource = equipes;
        }
        
    }
}