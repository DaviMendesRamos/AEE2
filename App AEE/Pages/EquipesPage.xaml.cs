namespace App_AEE.Pages;

public partial class EquipesPage : ContentPage
{
	public EquipesPage()
	{
		InitializeComponent();
	}

	private async void CriarEquipeClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new CriarEquipePage());
    }
}