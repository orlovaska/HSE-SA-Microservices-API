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
            // ������� ��������� MockData � �������������� ����
            var mockData = new MockData();
            this.responsibilities = mockData.SampleResponsibles;
            this.users = mockData.Users;
            this.samples = mockData.Samples;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // �������� �����
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // ������������ ������
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // ������������ ������
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // ������ ��������
        public ActionResult<IEnumerable<Sample_Responsible>> GetResponsibilitiesForSample(int sampleId, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("������������ ������: �������� �������� repositoryId.");
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // ������������ ������
        public ActionResult<IEnumerable<User>> GetAssignableResponsibles(int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("������������ ������: �������� �������� repositoryId.");
            }
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
        [ProducesResponseType(StatusCodes.Status200OK)] // �������� ���������� � ������������ �������
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // ������ ��������� ����������
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // ������ ��������
        public IActionResult UpdateResponsibility([FromBody] Sample_Responsible newResponsibility, int repositoryId)
        {
            if (repositoryId <= 0)
            {
                return BadRequest("������������ ������: �������� �������� repositoryId.");
            }

            var responsibility = responsibilities.FirstOrDefault(r => r.Id == newResponsibility.Id);
            if (responsibility == null)
            {
                return NotFound("���������� �� �������.");
            }

            var sample = samples.FirstOrDefault(s => s.Id == responsibility.SampleId);
            if (sample == null || sample.RepoId != repositoryId)
            {
                return Forbid("������ ��������: ���������� ��������� � ������� �����������.");
            }

            responsibility.IsCurrentResponsible = newResponsibility.IsCurrentResponsible;

            // ���������� ����������� ������ � ���� ������
            return Ok(responsibility);
        }

    }
}
