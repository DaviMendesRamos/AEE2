using App_AEE.Model;
using App_AEE.Validations;
using App_AEE.Services;
using App_AEE.Pages;

namespace App_AEE.Pages;

public partial class RegistroUsuarioPage : ContentPage
{
	Usuario _usuario;
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    public RegistroUsuarioPage(ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
    }
    private async void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        if (await _validator.Validar(txtNome.Text, txtEmail.Text, txtTelefone.Text, txtSenha.Text))
        {

            var response = await _apiService.RegistrarUsuario(txtNome.Text, txtEmail.Text, txtTelefone.Text, txtSenha.Text);

            if (!response.HasError)
            {
                await DisplayAlert("Aviso", "Sua conta foi criada com sucesso !!", "OK");
                await Navigation.PushAsync(new LoginUsuarioPage(_apiService, _validator));
            }
            else
            {
                await DisplayAlert("Erro", "Algo deu errado!!!", "Cancelar");
            }
        }
        else
        {
            string mensagemErro = "";
            mensagemErro += _validator.NomeErro != null ? $"\n- {_validator.NomeErro}" : "";
            mensagemErro += _validator.EmailErro != null ? $"\n- {_validator.EmailErro}" : "";
            mensagemErro += _validator.TelefoneErro != null ? $"\n- {_validator.TelefoneErro}" : "";
            mensagemErro += _validator.SenhaErro != null ? $"\n- {_validator.SenhaErro}" : "";

            await DisplayAlert("Erro", mensagemErro, "OK");
        }
    }

    private async void TapLogin_TappedAsync(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginUsuarioPage(_apiService, _validator));
    }
}