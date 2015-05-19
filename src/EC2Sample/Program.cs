using System;
using System.Collections.Generic;
using System.Net;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Tag = Amazon.EC2.Model.Tag;


namespace EC2Sample
{
    class Program
    {
        private static readonly RegionEndpoint regionEndpoint;
        private const string InstanceId = "";
        private const string AwsAccessKeyId = "";
        private const string AwsSecretAccessKey = "";

        static void Main()
        {
            AddTag();
            TerminateInstance();
            Console.ReadKey();
        }

        private static void TerminateInstance()
        {
            Console.WriteLine("To terminate instance type y");
            var key = Console.ReadKey();
            if (key.KeyChar == 'y')
            {
                WithEC2Client(client =>
                {
                    var terminateInstanceRequest = new TerminateInstancesRequest(new List<string> { InstanceId });
                    var response = client.TerminateInstances(terminateInstanceRequest);
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Instance terminated!");
                    }
                });
            }
        }

        private static void AddTag()
        {
            Console.WriteLine("Please enter the tag you want to assign the instance");
            var tag = Console.ReadLine();

            Console.WriteLine("Please enter the value of tag you ");
            var value = Console.ReadLine();

            WithEC2Client(ec2Client =>
            {
                var createTagRequest = new CreateTagsRequest(new List<string> { InstanceId }, new List<Tag> { new Tag(tag, value) });
                var response = ec2Client.CreateTags(createTagRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Tag is created!");
                }
            });
        }

        private static void WithEC2Client(Action<AmazonEC2Client> action)
        {
            using (var client = new AmazonEC2Client(AwsAccessKeyId, AwsSecretAccessKey, regionEndpoint))
            {
                action(client);
            }
        }
    }
}
