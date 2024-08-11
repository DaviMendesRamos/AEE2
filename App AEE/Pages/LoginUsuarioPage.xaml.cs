namespace App_AEE.Pages;

public partial class LoginUsuarioPage : ContentPage
{
	public LoginUsuarioPage()
	{
		InitializeComponent();
	}

	private void btnRegistrar_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new EditaUsuarioPage());
	}

	private void btnEntrar_Clicked(object sender, EventArgs e)
	{

	}
}