using App_AEE.Model; // Certifique-se de usar o namespace correto para o modelo "Evento"
using App_AEE.Services;
using App_AEE.Pages;
using System.Collections.ObjectModel;

namespace App_AEE.Pages.AdminPages
{
    public partial class EditarEventosPage : ContentPage
    {
        private readonly EventosService _eventosService;

        // Command for editing event
        public Command<Evento> EditarCommand { get; }

        // ObservableCollection to bind to the UI
        public ObservableCollection<Evento> Eventos { get; set; } = new ObservableCollection<Evento>();

        public EditarEventosPage(EventosService eventoService)
        {
            _eventosService = eventoService;
            InitializeComponent();

            // Initialize EditarCommand to navigate to event details
            EditarCommand = new Command<Evento>(AbrirConsultarEventoPage);

            // Set the BindingContext to this page
            BindingContext = this;
        }

        // OnAppearing method to load events when the page is shown
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ListarEventos();
        }

        // Fetch events from the service and update the ObservableCollection
        private async Task ListarEventos()
        {
            var eventos = await _eventosService.ListarEventos();

            // If no events are found, show an error message
            if (eventos == null)
            {
                await DisplayAlert("Erro", "Nenhum evento foi encontrado. Verifique a conexão ou tente novamente.", "OK");
                return;
            }

            // If events are found, bind them to the collection view
            if (eventos.Any())
            {
                Eventos.Clear();
                foreach (var evento in eventos)
                {
                    Eventos.Add(evento);
                }
            }
        }

        // Command method to navigate to the event details page
        private async void AbrirConsultarEventoPage(Evento eventoSelecionado)
        {
            // If no event is selected, return
            if (eventoSelecionado == null)
                return;

            // Navigate to the ConsultarEventoPage with the selected event
            await Navigation.PushAsync(new ConsultarEventoPage(eventoSelecionado));
        }
    }
}
