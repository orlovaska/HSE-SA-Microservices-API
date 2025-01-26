using Microsoft.AspNetCore.Mvc;
using ResponsibilityService.Models;
using ResponsibilityService.MockData;

namespace ResponsibilityService.Controllers
{
    [ApiController]
    [Route("samples")]
    public class SamplesController : ControllerBase
    {
        private static List<Sample_Responsible> responsibilities = MockData.MockData.SampleResponsibles;
        private static List<Sample> samples = MockData.MockData.Samples;
        private static List<User> users = MockData.MockData.Users;

        private bool IsValidToken(string token)
        {
            return !string.IsNullOrWhiteSpace(token) && !token.Contains(" ");
        }

        [HttpGet("{currentResponsibleId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователь не найден
        public ActionResult<IEnumerable<Sample>> GetSamplesByResponsibleUser(int currentResponsibleId)
        {
            var token = Request.Cookies["session_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Ошибка авторизации: отсутствует токен сессии.");
            }

            if (!IsValidToken(token))
            {
                return Unauthorized("Ошибка авторизации: неверный токен сессии.");
            }

            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
            {
                return Unauthorized("Ошибка авторизации: отсутствует параметр repositoryId в сессии.");
            }

            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
            if (repositoryId <= 0)
            {
                return Unauthorized("Ошибка авторизации: неверное значение repositoryId.");
            }

            // Проверка существования пользователя
            var user = users.FirstOrDefault(u => u.Id == currentResponsibleId);
            if (user == null)
            {
                return NotFound("Пользователь с указанным ID не найден.");
            }

            // Найти проб, где текущий ответственный пользователь соответствует переданному ID
            var userResponsibilities = responsibilities
                .Where(r => r.UserId == currentResponsibleId)
                .Select(r => r.SampleId)
                .Distinct()
                .ToList();

            var userSamples = samples
                .Where(s => userResponsibilities.Contains(s.Id) && s.RepoId == repositoryId)
                .ToList();

            return Ok(userSamples);
        }
    }
}
