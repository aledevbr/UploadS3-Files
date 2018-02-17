using System;

namespace UploadS3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...\n\n");

            new Sender().SendFiles();

            Console.WriteLine("\n\nFinished...\n\n\n[ENTER]");
            Console.ReadLine();
        }
    }
}
