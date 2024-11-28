namespace App_AEE.Pages;
using App_AEE.Services;
using App_AEE.Validations;


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

            var response = await _apiService.Login(txtEmail.Text, txtSenha.Text);

            if (!response.HasError)
            {
                Application.Current!.MainPage = new AppShell(_apiService, _validator);
            }
            else
            {
                await DisplayAlert("Erro", "Algo deu errado", "Cancelar");
            }

        }

        private async void TapRegister_Tapped(object sender, TappedEventArgs e)
        {

            await Navigation.PushAsync(new RegistroUsuarioPage(_apiService, _validator));

        }
    
}