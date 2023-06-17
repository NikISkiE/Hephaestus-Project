using System.ComponentModel;

namespace Hephaestus_Project.Interface
{
    public interface IDatabase
    {
        public interface IUserInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Division { get; set; }
            public string Rank { get; set; }
            public int IsRegistered { get; set; }
        }

        public interface IAccountData
        {
            public string Id { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string PermLVL { get; set; }
            public string Created_At { get; set; }
            public string UserID { get; set; }
        }

        public interface IDivisionInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Division { get; set; }
            public string Rank { get; set; }
        }

        public interface IMyEQinfo
        {
            public string Serial { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public bool InMaintance { get; set; }
        }
    }
}
