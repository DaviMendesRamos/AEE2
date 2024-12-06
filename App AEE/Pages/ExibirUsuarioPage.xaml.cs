using App_AEE.Services;
using App_AEE.Model;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using App_AEE.Validations;

namespace App_AEE.Pages
{
    public partial class ExibirUsuarioPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly IValidator _validator;
        private readonly EventosService _eventosService;

        public ExibirUsuarioPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
        }

        // Carregar dados do usu�rio ao carregar a p�gina
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarDadosUsuario();
        }

        // M�todo para carregar os dados do usu�rio
        private async Task CarregarDadosUsuario()
        {
            try
            {
                var usuario = await _apiService.GetUsuarioAtualAsync();
                if (usuario != null)
                {
                    lblNome.Text = "Nome: " + usuario.Nome;
                    lblEmail.Text = "Email: " + usuario.Email;
                    lblTelefone.Text = "Telefone: " + usuario.Telefone;
                }
                else
                {
                    // Exibe uma mensagem de erro ou vazio se o usu�rio n�o for encontrado
                    lblNome.Text = "Erro ao carregar os dados do usu�rio.";
                    lblEmail.Text = string.Empty;
                    lblTelefone.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Tratamento de erro em caso de falha na requisi��o
                lblNome.Text = "Erro ao carregar os dados.";
                lblEmail.Text = string.Empty;
                lblTelefone.Text = string.Empty;
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        // M�todo chamado ao clicar no bot�o de editar
        private async void OnEditarClicked(object sender, EventArgs e)
        {
            // Navegar para uma p�gina de edi��o (voc� precisa criar a p�gina de edi��o)
            await Navigation.PushAsync(new EditarUsuarioPage());
        }

        // M�todo chamado ao clicar no bot�o de deslogar
        private async void btndeslogar_Clicked(object sender, EventArgs e)
        {
            // Realiza o logout, limpando as prefer�ncias ou o token
            Preferences.Clear(); // Limpa todas as prefer�ncias

            // Navega para a p�gina de login ap�s deslogar
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new LoginUsuarioPage(_apiService, _validator, _eventosService));
        }
    }
}
