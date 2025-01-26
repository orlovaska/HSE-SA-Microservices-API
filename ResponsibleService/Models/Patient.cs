namespace ResponsibleService.Models
{
    public class Patient
    {
        public int Id { get; set; }
        private string FirstName { get; set; }
        private string MiddleName { get; set; }
        private string PolicyNumber { get; set; }
        private string Email { get; set; }
        private string Birthdate { get; set; }
        private string Phone { get; set; }
        private bool Gender { get; set; }
        public Guid RepositoryId { get; set; }

        // Конструктор
        public Patient(int id, string firstName, string middleName, string policyNumber, string email, string birthdate, string phone, bool gender, Guid repositoryId)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            PolicyNumber = policyNumber;
            Email = email;
            Birthdate = birthdate;
            Phone = phone;
            Gender = gender;
            RepositoryId = repositoryId;
        }
    }

}
