using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using App_AEE.Model;

namespace App_AEE.Services
{
    public class EventosService
    {
        private readonly HttpClient _httpClient;

        public EventosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para criar um evento
        public async Task<bool> CriarEvento(Evento evento)
        {
            var response = await _httpClient.PostAsJsonAsync("http://10.0.2.2:5053/api/eventos/criar", evento);

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
            var response = await _httpClient.PutAsJsonAsync("http://10.0.2.2:5053/api/eventos/editar/{id}", evento);

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
            var response = await _httpClient.DeleteAsync($"api/eventos/deletar/{id}");

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
            var response = await _httpClient.GetFromJsonAsync<Evento>($"http://10.0.2.2:5053/api/eventos/buscar/{id}");

            return response;
        }

        // Método para listar todos os eventos
        public async Task<List<Evento>> ListarEventos()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Evento>>("http://10.0.2.2:5053/api/eventos/listar");

            return response;
        }
    }
}
