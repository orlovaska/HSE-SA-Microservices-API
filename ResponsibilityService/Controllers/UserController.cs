using Microsoft.AspNetCore.Mvc;
using ResponsibilityService.Models;
using ResponsibilityService.MockData;

namespace ResponsibilityService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private static List<User> users = MockData.MockData.Users;

        private bool IsValidToken(string token)
        {
            return !string.IsNullOrWhiteSpace(token) && !token.Contains(" ");
        }

        [HttpGet("repo/{repoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Некорректный запрос
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователи не найдены
        public ActionResult<IEnumerable<User>> GetUsersByRepoId(int repoId)
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

            if (repoId <= 0)
            {
                return UnprocessableEntity("Некорректный запрос: неверное значение repoId.");
            }

            var usersByRepo = users.Where(u => u.RepoId == repoId).ToList();

            if (!usersByRepo.Any())
            {
                return NotFound("Пользователи с указанным RepoId не найдены.");
            }

            return Ok(usersByRepo);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователь не найден
        public ActionResult<User> GetUserById(int id)
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

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"Пользователь с ID {id} не найден.");
            }

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // Успешное создание
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректные данные
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
        public ActionResult<User> CreateUser([FromBody] User newUser)
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

            if (newUser == null || string.IsNullOrWhiteSpace(newUser.Name))
            {
                return BadRequest("Некорректные данные для создания пользователя.");
            }

            newUser.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(newUser);

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Успешное удаление
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователь не найден
        public IActionResult DeleteUser(int id)
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

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"Пользователь с ID {id} не найден.");
            }

            users.Remove(user);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Успешное обновление
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Ошибка авторизации
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректные данные
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователь не найден
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
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

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"Пользователь с ID {id} не найден.");
            }

            if (updatedUser == null || string.IsNullOrWhiteSpace(updatedUser.Name))
            {
                return BadRequest("Некорректные данные для обновления пользователя.");
            }

            user.Name = updatedUser.Name;
            user.Roles = updatedUser.Roles;

            return NoContent();
        }
    }
}
