namespace ResponsibilityService.Models
{
    public class Sample_Responsible
    {
        public int Id { get; set; }
        public int SampleId { get; set; }
        public int UserId { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool IsCurrentResponsible { get; set; }

        // Конструктор для создания объекта Sample_Responsible
        public Sample_Responsible(int id, int sampleId, int userId, DateTime assignedAt, bool isCurrentResponsible)
        {
            Id = id;
            SampleId = sampleId;
            UserId = userId;
            AssignedAt = assignedAt;
            IsCurrentResponsible = isCurrentResponsible;
        }
    }
}
