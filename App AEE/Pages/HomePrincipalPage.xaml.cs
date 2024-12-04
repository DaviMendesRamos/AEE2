using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;


namespace App_AEE.Pages;

public partial class HomePrincipalPage : ContentPage
{
	public HomePrincipalPage()
	{
		InitializeComponent();


	}
    private async void OnInscreverEventoTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventosPage());
    }
}
