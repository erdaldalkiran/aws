using System;
using System.Net;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;


namespace StorageSample
{
    class Program
    {
        private const string awsAccessKeyId = "";
        private const string awsSecretAccessKey = "";
        private const string volumeId = "";
        private const string availabilityZone = "";
        private const string instanceId = "";
        private static string snapShotId;
        private static string createdVolumeId;
        private static readonly RegionEndpoint regionEndpoint;

        static Program()
        {
            regionEndpoint = RegionEndpoint.USWest2;
        }

        static void Main()
        {
            Console.WriteLine("To Create Snapshot type y");
            var response = Console.ReadKey();
            Console.WriteLine();

            if (response.KeyChar == 'y')
            {
                CreateSnapShot();
            }

            Console.WriteLine("To Create Volume from snapshot type y");
            response = Console.ReadKey();
            Console.WriteLine();

            if (response.KeyChar == 'y')
            {
                CreateVolumeFromSnapShot();
            }

            Console.WriteLine("To Attach Volume type y");
            response = Console.ReadKey();
            Console.WriteLine();

            if (response.KeyChar == 'y')
            {
                AttachVolume();
            }

            Console.ReadKey();
        }

        private static void AttachVolume()
        {
            var attachVolumeRequest = new AttachVolumeRequest(createdVolumeId, instanceId, "xvdg");
            WithEC2Client(client =>
            {
                var response = client.AttachVolume(attachVolumeRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Volume attached with id: {0}", response.Attachment.VolumeId);
                }
            });
        }

        private static void CreateVolumeFromSnapShot()
        {
            var createVolumeRequest = new CreateVolumeRequest(availabilityZone, snapShotId)
            {
                Size = 1,
                VolumeType = VolumeType.Gp2
            };
            WithEC2Client(client =>
            {
                var response = client.CreateVolume(createVolumeRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Volume created with id: {0}", response.Volume.VolumeId);
                    createdVolumeId = response.Volume.VolumeId;
                }
            });
        }

        private static void CreateSnapShot()
        {
            var snapShotRequest = new CreateSnapshotRequest(volumeId, "Erdal was here!!!");
            WithEC2Client((client) =>
            {
                var response = client.CreateSnapshot(snapShotRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("SnapShot created with id: {0}", response.Snapshot.SnapshotId);
                    snapShotId = response.Snapshot.SnapshotId;
                }
            });
        }

        private static void WithEC2Client(Action<AmazonEC2Client> action)
        {
            using (var ec2Client = new AmazonEC2Client(awsAccessKeyId, awsSecretAccessKey, regionEndpoint))
            {
                action(ec2Client);
            }
        }
    }
}
