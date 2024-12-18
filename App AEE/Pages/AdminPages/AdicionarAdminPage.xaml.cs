using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using App_AEE.Model;
using App_AEE.Services;

namespace App_AEE.Pages.AdminPages
{
    public partial class AdicionarAdminPage : ContentPage
    {
        private readonly ApiService _apiService;

        public AdicionarAdminPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ListarUsuarios();
        }

        private async Task ListarUsuarios()
        {
            var usuarios = await _apiService.ListarUsuarios();

            if (usuarios != null && usuarios.Any())
            {
                UsuariosLayout.Children.Clear();

                foreach (var usuario in usuarios)
                {
                    var stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(10),
                        Spacing = 10
                    };

                    var label = new Label
                    {
                        Text = usuario.Nome,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        TextColor = Colors.White
                    };

                    var button = new Button
                    {
                        Text = AdminButtonTextConverter(usuario.IsAdmin),
                        BackgroundColor = AdminButtonColorConverter(usuario.IsAdmin),
                        TextColor = Colors.White,
                        HorizontalOptions = LayoutOptions.End
                    };

                    button.Clicked += async (sender, args) =>
                    {
                        usuario.IsAdmin = !usuario.IsAdmin;
                        button.Text = AdminButtonTextConverter(usuario.IsAdmin);
                        button.BackgroundColor = AdminButtonColorConverter(usuario.IsAdmin);

                        bool sucesso = await _apiService.AtribuirAdministradorAsync(usuario.Id);

                        if (!sucesso)
                        {
                            usuario.IsAdmin = !usuario.IsAdmin;
                            button.Text = AdminButtonTextConverter(usuario.IsAdmin);
                            button.BackgroundColor = AdminButtonColorConverter(usuario.IsAdmin);
                            await DisplayAlert("Erro", "Não foi possível alterar o estado de administrador.", "OK");
                        }
                    };

                    stackLayout.Children.Add(label);
                    stackLayout.Children.Add(button);
                    UsuariosLayout.Children.Add(stackLayout);
                }
            }
        }

        private string AdminButtonTextConverter(bool isAdmin)
        {
            return isAdmin ? "Admin" : "User";
        }

        private Color AdminButtonColorConverter(bool isAdmin)
        {
            return isAdmin ? Colors.Green : Colors.Gray;
        }
    }
}
