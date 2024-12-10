using App_AEE.Services;
using App_AEE.Model;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using App_AEE.Validations;
using Microsoft.Maui.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

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
                    if (!string.IsNullOrEmpty(usuario.UrlImagem))
                    {
                        ImgBtnPerfil.Source = new UriImageSource
                        {
                            Uri = new Uri(usuario.UrlImagem),
                            CachingEnabled = true, // Habilitar cache para a imagem
                            CacheValidity = TimeSpan.FromDays(1) // Definir a validade do cache
                        };
                    }
                    else
                    {
                        // Se a URL da imagem não estiver disponível, exiba uma imagem padrão ou nada
                        ImgBtnPerfil.Source = "defaultProfileImage.png"; // Substitua pelo caminho de uma imagem padrão
                    }
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

        private async Task<byte[]?> SelecionarImagemAsync()
        {
            try
            {
                var arquivo = await MediaPicker.PickPhotoAsync();

                if (arquivo is null) return null;

                using (var stream = await arquivo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Erro", "A funcionalidade n o   suportada no dispositivo", "Ok");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Erro", "Permiss es n o concedidas para acessar a c mera ou galeria", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao selecionar a imagem: {ex.Message}", "Ok");
            }
            return null;
        }

        private async void ImgBtnPerfil_Clicked(object sender, EventArgs e)
        {
            try
            {
                var imagemArray = await SelecionarImagemAsync();
                if (imagemArray is null)
                {
                    await DisplayAlert("Erro", "Não foi possível carregar a imagem", "Ok");
                    return;
                }
                ImgBtnPerfil.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

                var response = await _apiService.UploadImagemUsuario(imagemArray);
                if (response.Data)
                {
                    await DisplayAlert("", "Imagem enviada com sucesso", "Ok");
                }
                else
                {
                    await DisplayAlert("Erro", response.ErrorMessage ?? "Ocorreu um erro desconhecido", "Cancela");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "Ok");
            }
        }

        private async void OnUploadClicked(object sender, EventArgs e)
        {
            try
            {
                var imagemArray = await SelecionarImagemAsync();
                if (imagemArray is null)
                {
                    await DisplayAlert("Erro", "Não foi possível carregar a imagem", "Ok");
                    return;
                }
                ImgBtnPerfil.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

                var response = await _apiService.UploadImagemUsuario(imagemArray);
                if (response.Data)
                {
                    await DisplayAlert("", "Imagem enviada com sucesso", "Ok");
                }
                else
                {
                    await DisplayAlert("Erro", response.ErrorMessage ?? "Ocorreu um erro desconhecido", "Cancela");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "Ok");
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
            Application.Current.MainPage = new NavigationPage(new LoginUsuarioPage(_apiService, _validator, _eventosService));
        }
    }
}
