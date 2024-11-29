namespace App_AEE.Pages
{
	public partial class ErrorPage : ContentPage
	{
		public ErrorPage()
		{
			InitializeComponent();
			DisplayAlert("Erro Crítico", "Algo deu errado no aplicativo", "OK");
		}
	}
}
