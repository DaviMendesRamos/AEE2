using App_AEE.Model;
using CommunityToolkit.Maui;

namespace App_AEE.Pages
{
    public partial class ConsultarEventoPage : Shell, IQueryAttributable
    {
        // Propriedade para armazenar o evento selecionado
        public Evento EventoSelecionado { get; set; }

        // Propriedade para indicar se o usuário é admin
        public bool IsAdmin { get; set; } = true;

        public ConsultarEventoPage(Evento eventoSelecionado)
        {
            InitializeComponent();

            // Atribuindo o evento selecionado ao atributo EventoSelecionado
            EventoSelecionado = eventoSelecionado;

            // Definindo o BindingContext como o próprio objeto da página
            BindingContext = this;
        }

        // Implementação do IQueryAttributable para receber o evento selecionado
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("EventoSelecionado", out var eventoSelecionado) && eventoSelecionado is Evento evento)
            {
                EventoSelecionado = evento;
                BindingContext = this;  // Atualiza o BindingContext com o evento selecionado
            }
        }

        // Método para editar o evento (exemplo simples)
        private void EditarEvento_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Editar", "Função de edição ativada.", "OK");
        }
    }
}
