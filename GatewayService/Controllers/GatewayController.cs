using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace ResponsibilityServiceGateway.Controllers
{
    [ApiController]
    [Route("gateway/responsibility")]
    public class ResponsibilityGatewayController : ControllerBase
    {
        private readonly ILogger<ResponsibilityGatewayController> _logger;
        private readonly HttpClient _httpClient;

        public ResponsibilityGatewayController(ILogger<ResponsibilityGatewayController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetResponsibilitiesForSample(int sampleId, int repositoryId)
        {
            try
            {
                _logger.LogInformation("Получение обязанностей для образца с ID {SampleId} и репозитория {RepositoryId}", sampleId, repositoryId);

                var response = await _httpClient.GetAsync($"responsibility?sampleId={sampleId}&repositoryId={repositoryId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Content(content, "application/json");
                }

                _logger.LogWarning("Ошибка при получении обязанностей: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении обязанностей");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, повторите попытку позже.");
            }
        }

        [HttpGet("assignable-responsibles")]
        public async Task<IActionResult> GetAssignableResponsibles()
        {
            try
            {
                _logger.LogInformation("Получение списка доступных ответственных");

                var response = await _httpClient.GetAsync("responsibility/assignable-responsibles");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Content(content, "application/json");
                }

                _logger.LogWarning("Ошибка при получении списка ответственных: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении списка ответственных");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, повторите попытку позже.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteResponsibility(int id)
        {
            try
            {
                _logger.LogInformation("Удаление ответственности с ID {ResponsibilityId}", id);

                var response = await _httpClient.DeleteAsync($"responsibility/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return NoContent();
                }

                _logger.LogWarning("Ошибка при удалении ответственности: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при удалении ответственности");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, повторите попытку позже.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddResponsibility([FromBody] Sample_Responsible newResponsibility)
        {
            try
            {
                _logger.LogInformation("Добавление новой ответственности");

                var content = new StringContent(JsonSerializer.Serialize(newResponsibility), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("responsibility", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return Content(responseBody, "application/json");
                }

                _logger.LogWarning("Ошибка при добавлении ответственности: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при добавлении ответственности");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, повторите попытку позже.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateResponsibility(int id, [FromBody] bool isCurrentResponsible, int repositoryId)
        {
            try
            {
                _logger.LogInformation("Обновление ответственности с ID {ResponsibilityId}", id);

                var updateData = new { isCurrentResponsible, repositoryId };
                var content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"responsibility/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return NoContent();
                }

                _logger.LogWarning("Ошибка при обновлении ответственности: {StatusCode}", response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при обновлении ответственности");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, повторите попытку позже.");
            }
        }
    }
}
