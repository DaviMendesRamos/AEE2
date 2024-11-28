using App_AEE.Validations;
using App_AEE.Services;

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
            
        }
    }
}
