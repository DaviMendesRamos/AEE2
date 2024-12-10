using App_AEE.Services;
using App_AEE.Model;




namespace App_AEE.Pages;


public partial class CriarEquipePage : ContentPage
{

    private readonly ApiService _apiService;
	public CriarEquipePage(ApiService apiService)
	{
		InitializeComponent();
        _apiService = apiService;
	}

    private async void btnCriarEquipe_Clicked(object sender, EventArgs e)
    {
        // Verifica se todos os campos estão preenchidos
        // Verifica se todos os campos estão preenchidos
        if (string.IsNullOrEmpty(txtNomeEquipe.Text) ||
            string.IsNullOrEmpty(txtModalidade.Text))
        {
            await DisplayAlert("Erro", "Todos os campos devem ser preenchidos.", "OK");
            return;
        }

        // Cria o objeto de equipe com os dados fornecidos
        var novaEquipe = new App_AEE.Model.Equipe
        {
            NomeEquipe = txtNomeEquipe.Text,
            Modalidade = txtModalidade.Text,
        };

        // Chama o serviço para criar a equipe
        var response = await _apiService.CriarEquipe(novaEquipe);

        // Verifica se houve sucesso na criação da equipe
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            // Se houver mensagem de erro, exibe o alerta
            await DisplayAlert("Erro", $"Erro ao criar a equipe: {response.ErrorMessage}", "OK");
        }
        else
        {
            // Se não houver erro, significa que a equipe foi criada com sucesso
            await DisplayAlert("Sucesso", "Equipe criada com sucesso!", "OK");
            await Navigation.PopAsync(); // Volta para a página anterior
        }
    }
}