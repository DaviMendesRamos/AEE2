using App_AEE.Model;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace App_AEE.Pages
{
    public partial class ConsultarEventoPage : TabbedPage
    {
        public Evento EventoSelecionado { get; set; }
        public bool IsAdmin { get; set; } = true;

        public ConsultarEventoPage(Evento eventoSelecionado)
        {
            InitializeComponent();
            EventoSelecionado = eventoSelecionado;
            BindingContext = this;
        }

        private async void EditarEvento_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Editar", "Função de edição ativada.", "OK");
        }
    }
}
