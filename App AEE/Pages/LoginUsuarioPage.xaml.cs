using App_AEE.Services;
using App_AEE.Validations;
namespace App_AEE.Pages
{
	public partial class LoginUsuarioPage : ContentPage
	{
		private readonly ApiService _apiService;
		private readonly IValidator _validator;

		public LoginUsuarioPage(ApiService apiService, IValidator validator)
		{
			InitializeComponent();
			_apiService = apiService;
			_validator = validator;
		}

		private async void btnEntrar_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txtEmail.Text))
			{
				await DisplayAlert("Erro", "Informe o email", "Cancelar");
				return;
			}

			if (string.IsNullOrEmpty(txtSenha.Text))
			{
				await DisplayAlert("Erro", "Informe a senha", "Cancelar");
				return;
			}

			// Chama o serviço de login
			var response = await _apiService.Login(txtEmail.Text, txtSenha.Text);

			if (!response.HasError)
			{
				// Se o login for bem-sucedido, redireciona para a página principal
				Application.Current!.MainPage = new AppShell(_apiService, _validator);
			}
			else
			{
				// Exibe a mensagem de erro retornada pela API
				await DisplayAlert("Erro", response.ErrorMessage ?? "Algo deu errado", "Cancelar");
			}
		}

		private async void btnRegistrar_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new RegistroUsuarioPage(_apiService, _validator));
		}
	}
}
