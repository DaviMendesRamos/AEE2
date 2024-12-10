using App_AEE.Services;

namespace App_AEE.Pages.AdminPages;

public partial class CriarEventoPage : ContentPage
{
	private readonly EventosService _eventoService;
	public CriarEventoPage(EventosService eventoService)
	{
		InitializeComponent();
		_eventoService = eventoService;

    }

    private async void btnCriarEvento_Clicked(object sender, EventArgs e)
    {
        // Verifica se todos os campos estão preenchidos
        if (string.IsNullOrEmpty(txtNomeEvento.Text) ||
            string.IsNullOrEmpty(txtLocalEvento.Text)

            )
        {
            await DisplayAlert("Erro", "Todos os campos devem ser preenchidos.", "OK");
            return;
        }

        // Cria o objeto de evento com os dados fornecidos
        var novoEvento = new App_AEE.Model.Evento
        {
            NomeEvento = txtNomeEvento.Text,
            DataInicio = datePickerDataInicio.Date,
            DataFim = datePickerDataFim.Date,
            LocalEvento = txtLocalEvento.Text,

        };

        // Chama o serviço para criar o evento
        var sucesso = await _eventoService.CriarEvento(novoEvento);

        if (sucesso)
        {
            await DisplayAlert("Sucesso", "Evento criado com sucesso!", "OK");
            await Navigation.PopAsync(); // Volta para a página anterior
        }
        else
        {
            await DisplayAlert("Erro", "Ocorreu um erro ao criar o evento.", "OK");
        }
    }

}