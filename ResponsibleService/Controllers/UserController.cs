using Microsoft.AspNetCore.Mvc;
using ResponsibleService.Models;
using ResponsibleService.DataMock;

namespace ResponsibleService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly MockData mockData;
        private readonly List<User> users;

        public UserController()
        {
            this.mockData = new MockData();
            this.users = mockData.Users;
        }

        [HttpGet("by-repository")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректный запрос
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователи не найдены
        public ActionResult<IEnumerable<User>> GetUsersByRepoId(int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("Некорректный запрос: неверное значение repoId.");
            }

            var usersByRepo = users.Where(u => u.RepoId == repositoryId).ToList();

            if (!usersByRepo.Any())
            {
                return NotFound("Пользователи с указанным RepoId не найдены.");
            }

            return Ok(usersByRepo);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователь не найден
        public ActionResult<User> GetUserById(int id)
        {
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
        public ActionResult<User> AddUser([FromBody] User newUser)
        {
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
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователь не найден
        public IActionResult DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"Пользователь с ID {id} не найден.");
            }

            users.Remove(user);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Успешное обновление
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Некорректные данные
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Пользователь не найден
        public IActionResult UpdateUser([FromBody] User updatedUser, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("Некорректный запрос: неверное значение repositoryId.");
            }

            var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null)
            {
                return NotFound($"Пользователь с ID {updatedUser.Id} не найден.");
            }

            if (updatedUser == null || string.IsNullOrWhiteSpace(updatedUser.Name))
            {
                return BadRequest("Некорректные данные для обновления пользователя.");
            }

            user.Name = updatedUser.Name;
            user.Roles = updatedUser.Roles;

            return Ok(user);
        }
    }
}
