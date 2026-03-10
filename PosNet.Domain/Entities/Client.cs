
namespace PosNet.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string Surname { get; set; }
        public string? SecondSurname { get; set; }
        public required int PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? ImageUrl { get; set; }


        public Client(
            string firstName, 
            string surname, 
            int phoneNumber,
            string email,
            string? middleName = null, 
            string? secondSurname = null, 
            string? imageUrl =null)
        {
            FirstName = firstName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            Email = email;
            Surname = surname;
            SecondSurname = secondSurname;
            ImageUrl = imageUrl;
        }
        public Client() { }

    }
}
