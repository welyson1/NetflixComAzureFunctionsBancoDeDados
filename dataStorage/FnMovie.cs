using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NetflixWelyson
{
    public class FnMovie
    {
        private readonly ILogger<FnMovie> _logger;

        public FnMovie(ILogger<FnMovie> logger)
        {
            _logger = logger;
        }

        [Function("FnMovie")]
        [CosmosDBOutput("%DatabaseName%", "movies", Connection = "CosmoDBConnection", CreateIfNotExists = true, PartitionKey = "id")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            MovieRequest movie = null;

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                movie = JsonConvert.DeserializeObject<MovieRequest>(content);

                // Garante que o ID esteja presente antes de salvar no CosmosDB
                if (string.IsNullOrEmpty(movie.Id))
                {
                    movie.Id = Guid.NewGuid().ToString();
                }
            }

            catch (Exception ex)
            {                
                return new BadRequestObjectResult("Erro ao deserializar o objeto: " + ex.Message);
            }

            return new OkObjectResult(JsonConvert.SerializeObject(movie));
        }
    }
}