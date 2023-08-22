using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;

namespace VianaSoft.WhenAll.Api.Controllers
{
    [ApiController]
    [Route("v1/WhenAll")]
    public class WhenAllController : ControllerBase
    {
        [HttpGet]
        [Route("paged")]
        [SwaggerOperation(Summary = "Exemplo de código para fazer chamadas assíncronas em três APIs diferentes e aguardar o retorno das chamadas.")]
        public async Task<IActionResult> GetAllPagedAsync()
        {
            // URLs das três APIs diferentes
            string apiUrl1 = "https://api.adviceslip.com/advice";
            string apiUrl2 = "https://api.chucknorris.io/jokes/random";
            string apiUrl3 = "https://jsonplaceholder.typicode.com/todos/1";

            // Fazer chamadas assíncronas para as três APIs e aguardar o resultado
            var task1 = FetchApiData(apiUrl1);
            var task2 = FetchApiData(apiUrl2);
            var task3 = FetchApiData(apiUrl3);

            // Aguardar o término de todas as tarefas
            await Task.WhenAll(task1, task2, task3);

            // Acessar os resultados das chamadas
            var item = new 
            {
                result1 = task1.Result,
                result2 = task2.Result,
                result3 = task3.Result
            };

            return Ok(item);
        }

        static async Task<string> FetchApiData(string apiUrl)
        {
            // Criar um cliente HttpClient para fazer as chamadas HTTP
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }
            else
            {
                throw new Exception($"Erro ao chamar a API: {response.StatusCode}");
            }
        }
    }
}