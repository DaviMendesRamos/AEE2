using App_AEE.Services;
using App_AEE.Model;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using App_AEE.Validations;
using Microsoft.Maui.Storage;

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
        private async Task<byte[]?> SelecionarImagemAsync()
        {
            try
            {
                // Abre o seletor de fotos
                var arquivo = await MediaPicker.PickPhotoAsync();

                if (arquivo is null)
                {
                    await DisplayAlert("Aten��o", "Nenhuma imagem foi selecionada.", "Ok");
                    return null;
                }

                using (var stream = await arquivo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Erro", "A funcionalidade n�o � suportada neste dispositivo.", "Ok");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Erro", "Permiss�es n�o concedidas para acessar a c�mera ou galeria.", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao selecionar a imagem: {ex.Message}", "Ok");
            }
            return null;
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            // Selecionar imagem
            try
            {
                // Seleciona a imagem do dispositivo
                var imagemArray = await SelecionarImagemAsync();
                if (imagemArray is null)
                {
                    await DisplayAlert("Erro", "N�o foi poss�vel carregar a imagem.", "Ok");
                    return;
                }

                // Atualiza a visualiza��o da imagem no bot�o
                ImgBtnPerfil.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

                // Envia a imagem para o servidor usando ApiService
                var response = await _apiService.UploadImagemUsuario(imagemArray);
                if (response.Data)
                {
                    await DisplayAlert("Sucesso", "Imagem enviada com sucesso!", "Ok");
                }
                else
                {
                    await DisplayAlert("Erro", response.ErrorMessage ?? "Ocorreu um erro ao enviar a imagem.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "Ok");
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
            Application.Current.MainPage = new NavigationPage(new LoginUsuarioPage(_apiService, _validator, _eventosService));
        }
    }
}
