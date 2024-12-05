using App_AEE.Services;
using App_AEE.Pages;

namespace App_AEE.Pages
{
    public partial class EventosPage : ContentPage
    {
        private readonly EventosService _eventoService;

        public EventosPage(EventosService eventoService)
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
            var novoEvento = new App_AEE.Models.Evento
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

        private async void btnListarEventos_Clicked(object sender, EventArgs e)
        {
            // Chama o serviço para listar os eventos existentes
            var eventos = await _eventoService.ListarEventos();

            if (eventos != null && eventos.Any())
            {
                // Mostra a lista de eventos
                eventosCollectionView.ItemsSource = eventos;
            }
            else
            {
                await DisplayAlert("Sem eventos", "Não há eventos cadastrados.", "OK");
            }
        }
    }
}
