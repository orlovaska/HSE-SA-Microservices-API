namespace GatewayService.Models
{
    public class Sample
    {
        public int Id { get; set; }
        public int RepoId { get; set; }
        public SampleType Type { get; set; }
        public Sample(int id, int repoId, SampleType type)
        {
            Id = id;
            RepoId = repoId;
            Type = type;
        }
    }

    public enum SampleType
    {
        OAK,   // Общий анализ крови
        GRAM,  // Грам-пятно
        PAP,   // ПАП-тест
        KM,    // КМ (Костный мозг)
        CUV    // ЦУВ (Цитологическое исследование)
    }
}
