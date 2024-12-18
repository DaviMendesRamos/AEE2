using System.Collections.ObjectModel;
using App_AEE.Model;
using CommunityToolkit.Maui;

namespace App_AEE.Pages;

public partial class ConsultarEventoPage : TabbedPage
{
    public bool IsAdmin { get; set; } = true; // Simula a verificação de admin

    public ObservableCollection<Equipe> Equipes { get; set; }
    

    private void EditarEvento_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Editar", "Função de edição ativada.", "OK");
    }
}


		