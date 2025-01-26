using ResponsibleService.DataMock;
using Microsoft.AspNetCore.Mvc;
using ResponsibleService.Models;

namespace ResponsibilityService.Controllers
{
    [ApiController]
    [Route("samples")]
    public class SamplesController : ControllerBase
    {
        private readonly List<Sample_Responsible> responsibilities;
        private readonly List<User> users;
        private readonly List<Sample> samples;

        public SamplesController()
        {
            // Создаем экземпляр MockData и инициализируем поля
            var mockData = new MockData();
            this.responsibilities = mockData.SampleResponsibles;
            this.users = mockData.Users;
            this.samples = mockData.Samples;
        }

        [HttpGet("by-responsible-user")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Sample>> GetSamplesByResponsibleUser([FromQuery] int currentResponsibleId)
        {
            // Проверка существования пользователя
            var user = users.FirstOrDefault(u => u.Id == currentResponsibleId);
            if (user == null)
            {
                return BadRequest("Пользователь с указанным ID не найден.");
            }

            // Найти проб, где текущий ответственный пользователь соответствует переданному ID
            var userResponsibilities = responsibilities
                .Where(r => r.UserId == currentResponsibleId)
                .Select(r => r.SampleId)
                .Distinct()
                .ToList();

            var userSamples = samples
                .Where(s => userResponsibilities.Contains(s.Id))
                .ToList();

            return Ok(userSamples);
        }

    }
}
