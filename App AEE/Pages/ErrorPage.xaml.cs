namespace App_AEE.Pages
{
	public partial class ErrorPage : ContentPage
	{
		public ErrorPage()
		{
			InitializeComponent();
			DisplayAlert("Erro Cr�tico", "Algo deu errado no aplicativo", "OK");
		}
	}
}
