using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using App_AEE.Model;
using Microsoft.Extensions.Logging;

namespace App_AEE.Services
{
    public class EventosService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public EventosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = AppConfig.BaseUrl;
        }

        // Método para criar um evento
        public async Task<bool> CriarEvento(Evento evento)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Eventos/criar", evento);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // Lógica de erro, se necessário
            return false;
        }

        // Método para editar um evento
        public async Task<bool> EditarEvento(int id, Evento evento)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}eventos/editar/{id}", evento);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // Lógica de erro, se necessário
            return false;
        }

        // Método para excluir um evento
        public async Task<bool> DeletarEvento(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}eventos/deletar/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // Lógica de erro, se necessário
            return false;
        }

        // Método para buscar um evento por ID
        public async Task<Evento> BuscarEventoPorId(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<Evento>($"{_baseUrl}eventos/buscar/{id}");

            return response;
        }

        // Método para listar todos os eventos
        public async Task<List<Evento>> ListarEventos()
        {
            // Faz a chamada para a API e deserializa a resposta diretamente
            var eventos = await _httpClient.GetFromJsonAsync<List<Evento>>( $"{_baseUrl}eventos/listar");

            if (eventos != null)
            {
                // Define DeveSerializarCod como true para todos os eventos retornados
                foreach (var evento in eventos)
                {
                    evento.DeveSerializarCod = true;
                }
            }

            return eventos ?? new List<Evento>(); // Garante que não haverá retorno nulo
        }

        public async Task<List<Equipe>> ListarEquipesPorEvento(int codEvento)
        {
            // Constrói a URL da API com o código do evento
            

            try
            {
                // Faz a requisição GET para a API
                var equipes = await _httpClient.GetFromJsonAsync<List<Equipe>>($"{_baseUrl}Cadastrar/{codEvento}/Equipes");

                // Retorna a lista de equipes ou uma lista vazia caso nulo
                return equipes ?? new List<Equipe>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro ao buscar equipes do evento: {ex.Message}");
                return new List<Equipe>();
            }
        }
    }
}
