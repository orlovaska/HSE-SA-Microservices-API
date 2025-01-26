using Microsoft.AspNetCore.Mvc;
using ResponsibleService.DataMock;
using ResponsibleService.Models;

namespace ResponsibilityService.Controllers
{
    [ApiController]
    [Route("responsibility")]
    public class ResponsibilityController : ControllerBase
    {
        private readonly List<Sample_Responsible> responsibilities;
        private readonly List<User> users;
        private readonly List<Sample> samples;

        public ResponsibilityController()
        {
            // Создаем экземпляр MockData и инициализируем поля
            var mockData = new MockData();
            this.responsibilities = mockData.SampleResponsibles;
            this.users = mockData.Users;
            this.samples = mockData.Samples;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректные данные
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Доступ запрещён
        public ActionResult<IEnumerable<Sample_Responsible>> GetResponsibilitiesForSample(int sampleId, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("Некорректный запрос: неверное значение repositoryId.");
            }

            var sample = samples.FirstOrDefault(s => s.Id == sampleId);
            if (sample == null)
            {
                return NotFound("Проба с указанным ID не найдена.");
            }

            if (sample.RepoId != repositoryId)
            {
                return Forbid("Доступ запрещен: проба относится к другому репозиторию.");
            }

            var sampleResponsibilities = responsibilities.Where(r => r.SampleId == sampleId).ToList();

            return Ok(sampleResponsibilities);
        }

        [HttpGet("assignable-responsibles")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректный запрос
        public ActionResult<IEnumerable<User>> GetAssignableResponsibles(int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("Некорректный запрос: неверное значение repositoryId.");
            }
            if (users == null || !users.Any())
            {
                return NotFound("Список возможных ответственных пуст.");
            }

            return Ok(users);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Успешное удаление
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Назначение не найдено
        public IActionResult DeleteResponsibility(int id)
        {
            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
            if (responsibility == null)
            {
                return NotFound("Назначение с указанным ID не найдено.");
            }

            responsibilities.Remove(responsibility);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // Успешное создание
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректные данные
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Ошибка валидации
        public ActionResult<Sample_Responsible> AddResponsibility([FromBody] Sample_Responsible newResponsibility)
        {
            newResponsibility.Id = responsibilities.Any() ? responsibilities.Max(r => r.Id) + 1 : 1;

            responsibilities.Add(newResponsibility);

            return CreatedAtAction(nameof(AddResponsibility), new { id = newResponsibility.Id }, newResponsibility);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешное обновление с возвращением объекта
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Ошибка валидации параметров
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Доступ запрещен
        public IActionResult UpdateResponsibility([FromBody] Sample_Responsible newResponsibility, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("Некорректный запрос: неверное значение repositoryId.");
            }

            var responsibility = responsibilities.FirstOrDefault(r => r.Id == newResponsibility.Id);
            if (responsibility == null)
            {
                return NotFound("Назначение не найдено.");
            }

            var sample = samples.FirstOrDefault(s => s.Id == responsibility.SampleId);
            if (sample == null || sample.RepoId != repositoryId)
            {
                return Forbid("Доступ запрещен: назначение относится к другому репозиторию.");
            }

            responsibility.IsCurrentResponsible = newResponsibility.IsCurrentResponsible;

            // Возвращаем обновленный объект в теле ответа
            return Ok(responsibility);
        }

    }
}
