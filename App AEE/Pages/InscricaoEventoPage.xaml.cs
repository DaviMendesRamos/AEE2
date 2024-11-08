using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace App_AEE.Pages;

public partial class InscricaoEventoPage : ContentPage
{
    public ObservableCollection<Evento> Eventos { get; set; }
    public ICommand InscreverCommand { get; }

    public InscricaoEventoPage()
    {
        InitializeComponent();
        Eventos = new ObservableCollection<Evento>
            {
                new Evento { Nome = "Interclasse", Data = "15/11/2024", Imagem = "interclasse.png" },
                new Evento { Nome = "Jemupi", Data = "25/10/2024", Imagem = "jemupi.png" },

            };
        InscreverCommand = new Command<Evento>(OnInscrever);
        BindingContext = this;
    }

    private async void OnVerDetalhes(Evento evento)
    {
        // Lógica para mostrar detalhes do evento
        await DisplayAlert("Detalhes do Evento", $"Detalhes do evento {evento.Nome}.", "OK");
    }

    private async void OnInscrever(Evento evento)
    {
        // Lógica para inscrever o usuário no evento
        // Pode ser uma chamada para uma API ou apenas uma notificação
        await DisplayAlert("Inscrição", $"Você se inscreveu no {evento.Nome}!", "OK");
    }
}

public class Evento
{
    public string Nome { get; set; }
    public string Data { get; set; }
    public string Imagem { get; set; }
}