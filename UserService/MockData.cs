namespace UserService
{
    using UserService.Models;
    using System.Collections.Generic;

    namespace DataMock
    {
        public class MockData
        {
            public List<Privilege> Privileges { get; private set; }
            public List<Role> Roles { get; private set; }
            public List<Repository> Repositories { get; private set; }
            public List<User> Users { get; private set; }
            public List<Sample> Samples { get; private set; }
            public List<Sample_Responsible> SampleResponsibles { get; private set; }

            public MockData()
            {
                Privileges = new List<Privilege>
                {
                    new Privilege(1, "CanReadResponsible"),
                    new Privilege(2, "CanWriteResponsible"),
                    new Privilege(3, "CanDeleteResponsible"),
                    new Privilege(4, "CanWriteSamples"),
                    new Privilege(5, "CanViewSampleReports"),
                    new Privilege(6, "CanAddUsers"),
                    new Privilege(7, "CanDeleteUsers")
                };

                Roles = new List<Role>
                {
                    new Role(1, "Admin", new Privilege[] { Privileges[0], Privileges[1], Privileges[2], Privileges[3], Privileges[4], Privileges[5], Privileges[6] }),
                    new Role(2, "Editor", new Privilege[] { Privileges[0], Privileges[1], Privileges[4], Privileges[5] }),
                    new Role(3, "Viewer", new Privilege[] { Privileges[0], Privileges[4] }),
                    new Role(4, "Scheduler", new Privilege[] { Privileges[0], Privileges[6] }),
                    new Role(5, "ResponsibleAll", new Privilege[] { Privileges[0], Privileges[1], Privileges[2] })
                };

                Repositories = new List<Repository>
                {
                    new Repository(1, "KDLFirstRepository", 100, "FirstRepo"),
                    new Repository(2, "KDLSecondRepository", 50, "SecondRepo")
                };

                Users = new List<User>
                {
                    new User(1, "admin", "John", "Doe", "hashed_password_admin", new Role[] { Roles[0] }, 1),
                    new User(2, "editor1", "Alice", "Smith", "hashed_password_editor1", new Role[] { Roles[1] }, 1),
                    new User(3, "editor2", "Bob", "Johnson", "hashed_password_editor2", new Role[] { Roles[1] }, 1),
                    new User(4, "viewer1", "Carol", "Brown", "hashed_password_viewer1", new Role[] { Roles[2] }, 1),
                    new User(5, "viewer2", "David", "Davis", "hashed_password_viewer2", new Role[] { Roles[2] }, 1),
                    new User(6, "scheduler1", "Eve", "Miller", "hashed_password_scheduler1", new Role[] { Roles[3] }, 1),
                    new User(7, "scheduler2", "Frank", "Wilson", "hashed_password_scheduler2", new Role[] { Roles[3] }, 1),
                    new User(8, "user1", "Grace", "Taylor", "hashed_password_user1", new Role[] { Roles[1], Roles[2] }, 1),
                    new User(9, "user2", "Hank", "Anderson", "hashed_password_user2", new Role[] { Roles[2], Roles[3] }, 1),
                    new User(10, "user3", "Ivy", "Thomas", "hashed_password_user3", new Role[] { Roles[1] }, 1),
                    new User(11, "user4", "Jack", "Moore", "hashed_password_user4", new Role[] { Roles[2] }, 1),
                    new User(12, "admin1", "Kate", "White", "hashed_password_admin1", new Role[] { Roles[0] }, 1),
                    new User(13, "admin2", "Leo", "Hall", "hashed_password_admin2", new Role[] { Roles[0] }, 1),
                    new User(14, "editor3", "Mia", "Young", "hashed_password_editor3", new Role[] { Roles[1] }, 1),
                    new User(15, "editor4", "Nick", "King", "hashed_password_editor4", new Role[] { Roles[1] }, 1),
                    new User(16, "viewer3", "Olivia", "Wright", "hashed_password_viewer3", new Role[] { Roles[2] }, 1),
                    new User(17, "viewer4", "Paul", "Scott", "hashed_password_viewer4", new Role[] { Roles[2] }, 1),
                    new User(18, "scheduler3", "Quinn", "Green", "hashed_password_scheduler3", new Role[] { Roles[3] }, 1),
                    new User(19, "scheduler4", "Rose", "Adams", "hashed_password_scheduler4", new Role[] { Roles[3] }, 1),
                    new User(20, "user5", "Sam", "Baker", "hashed_password_user5", new Role[] { Roles[4] }, 1)
                };

                Samples = new List<Sample>
                {
                    new Sample(1, 1, SampleType.OAK),
                    new Sample(2, 1, SampleType.GRAM),
                    new Sample(3, 2, SampleType.PAP),
                    new Sample(4, 2, SampleType.KM),
                    new Sample(5, 1, SampleType.CUV)
                };

                SampleResponsibles = new List<Sample_Responsible>
                {
                    new Sample_Responsible(1, 1, 1, DateTime.Now.AddDays(-10), true),
                    new Sample_Responsible(2, 2, 3, DateTime.Now.AddDays(-20), false),
                    new Sample_Responsible(3, 3, 5, DateTime.Now.AddDays(-15), true),
                    new Sample_Responsible(4, 4, 7, DateTime.Now.AddDays(-5), true),
                    new Sample_Responsible(5, 5, 9, DateTime.Now.AddDays(-30), false),
                    new Sample_Responsible(6, 1, 2, DateTime.Now.AddDays(-25), false),
                    new Sample_Responsible(7, 3, 10, DateTime.Now.AddDays(-2), true),
                    new Sample_Responsible(8, 5, 8, DateTime.Now.AddDays(-7), true),
                    new Sample_Responsible(9, 2, 12, DateTime.Now.AddDays(-3), true),
                    new Sample_Responsible(10, 4, 15, DateTime.Now.AddDays(-1), true)
                };
            }
        }
    }
}

