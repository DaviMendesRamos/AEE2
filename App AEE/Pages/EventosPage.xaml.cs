using App_AEE.Services;
using CommunityToolkit.Maui.Views;
using App_AEE.Model;
using App_AEE.Pages;

namespace App_AEE.Pages
{
    public partial class EventosPage : ContentPage
    {
        private readonly EventosService _eventoService;
        private readonly ApiService _apiService;
        public Command<Evento> InscreverCommand { get; }

        public EventosPage(EventosService eventoService, ApiService apiService)
        {
            InitializeComponent();
            _eventoService = eventoService;
            _apiService = apiService;
            InscreverCommand = new Command<Evento>(InscreverEquipe);

            // Define o BindingContext como a pr�pria p�gina
            BindingContext = this;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ListarEventos();
        }



        private async Task ListarEventos()
        {
            // Chama o servi�o para listar os eventos existentes
            var eventos = await _eventoService.ListarEventos();

            if (eventos != null && eventos.Any())
            {
                // Mostra a lista de eventos
                eventosCollectionView.ItemsSource = eventos;
            }

        }

        // M�todo chamado quando o usu�rio clicar no bot�o "Inscrever"
        public async void InscreverEquipe(Evento evento)
        {
            // Lista de equipes do usu�rio
            var equipes = await _apiService.ListarEquipesDoUsuario();

            if (equipes == null || equipes.Count == 0)
            {
                await DisplayAlert("Erro", "Voc� n�o est� em nenhuma equipe.", "OK");
                return;
            }

            // Criar o popup e passar as equipes
            var popup = new SelecionarEquipePopup(equipes);
            popup.OnPopupClosed += (equipeSelecionada) =>
            {
                // L�gica para inscrever a equipe no evento
                if (equipeSelecionada != null)
                {
                    // Aqui voc� pode chamar um m�todo para inscrever a equipe no evento
                    InscreverEquipeNoEvento(evento, equipeSelecionada);
                }
            };

            await this.ShowPopupAsync(popup);
        }

        // M�todo para inscrever a equipe no evento
        private async void InscreverEquipeNoEvento(Evento evento, Equipe equipeSelecionada)
        {
            // Sua l�gica de inscri��o aqui
            await DisplayAlert("Inscri��o Confirmada", $"Equipe {equipeSelecionada.NomeEquipe} foi inscrita no evento {evento.NomeEvento}.", "OK");
        }
    }
}
