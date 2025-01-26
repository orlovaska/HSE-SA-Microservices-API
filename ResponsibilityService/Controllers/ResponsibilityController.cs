//using Microsoft.AspNetCore.Mvc;
//using ResponsibilityService.Models;
//using ResponsibilityService.MockData;

//namespace ResponsibilityService.Controllers
//{
//    [ApiController]
//    [Route("responsibility")]
//    public class ResponsibilityController : ControllerBase
//    {
//        private static List<Sample_Responsible> responsibilities = MockData.MockData.SampleResponsibles;
//        private static List<User> users = MockData.MockData.Users;
//        private static List<Sample> samples = MockData.MockData.Samples;

//        private bool IsValidToken(string token)
//        {
//            return !string.IsNullOrWhiteSpace(token) && !token.Contains(" ");
//        }

//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // Проба не найдена
//        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Доступ запрещён
//        public ActionResult<IEnumerable<Sample_Responsible>> GetResponsibilitiesForSample(int sampleId)
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("Ошибка авторизации: отсутствует токен сессии.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("Ошибка авторизации: неверный токен сессии.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("Некорректный запрос: отсутствует обязательный параметр repositoryId в сессии.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("Некорректный запрос: неверное значение repositoryId.");
//            }

//            var sample = samples.FirstOrDefault(s => s.Id == sampleId);
//            if (sample == null)
//            {
//                return NotFound("Проба с указанным ID не найдена.");
//            }

//            if (sample.RepoId != repositoryId)
//            {
//                return Forbid("Доступ запрещен: проба относится к другому репозиторию.");
//            }

//            var sampleResponsibilities = responsibilities.Where(r => r.SampleId == sampleId).ToList();

//            return Ok(sampleResponsibilities);
//        }

//        [HttpGet("assignable-responsibles")]
//        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователи не найдены
//        public ActionResult<IEnumerable<User>> GetAssignableResponsibles()
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("Ошибка авторизации: отсутствует токен сессии.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("Ошибка авторизации: неверный токен сессии.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("Некорректный запрос: отсутствует обязательный параметр repositoryId в сессии.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("Некорректный запрос: неверное значение repositoryId.");
//            }

//            if (users == null || !users.Any())
//            {
//                return NotFound("Список возможных ответственных пуст.");
//            }

//            return Ok(users);
//        }

//        [HttpDelete("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)] // Успешное удаление
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // Назначение не найдено
//        public IActionResult DeleteResponsibility(int id)
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("Ошибка авторизации: отсутствует токен сессии.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("Ошибка авторизации: неверный токен сессии.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("Некорректный запрос: отсутствует обязательный параметр repositoryId в сессии.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("Некорректный запрос: неверное значение repositoryId.");
//            }

//            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
//            if (responsibility == null)
//            {
//                return NotFound("Назначение с указанным ID не найдено.");
//            }

//            responsibilities.Remove(responsibility);

//            return NoContent();
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)] // Успешное создание
//        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректные данные
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Ошибка валидации
//        public ActionResult<Sample_Responsible> AddResponsibility([FromBody] Sample_Responsible newResponsibility)
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("Ошибка авторизации: отсутствует токен сессии.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("Ошибка авторизации: неверный токен сессии.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("Некорректный запрос: отсутствует обязательный параметр repositoryId в сессии.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("Некорректный запрос: неверное значение repositoryId.");
//            }

//            newResponsibility.Id = responsibilities.Any() ? responsibilities.Max(r => r.Id) + 1 : 1;

//            responsibilities.Add(newResponsibility);

//            return CreatedAtAction(nameof(AddResponsibility), new { id = newResponsibility.Id }, newResponsibility);
//        }

//        [HttpPut("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)] // Успешное обновление
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Ошибка валидации параметров
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // Назначение не найдено
//        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Доступ запрещен
//        public IActionResult UpdateResponsibility(int id, [FromBody] bool isCurrentResponsible)
//        {
//            // Получение токена из куков
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                // 401 Unauthorized - Отсутствует токен сессии
//                return Unauthorized("Ошибка авторизации: отсутствует токен сессии.");
//            }

//            // Проверка валидности токена
//            if (!IsValidToken(token))
//            {
//                // 401 Unauthorized - Неверный токен сессии
//                return Unauthorized("Ошибка авторизации: неверный токен сессии.");
//            }

//            // Получение repositoryId из сессии
//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                // 422 Unprocessable Entity - RepositoryId отсутствует в сессии
//                return UnprocessableEntity("Некорректный запрос: отсутствует обязательный параметр repositoryId в сессии.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                // 422 Unprocessable Entity - Некорректный repositoryId
//                return UnprocessableEntity("Некорректный запрос: неверное значение repositoryId.");
//            }

//            // Поиск ответственности
//            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
//            if (responsibility == null)
//            {
//                // 404 Not Found - Назначение не найдено
//                return NotFound("Назначение не найдено.");
//            }

//            // Проверка принадлежности ответственности к текущему репозиторию
//            var sample = samples.FirstOrDefault(s => s.Id == responsibility.SampleId);
//            if (sample == null || sample.RepoId != repositoryId)
//            {
//                // 403 Forbidden - Назначение относится к другому репозиторию
//                return Forbid("Доступ запрещен: назначение относится к другому репозиторию.");
//            }

//            // Обновление состояния ответственности
//            responsibility.IsCurrentResponsible = isCurrentResponsible;

//            // 204 No Content - Успешное обновление
//            return NoContent();
//        }

//    }
//}
using Microsoft.AspNetCore.Mvc;
using ResponsibilityService.Models;
using ResponsibilityService.MockData;

namespace ResponsibilityService.Controllers
{
    [ApiController]
    [Route("responsibility")]
    public class ResponsibilityController : ControllerBase
    {
        private static List<Sample_Responsible> responsibilities = MockData.MockData.SampleResponsibles;
        private static List<User> users = MockData.MockData.Users;
        private static List<Sample> samples = MockData.MockData.Samples;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Проба не найдена
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Доступ запрещён
        public ActionResult<IEnumerable<Sample_Responsible>> GetResponsibilitiesForSample(int sampleId, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return UnprocessableEntity("Некорректный запрос: неверное значение repositoryId.");
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
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователи не найдены
        public ActionResult<IEnumerable<User>> GetAssignableResponsibles()
        {
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
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Успешное обновление
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Ошибка валидации параметров
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Назначение не найдено
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Доступ запрещен
        public IActionResult UpdateResponsibility(int id, [FromBody] bool isCurrentResponsible, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return UnprocessableEntity("Некорректный запрос: неверное значение repositoryId.");
            }

            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
            if (responsibility == null)
            {
                return NotFound("Назначение не найдено.");
            }

            var sample = samples.FirstOrDefault(s => s.Id == responsibility.SampleId);
            if (sample == null || sample.RepoId != repositoryId)
            {
                return Forbid("Доступ запрещен: назначение относится к другому репозиторию.");
            }

            responsibility.IsCurrentResponsible = isCurrentResponsible;

            return NoContent();
        }
    }
}
