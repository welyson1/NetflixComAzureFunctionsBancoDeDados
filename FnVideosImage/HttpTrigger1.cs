using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Fn.NetflixWelyson
{
    public class HttpTrigger1
    {
        private readonly ILogger<HttpTrigger1> _logger;

        public HttpTrigger1(ILogger<HttpTrigger1> logger)
        {
            _logger = logger;
        }

        [Function("dataStorage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processando a mídia");

            try
            {
                if (!req.Headers.TryGetValue("file-type", out var fileTypeHeader))
                {
                    return new BadRequestObjectResult("O cabeçalho 'file-type' é obrigatório");
                }

                var fileType = fileTypeHeader.ToString();
                string[] allowedContentTypes = { "image/png", "image/jpeg", "video/mp4", "video/mkv" };

                // Verifica se o Content-Type é suportado
                if (!allowedContentTypes.Any(ct => req.ContentType.StartsWith(ct)))
                {
                    return new BadRequestObjectResult("Formato de arquivo não suportado");
                }

                if (req.ContentType.StartsWith("multipart/form-data"))
                {
                    // Upload via formulário multipart
                    var form = await req.ReadFormAsync();
                    var file = form.Files["file"];

                    if (file == null || file.Length == 0)
                    {
                        return new BadRequestObjectResult("O arquivo não foi enviado");
                    }

                    return await UploadToBlobStorage(file.OpenReadStream(), file.FileName, fileType);
                }
                else
                {
                    // Upload direto (vídeo ou imagem no corpo da requisição)
                    string extension = req.ContentType.StartsWith("image/") ? ".jpg" : ".mp4"; // Ajusta extensão
                    string fileName = $"upload-{Guid.NewGuid()}{extension}";

                    return await UploadToBlobStorage(req.Body, fileName, fileType);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Erro ao processar arquivo: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        private async Task<IActionResult> UploadToBlobStorage(Stream fileStream, string fileName, string fileType)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = fileType;
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            var blob = containerClient.GetBlobClient(fileName);
            await blob.UploadAsync(fileStream, true);

            return new OkObjectResult(new
            {
                Message = "Arquivo armazenado com sucesso",
                BlobUri = blob.Uri
            });
        }
    }
}