using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NetflixWelyson
{
    public class MovieRequest
    {
        [JsonProperty("id")] // Garante que o nome da propriedade no JSON seja "id"
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("video")]
        public string Video { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }

}