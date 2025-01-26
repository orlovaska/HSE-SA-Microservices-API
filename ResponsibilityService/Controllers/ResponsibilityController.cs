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
//        [ProducesResponseType(StatusCodes.Status200OK)] // �������� �����
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // ������ �����������
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������������ ������
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // ����� �� �������
//        [ProducesResponseType(StatusCodes.Status403Forbidden)] // ������ ��������
//        public ActionResult<IEnumerable<Sample_Responsible>> GetResponsibilitiesForSample(int sampleId)
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("������ �����������: ����������� ����� ������.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("������ �����������: �������� ����� ������.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("������������ ������: ����������� ������������ �������� repositoryId � ������.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("������������ ������: �������� �������� repositoryId.");
//            }

//            var sample = samples.FirstOrDefault(s => s.Id == sampleId);
//            if (sample == null)
//            {
//                return NotFound("����� � ��������� ID �� �������.");
//            }

//            if (sample.RepoId != repositoryId)
//            {
//                return Forbid("������ ��������: ����� ��������� � ������� �����������.");
//            }

//            var sampleResponsibilities = responsibilities.Where(r => r.SampleId == sampleId).ToList();

//            return Ok(sampleResponsibilities);
//        }

//        [HttpGet("assignable-responsibles")]
//        [ProducesResponseType(StatusCodes.Status200OK)] // �������� �����
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // ������ �����������
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������������ ������
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // ������������ �� �������
//        public ActionResult<IEnumerable<User>> GetAssignableResponsibles()
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("������ �����������: ����������� ����� ������.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("������ �����������: �������� ����� ������.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("������������ ������: ����������� ������������ �������� repositoryId � ������.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("������������ ������: �������� �������� repositoryId.");
//            }

//            if (users == null || !users.Any())
//            {
//                return NotFound("������ ��������� ������������� ����.");
//            }

//            return Ok(users);
//        }

//        [HttpDelete("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)] // �������� ��������
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // ������ �����������
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������������ ������
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // ���������� �� �������
//        public IActionResult DeleteResponsibility(int id)
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("������ �����������: ����������� ����� ������.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("������ �����������: �������� ����� ������.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("������������ ������: ����������� ������������ �������� repositoryId � ������.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("������������ ������: �������� �������� repositoryId.");
//            }

//            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
//            if (responsibility == null)
//            {
//                return NotFound("���������� � ��������� ID �� �������.");
//            }

//            responsibilities.Remove(responsibility);

//            return NoContent();
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)] // �������� ��������
//        [ProducesResponseType(StatusCodes.Status400BadRequest)] // ������������ ������
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // ������ �����������
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������ ���������
//        public ActionResult<Sample_Responsible> AddResponsibility([FromBody] Sample_Responsible newResponsibility)
//        {
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                return Unauthorized("������ �����������: ����������� ����� ������.");
//            }

//            if (!IsValidToken(token))
//            {
//                return Unauthorized("������ �����������: �������� ����� ������.");
//            }

//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                return UnprocessableEntity("������������ ������: ����������� ������������ �������� repositoryId � ������.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                return UnprocessableEntity("������������ ������: �������� �������� repositoryId.");
//            }

//            newResponsibility.Id = responsibilities.Any() ? responsibilities.Max(r => r.Id) + 1 : 1;

//            responsibilities.Add(newResponsibility);

//            return CreatedAtAction(nameof(AddResponsibility), new { id = newResponsibility.Id }, newResponsibility);
//        }

//        [HttpPut("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)] // �������� ����������
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // ������ �����������
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������ ��������� ����������
//        [ProducesResponseType(StatusCodes.Status404NotFound)] // ���������� �� �������
//        [ProducesResponseType(StatusCodes.Status403Forbidden)] // ������ ��������
//        public IActionResult UpdateResponsibility(int id, [FromBody] bool isCurrentResponsible)
//        {
//            // ��������� ������ �� �����
//            var token = Request.Cookies["session_token"];
//            if (string.IsNullOrEmpty(token))
//            {
//                // 401 Unauthorized - ����������� ����� ������
//                return Unauthorized("������ �����������: ����������� ����� ������.");
//            }

//            // �������� ���������� ������
//            if (!IsValidToken(token))
//            {
//                // 401 Unauthorized - �������� ����� ������
//                return Unauthorized("������ �����������: �������� ����� ������.");
//            }

//            // ��������� repositoryId �� ������
//            if (!HttpContext.Session.TryGetValue("repositoryId", out var repositoryIdBytes))
//            {
//                // 422 Unprocessable Entity - RepositoryId ����������� � ������
//                return UnprocessableEntity("������������ ������: ����������� ������������ �������� repositoryId � ������.");
//            }

//            int repositoryId = BitConverter.ToInt32(repositoryIdBytes);
//            if (repositoryId <= 0)
//            {
//                // 422 Unprocessable Entity - ������������ repositoryId
//                return UnprocessableEntity("������������ ������: �������� �������� repositoryId.");
//            }

//            // ����� ���������������
//            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
//            if (responsibility == null)
//            {
//                // 404 Not Found - ���������� �� �������
//                return NotFound("���������� �� �������.");
//            }

//            // �������� �������������� ��������������� � �������� �����������
//            var sample = samples.FirstOrDefault(s => s.Id == responsibility.SampleId);
//            if (sample == null || sample.RepoId != repositoryId)
//            {
//                // 403 Forbidden - ���������� ��������� � ������� �����������
//                return Forbid("������ ��������: ���������� ��������� � ������� �����������.");
//            }

//            // ���������� ��������� ���������������
//            responsibility.IsCurrentResponsible = isCurrentResponsible;

//            // 204 No Content - �������� ����������
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
        [ProducesResponseType(StatusCodes.Status200OK)] // �������� �����
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������������ ������
        [ProducesResponseType(StatusCodes.Status404NotFound)] // ����� �� �������
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // ������ ��������
        public ActionResult<IEnumerable<Sample_Responsible>> GetResponsibilitiesForSample(int sampleId, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return UnprocessableEntity("������������ ������: �������� �������� repositoryId.");
            }

            var sample = samples.FirstOrDefault(s => s.Id == sampleId);
            if (sample == null)
            {
                return NotFound("����� � ��������� ID �� �������.");
            }

            if (sample.RepoId != repositoryId)
            {
                return Forbid("������ ��������: ����� ��������� � ������� �����������.");
            }

            var sampleResponsibilities = responsibilities.Where(r => r.SampleId == sampleId).ToList();

            return Ok(sampleResponsibilities);
        }

        [HttpGet("assignable-responsibles")]
        [ProducesResponseType(StatusCodes.Status200OK)] // �������� �����
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������������ ������
        [ProducesResponseType(StatusCodes.Status404NotFound)] // ������������ �� �������
        public ActionResult<IEnumerable<User>> GetAssignableResponsibles()
        {
            if (users == null || !users.Any())
            {
                return NotFound("������ ��������� ������������� ����.");
            }

            return Ok(users);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // �������� ��������
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������������ ������
        [ProducesResponseType(StatusCodes.Status404NotFound)] // ���������� �� �������
        public IActionResult DeleteResponsibility(int id)
        {
            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
            if (responsibility == null)
            {
                return NotFound("���������� � ��������� ID �� �������.");
            }

            responsibilities.Remove(responsibility);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // �������� ��������
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // ������������ ������
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������ ���������
        public ActionResult<Sample_Responsible> AddResponsibility([FromBody] Sample_Responsible newResponsibility)
        {
            newResponsibility.Id = responsibilities.Any() ? responsibilities.Max(r => r.Id) + 1 : 1;

            responsibilities.Add(newResponsibility);

            return CreatedAtAction(nameof(AddResponsibility), new { id = newResponsibility.Id }, newResponsibility);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // �������� ����������
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������ ��������� ����������
        [ProducesResponseType(StatusCodes.Status404NotFound)] // ���������� �� �������
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // ������ ��������
        public IActionResult UpdateResponsibility(int id, [FromBody] bool isCurrentResponsible, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return UnprocessableEntity("������������ ������: �������� �������� repositoryId.");
            }

            var responsibility = responsibilities.FirstOrDefault(r => r.Id == id);
            if (responsibility == null)
            {
                return NotFound("���������� �� �������.");
            }

            var sample = samples.FirstOrDefault(s => s.Id == responsibility.SampleId);
            if (sample == null || sample.RepoId != repositoryId)
            {
                return Forbid("������ ��������: ���������� ��������� � ������� �����������.");
            }

            responsibility.IsCurrentResponsible = isCurrentResponsible;

            return NoContent();
        }
    }
}
