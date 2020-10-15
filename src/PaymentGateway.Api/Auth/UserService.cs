using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Auth
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
    public class UserService : IUserService
    {
        private static readonly List<User> TestUsers = new List<User>
        {
            new User
            {
                Id = Guid.Parse("441e55d3-f3fb-4773-a955-a12a637a8d0c"),
                UserName = "tombid",
                Password = "test"
            },
            new User
            {
                Id = Guid.Parse("df90bd5e-1b3d-4b93-bb5d-b6133bec354c"),
                UserName = "testuser",
                Password = "test"
            }
        };
        
        public Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }


            return Task.FromResult(
                TestUsers.FirstOrDefault(x => x.UserName == username && x.Password == password));
        }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
    }
}