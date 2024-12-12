
using System.Collections.ObjectModel;
using System.Linq;
using App_AEE.Model;

using App_AEE.Services;

namespace App_AEE.Pages.AdminPages
{
    public partial class AdicionarAdminPage : ContentPage
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Usuario> Usuarios { get; set; }
        public Command BuscarUsuariosCommand { get; }
        public Command AdicionarAdminCommand { get; }
        public Usuario UsuarioSelecionado { get; set; }
        public bool IsUsuarioSelecionado => UsuarioSelecionado != null;

        public AdicionarAdminPage(ApiService apiService)
        {
            InitializeComponent();

            _apiService = apiService;
            Usuarios = new ObservableCollection<Usuario>();

            BuscarUsuariosCommand = new Command<string>(async (nome) => await BuscarUsuarios(nome));
            AdicionarAdminCommand = new Command(async () => await AdicionarAdmin());

            BindingContext = this;
        }

        // M�todo para buscar usu�rios conforme o texto de pesquisa
        private async Task BuscarUsuarios(string nome)
        {
            try
            {
                // Chama o m�todo para buscar os usu�rios
                var usuarios = await _apiService.GetUsuariosAsync(); // Chama o servi�o da API

                // Filtra a lista de usu�rios pelo nome (se o nome for fornecido)
                if (!string.IsNullOrEmpty(nome))
                {
                    usuarios = usuarios.Where(u => u.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Atualiza a lista de usu�rios na UI
                Usuarios.Clear();
                foreach (var usuario in usuarios)
                {
                    Usuarios.Add(usuario);
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, exiba uma mensagem (pode ser um alert ou outro tipo de tratamento)
                await DisplayAlert("Erro", $"Erro ao buscar usu�rios: {ex.Message}", "OK");
            }
        }

        // M�todo chamado quando um usu�rio � selecionado na lista
        private void OnUsuarioSelected(object sender, SelectedItemChangedEventArgs e)
        {
            UsuarioSelecionado = e.SelectedItem as Usuario;
            OnPropertyChanged(nameof(IsUsuarioSelecionado));
        }

        // M�todo para atribuir o administrador
        private async Task AdicionarAdmin()
        {
            if (UsuarioSelecionado != null)
            {
                bool sucesso = await _apiService.AtribuirAdministradorAsync(UsuarioSelecionado.Id);

                if (sucesso)
                {
                    await DisplayAlert("Sucesso", "Usu�rio agora � administrador!", "OK");
                    UsuarioSelecionado = null; // Limpar sele��o ap�s sucesso
                    OnPropertyChanged(nameof(IsUsuarioSelecionado));
                }
                else
                {
                    await DisplayAlert("Erro", "Falha ao adicionar o usu�rio como administrador.", "OK");
                }
            }
        }
    }

    
}
