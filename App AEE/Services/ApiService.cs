using Microsoft.Extensions.Logging;
using System.Text.Json;
using App_AEE.Services;
using App_AEE.Model;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using App_AEE;


public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly string _baseUrl;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = AppConfig.BaseUrl;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<bool> IsApiDisponivel()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}status");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao verificar disponibilidade da API: {ex.Message}");
            return false;
        }
    }

    // Método de registro
    public async Task<ApiResponse<bool>> RegistrarUsuario(string nome, string email, string telefone, string password)
    {
        try
        {
            var register = new Register
            {
                Nome = nome,
                Email = email,
                Telefone = telefone,
                Senha = password
            };

            var content = new StringContent(JsonSerializer.Serialize(register, _serializerOptions), Encoding.UTF8, "application/json");
            var response = await PostRequest($"{_baseUrl}Usuarios/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro no registro: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registrar o usuário: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    // Método de login
    public async Task<ApiResponse<bool>> Login(string email, string password)
    {
        try
        {
            var login = new Login
            {
                Email = email,
                Senha = password
            };

            // Serializa os dados de login e define o content-type como JSON
            var content = new StringContent(JsonSerializer.Serialize(login, _serializerOptions), Encoding.UTF8, "application/json");
            var response = await PostRequest($"{_baseUrl}Usuarios/Login", content);

            // Verifica se a resposta é bem-sucedida
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro no login: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }

            // Desserializa a resposta para o objeto Token
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

            if (result == null)
            {
                return new ApiResponse<bool> { ErrorMessage = "Resposta inválida da API." };
            }

            // Armazena os dados importantes do token nas preferências
            Preferences.Set("accesstoken", result.AccessToken);
            Preferences.Set("usuarioid", result.UsuarioId ?? 0);
            Preferences.Set("usuarionome", result.UsuarioNome);
            Preferences.Set("role", result.Role); // Adiciona o papel do usuário (admin/user)

            // Validação opcional do token JWT
            if (!IsTokenValid(result.AccessToken))
            {
                return new ApiResponse<bool> { ErrorMessage = "Token inválido" };
            }

            // Retorna sucesso com base na role
            _logger.LogInformation($"Usuário logado com role: {result.Role}");
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro no login: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    // Método para verificar se o token JWT é válido
    private bool IsTokenValid(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ZdYM000OLlMQG6VVVp1OH7RxtuEfGvBnXarp7gHuw1qvUC5dcGt3SNM"); // Utilize sua chave secreta ou configure-a no appsettings.json

            // Validação do token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Valida o token
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return true; // Token válido
        }
        catch
        {
            return false; // Token inválido
        }
    }
    public async Task<Usuario> GetUsuarioAtualAsync()
    {
        try
        {
            // Obtendo o token JWT armazenado
            var token = Preferences.Get("accesstoken", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Token JWT não encontrado.");
                return null;
            }


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            // Faz a requisição GET para buscar os dados do usuário
            var response = await _httpClient.GetAsync($"{_baseUrl}Usuarios/GetUsuarioAtual");

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var usuario = JsonSerializer.Deserialize<Usuario>(jsonResult, _serializerOptions);
                return usuario;
            }
            else
            {
                _logger.LogError($"Erro ao obter dados do usuário: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao obter dados do usuário: {ex.Message}");
            return null;
        }
    }
    public async Task<bool> AtualizarUsuarioAsync(EditaUsuario usuarioAtualizado)
    {
        try
        {
            var json = JsonSerializer.Serialize(usuarioAtualizado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}Usuarios/Editar", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
            return false;
        }
    }
    public async Task<ApiResponse<bool>> UploadImagemUsuario(byte[] imageArray)
    {
        try
        {
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(imageArray), "imagem", "image.jpg");
            var response = await PostRequest($"{_baseUrl}usuarios/uploadfotousuario", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                  ? "Unauthorized"
                  : $"Erro ao enviar requisição HTTP: {response.StatusCode}";

                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao fazer upload da imagem do usuário: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }
    public async Task<List<Usuario>> ListarUsuarios()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Usuario>>($"{_baseUrl}Usuarios/ListarUsuarios");

        return response;
    }
    public async Task<bool> AtribuirAdministradorAsync(int usuarioId)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Usuarios/AtribuirAdministrador", usuarioId);

            if (response.IsSuccessStatusCode)
            {
                // Caso o usuário seja atribuído como administrador com sucesso
                return true;
            }
            else
            {
                // Caso a API retorne um erro (status diferente de 2xx)
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro ao atribuir administrador: {error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            // Exceção se houver problemas de rede ou algo inesperado
            Console.WriteLine($"Erro de comunicação com a API: {ex.Message}");
            return false;
        }
    }
    // Método para criar uma nova equipe
    public async Task<ApiResponse<Equipe>> CriarEquipe(Equipe novaEquipe)
    {
        try
        {
            // Recupera o token JWT armazenado localmente (ajuste conforme necessário)
            var token = Preferences.Get("accesstoken", string.Empty);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<Equipe> { ErrorMessage = "Token JWT não encontrado." };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serializa a equipe para o formato JSON
            var json = JsonSerializer.Serialize(novaEquipe, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Adiciona o cabeçalho Authorization com o token JWT
           

            // Envia a requisição POST para a API
            var response = await PostRequest($"{_baseUrl}Equipe/criarEquipe", content);

            // Verifica se a resposta foi bem-sucedida
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao criar equipe: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<Equipe> { ErrorMessage = errorMessage };
            }

            // Processa a resposta bem-sucedida
            var jsonResult = await response.Content.ReadAsStringAsync();
            var equipeCriada = JsonSerializer.Deserialize<Equipe>(jsonResult, _serializerOptions);

            return new ApiResponse<Equipe> { Data = equipeCriada };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar equipe: {ex.Message}");
            return new ApiResponse<Equipe> { ErrorMessage = ex.Message };
        }
    }
    public async Task<List<Equipe>> ListarEquipesDoUsuario()
    {
        var token = Preferences.Get("accesstoken", string.Empty);

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogError("Token JWT não encontrado.");
            return null;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"{_baseUrl}Equipe/equipesDoUsuario");

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            _logger.LogInformation("Usuário não está em nenhuma equipe.");
            return new List<Equipe>();
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao buscar equipes: {response.StatusCode} - {errorMessage}");
            throw new Exception($"Erro ao buscar as equipes do usuário: {response.ReasonPhrase}");
        }

        var jsonResult = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(jsonResult))
        {
            _logger.LogInformation("Resposta vazia, retornando lista vazia.");
            return new List<Equipe>();
        }

        // Desserializa a resposta em uma lista de Equipe
        var equipes = JsonSerializer.Deserialize<List<Equipe>>(jsonResult, _serializerOptions) ?? new List<Equipe>();

        // Verifica se CodEquipe está presente (não é nulo ou zero)
        foreach (var equipe in equipes)
        {
            if (equipe.CodEquipe == null || equipe.CodEquipe == 0)
            {
                _logger.LogWarning($"Equipe com nome '{equipe.NomeEquipe}' retornada sem CodEquipe válido.");
                throw new Exception($"Erro ao processar equipe '{equipe.NomeEquipe}'. Identificador inválido.");
            }
        }

        return equipes;
    }
    // Método para buscar uma equipe pelo nome
    public async Task<ApiResponse<Equipe>> BuscarEquipePorNome(string nomeEquipe)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}Equipe/buscar/{nomeEquipe}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao buscar equipe: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<Equipe> { ErrorMessage = errorMessage };
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var equipe = JsonSerializer.Deserialize<Equipe>(jsonResult, _serializerOptions);

            return new ApiResponse<Equipe> { Data = equipe };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao buscar equipe: {ex.Message}");
            return new ApiResponse<Equipe> { ErrorMessage = ex.Message };
        }
    }
    // Método para deletar uma equipe pelo nome
    public async Task<ApiResponse<bool>> DeletarEquipePorNome(string nomeEquipe)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}Equipe/deletar/{nomeEquipe}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao deletar equipe: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao deletar equipe: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }
    // Método auxiliar para requisições POST
    public async Task<ApiResponse<string>> InscreverEquipeAsync(int codevento, int codequipe)
    {
        try
        {
            // Recupera o token JWT armazenado localmente (ajuste conforme necessário)
            var token = Preferences.Get("accesstoken", string.Empty);
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<string> { ErrorMessage = "Token JWT não encontrado." };
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Cria o objeto de requisição
            var requestPayload = new
            {
                CodEvento = codevento,
                CodEquipe = codequipe
            };
            Console.WriteLine($"Dados enviados: EventoId = {codevento}, EquipeId = {codequipe}");
            // Serializa o objeto de requisição para JSON
            var json = JsonSerializer.Serialize(requestPayload, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Envia a requisição POST para a API
            var response = await _httpClient.PostAsync($"{_baseUrl}Cadastrar/InscreverEquipe", content);

            // Verifica se a resposta foi bem-sucedida
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao inscrever equipe: {response.StatusCode} - {errorMessage}");
                return new ApiResponse<string> { ErrorMessage = errorMessage };
            }

            // Processa a resposta bem-sucedida
            var resultado = await response.Content.ReadAsStringAsync();

            return new ApiResponse<string> { Data = resultado };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao inscrever equipe: {ex.Message}");
            return new ApiResponse<string> { ErrorMessage = ex.Message };
        }
    }
    // Enviar requisição POST
    private async Task<HttpResponseMessage> PostRequest(string endpoint, HttpContent content)
    {
        try
        {
            return await _httpClient.PostAsync(endpoint, content);
        }
        catch (HttpRequestException ex)
        {

            _logger.LogError($"Erro na requisição HTTP para {endpoint}: {ex.Message}");
            throw new Exception("Não foi possível conectar ao servidor.");
        }
    }
}

