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

        // Carregar dados do usuário ao carregar a página
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarDadosUsuario();
        }

        // Método para carregar os dados do usuário
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
                    // Exibe uma mensagem de erro ou vazio se o usuário não for encontrado
                    lblNome.Text = "Erro ao carregar os dados do usuário.";
                    lblEmail.Text = string.Empty;
                    lblTelefone.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Tratamento de erro em caso de falha na requisição
                lblNome.Text = "Erro ao carregar os dados.";
                lblEmail.Text = string.Empty;
                lblTelefone.Text = string.Empty;
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        // Método chamado ao clicar no botão de editar
        private async void OnEditarClicked(object sender, EventArgs e)
        {
            // Navegar para uma página de edição (você precisa criar a página de edição)
            await Navigation.PushAsync(new EditarUsuarioPage());
        }

        // Método chamado ao clicar no botão de deslogar
        private async void btndeslogar_Clicked(object sender, EventArgs e)
        {
            // Realiza o logout, limpando as preferências ou o token
            Preferences.Clear(); // Limpa todas as preferências

            // Navega para a página de login após deslogar
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new LoginUsuarioPage(_apiService, _validator, _eventosService));
        }
    }
}
