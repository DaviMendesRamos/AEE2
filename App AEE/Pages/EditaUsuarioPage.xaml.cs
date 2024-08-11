using App_AEE.Model;

namespace App_AEE.Pages;

public partial class EditaUsuarioPage : ContentPage
{
	Usuario _usuario;
	public EditaUsuarioPage()
	{
		InitializeComponent();
		_usuario = new Usuario();

		this.BindingContext = _usuario;
	}

	private async void btnCadastrar_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_usuario.Email) && 
			string.IsNullOrWhiteSpace(_usuario.Senha))
		{
			await DisplayAlert("Atenção", "Preencha todas as informações", "Fechar");
			return;
		}

		var cadastro = await App.BancoDados.UsuarioDataTable.SalvaUsuario(_usuario);	
		
		if (cadastro > 0)
		{
			await Navigation.PopAsync();
		}
		
	}

	private async void btnVoltar_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}