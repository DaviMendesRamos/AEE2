using App_AEE.Services;
using CommunityToolkit.Maui.Views;
using App_AEE.Model;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace App_AEE.Pages
{
    public partial class EventosPage : ContentPage
    {
        private readonly EventosService _eventoService;
        private readonly ApiService _apiService;
        public ObservableCollection<Evento> Eventos { get; set; } = new ObservableCollection<Evento>();
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
                // Verifique os eventos carregados
                foreach (var evento in eventos)
                {
                    Debug.WriteLine($"Evento: {evento.NomeEvento}, CodEvento: {evento.CodEvento}");
                }

                Eventos.Clear();
                foreach (var evento in eventos)
                {
                    Eventos.Add(evento);
                }
            }
        }

        // M�todo chamado quando o usu�rio clicar no bot�o "Inscrever"
        public async void InscreverEquipe(Evento evento)
        {
            if (evento == null)
            {
                await DisplayAlert("Erro", "Evento � nulo. Verifique se foi passado corretamente.", "OK");
                return;
            }

            if (evento.CodEvento <= 0)
            {
                await DisplayAlert("Erro", $"Evento inv�lido. CodEvento: {evento.CodEvento}", "OK");
                return;
            }


            // Lista de equipes do usu�rio
            var equipes = await _apiService.ListarEquipesDoUsuario();

            if (equipes == null || equipes.Count == 0)
            {
                await DisplayAlert("Erro", "Voc� n�o est� em nenhuma equipe.", "OK");
                return;
            }

            // Criar o popup e passar as equipes
            var popup = new SelecionarEquipePopup(equipes);
            popup.OnPopupClosed += async (equipeSelecionada) =>
            {
                // L�gica para inscrever a equipe no evento
                if (equipeSelecionada != null)
                {
                    await InscreverEquipeNoEvento(evento, equipeSelecionada);
                }
                else
                {
                    await DisplayAlert("Aten��o", "Nenhuma equipe foi selecionada.", "OK");
                }
            };

            await this.ShowPopupAsync(popup);
        }

        // M�todo para inscrever a equipe no evento
        private async Task InscreverEquipeNoEvento(Evento evento, Equipe equipeSelecionada)
        {
            if (evento == null || equipeSelecionada == null)
            {
                await DisplayAlert("Erro", "Evento ou equipe n�o foram selecionados.", "OK");
                return;
            }

            // Confirma��o antes de inscrever
            bool confirm = await DisplayAlert("Confirma��o",
                $"Deseja inscrever a equipe {equipeSelecionada.NomeEquipe} no evento {evento.NomeEvento}?",
                "Sim", "N�o");

            if (!confirm) return;

            // Chamar o m�todo da API para inscrever a equipe no evento
            var resultado = await _apiService.InscreverEquipeAsync(evento.CodEvento, equipeSelecionada.CodEquipe);

            if (!resultado.HasError)
            {
                await DisplayAlert("Sucesso", $"Equipe {equipeSelecionada.NomeEquipe} foi inscrita no evento {evento.NomeEvento}.", "OK");
            }
            else
            {
                await DisplayAlert("Erro", resultado.ErrorMessage, "OK");
            }
        }
    }
}
