using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UploadS3
{
    public class Sender
    {
        private static IAmazonS3 clientS3;

        private string awsBucketName = "<your-bucket-name>";

        public Sender()
        {
            // -----------------------------------------------------------------------vvvvvvvvvvvvvvvvvvvvvv-- <your-region>
            clientS3 = new AmazonS3Client("<your-access-key>", "<your-security-key>", RegionEndpoint.USEast1);
        }



        public void SendFiles()
        {

            string localBasePath = @"C:\<your-folder>";

            var lstFiles = Directory.GetFiles(localBasePath, "*", SearchOption.AllDirectories).ToList();

            List<string> lstFaults = new List<string>();

            do
            {
                Console.WriteLine("\n\nList processing...\n\n");
                long cont = 0;
                Console.WriteLine($"Total: {lstFiles.Count():N0}");
                Console.Write($"Files:");
                lstFiles.AsParallel().ForAll(item =>
                {
                    if (!WriteFileOnBucket(item, item.Replace(localBasePath, "").Replace(@"\", @"/")))
                    {
                        lstFaults.Add(item);
                    }
                    Console.CursorLeft = 15;
                    Console.Write($"{cont++:N0}                  ");
                });

                Console.WriteLine("\n\n\nFailures...\n");
                Console.WriteLine($"QUantity: {lstFaults.Count()}");
                lstFiles.Clear();
                lstFiles.AddRange(lstFaults);

            } while (lstFiles.Count() > 0);


        }


        public bool WriteFileOnBucket(string origFileName, string destFileName)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = awsBucketName,
                Key = destFileName,
                // -----------vvvvvvvvvvvvvvvvv--- Especify <your-content-type> or comment this line, in my case, the files are xml type
                ContentType = "application/xml",
                FilePath = origFileName
            };
            PutObjectResponse response = clientS3.PutObjectAsync(request).Result;
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
