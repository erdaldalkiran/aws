using System;
using System.Net;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;


namespace CreateUserSample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Please enter the user name you want to create!");
            var userName = Console.ReadLine();
            CreateUser(userName);
            Console.ReadKey();
        }

        private static void CreateUser(string userName)
        {
            CreateUserResponse response;
            using (var client = new AmazonIdentityManagementServiceClient())
            {
                var createUserRequest = new CreateUserRequest(userName);
                response = client.CreateUser(createUserRequest);
            }

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("{0} is created", userName);
            }
        }
    }
}
