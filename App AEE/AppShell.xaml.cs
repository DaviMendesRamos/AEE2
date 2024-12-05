using App_AEE.Validations;
using App_AEE.Services;
using App_AEE.Pages;
using App_AEE.Pages.AdminPages;

namespace App_AEE
 
{
	public partial class AppShell : Shell
	{

        private readonly ApiService _apiService;
        private readonly IValidator _validator;


        public AppShell(ApiService apiService, IValidator validator)
        {
            InitializeComponent();
            _apiService = apiService;
            _validator = validator;
            Routing.RegisterRoute("HomeAdmin", typeof(HomeAdminPage));
            Routing.RegisterRoute("HomePrincipal", typeof(HomePrincipalPage));

            ConfigureHomeRoute();
        }
        private void ConfigureHomeRoute()
        {
            var role = Preferences.Get("role", string.Empty);

            // Redireciona o "HOME" para a página correta com base na role
            if (role == "admin")
            {
                GoToAsync("//HomeAdmin");
            }
            else
            {
                GoToAsync("//HomePrincipal");
            }
        }
    }
}

