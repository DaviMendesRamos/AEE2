using App_AEE.Model;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace App_AEE.Services
{
	public class ApiService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<ApiService> _logger;

		private const string BaseUrl = "https://192.168.1.6:7066/"; // Certifique-se de usar a URL correta
		private readonly JsonSerializerOptions _serializerOptions;

		public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
		{
			// Configuração para ignorar erros de certificado SSL (em um ambiente de desenvolvimento)
			var handler = new HttpClientHandler
			{
				// Ignora erros de certificado SSL
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};

			// Criando o HttpClient com o handler configurado
			_httpClient = new HttpClient(handler);
			_logger = logger;
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
		}

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

				var json = JsonSerializer.Serialize(register, _serializerOptions);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await PostRequest("api/Usuarios/Register", content);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
					return new ApiResponse<bool>
					{
						ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
					};
				}

				return new ApiResponse<bool> { Data = true };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro ao registrar o usuário: {ex.Message}");
				return new ApiResponse<bool> { ErrorMessage = ex.Message };
			}
		}

		public async Task<ApiResponse<bool>> Login(string email, string password)
		{
			try
			{
				var login = new Login
				{
					Email = email,
					Senha = password
				};

				var json = JsonSerializer.Serialize(login, _serializerOptions);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await PostRequest("api/Usuarios/Login", content);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
					return new ApiResponse<bool>
					{
						ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
					};
				}

				var jsonResult = await response.Content.ReadAsStringAsync();
				var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

				Preferences.Set("accesstoken", result!.AccessToken);
				Preferences.Set("usuarioid", (int)result.UsuarioId!);
				Preferences.Set("usuarionome", result.UsuarioNome);

				return new ApiResponse<bool> { Data = true };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro no login: {ex.Message}");
				return new ApiResponse<bool> { ErrorMessage = ex.Message };
			}
		}

		private async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content)
		{
			var enderecoUrl = BaseUrl + uri;
			try
			{
				var result = await _httpClient.PostAsync(enderecoUrl, content);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Erro ao enviar requisição POST para {uri}: {ex.Message}");
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
		}
	}
}
