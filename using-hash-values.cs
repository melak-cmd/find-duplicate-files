using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        // Get the directory path from the user
        Console.WriteLine("Enter the directory path:");
        string dirPath = Console.ReadLine();

        // Check if the directory exists
        if (Directory.Exists(dirPath))
        {
            // Create a dictionary to store image names and hash values
            Dictionary<string, string> imageDict = new Dictionary<string, string>();

            // Create a list to store duplicate images
            List<string> dupImages = new List<string>();

            // Get all the images in the directory and its subdirectories
            string[] images = Directory.GetFiles(dirPath, "*.jpg", SearchOption.AllDirectories);

            // Create a variable to store the current progress
            int progress = 0;

            // Create a variable to store the total number of images
            int total = images.Length;

            // Display the initial progress bar
            Console.WriteLine("Searching for duplicate images...");
            DrawProgressBar(progress, total);

            // Loop through each image
            foreach (string image in images)
            {
                // Get the image name
                string imageName = Path.GetFileName(image);

                // Get the hash value of the image
                string hashValue = GetHashValue(image);

                // Check if the image name and hash value already exist in the dictionary
                if (imageDict.ContainsKey(imageName) && imageDict[imageName] == hashValue)
                {
                    // Add the image to the duplicate list
                    dupImages.Add(image);
                }
                else
                {
                    // Add the image name and hash value to the dictionary
                    imageDict.Add(imageName, hashValue);
                }

                // Increment the progress by one
                progress++;

                // Update the progress bar
                DrawProgressBar(progress, total);
            }

            // Check if there are any duplicate images
            if (dupImages.Count > 0)
            {
                // Display the duplicate images
                Console.WriteLine("The following images are duplicates:");
                foreach (string dupImage in dupImages)
                {
                    Console.WriteLine(dupImage);
                }
            }
            else
            {
                // Display a message that no duplicate images were found
                Console.WriteLine("No duplicate images were found.");
            }
        }
        else
        {
            // Display an error message that the directory does not exist
            Console.WriteLine("The directory does not exist.");
        }
    }

    // A method to get the hash value of an image file
    static string GetHashValue(string file)
    {
        // Create a MD5 hash algorithm object
        using (var md5 = MD5.Create())
        {
            // Open the file as a read-only stream
            using (var stream = File.OpenRead(file))
            {
                // Compute the hash value of the stream
                var hash = md5.ComputeHash(stream);

                // Convert the hash value to a hexadecimal string
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }

    // A method to draw a progress bar on the console
    static void DrawProgressBar(int progress, int total)
    {
        // Calculate the percentage of completion
        double percent = (double)progress / total;

        // Calculate how many blocks to display on the bar
        int blocks = (int)Math.Round(percent * 50);

        // Create a string builder to store the bar characters
        var bar = new System.Text.StringBuilder();

        // Add an opening bracket to the bar string
        bar.Append("[");

        // Loop through each block on the bar
        for (int i = 0; i < 50; i++)
        {
            // If the block is less than or equal to the number of blocks to display, add a filled block character
            if (i < blocks)
            {
                bar.Append("█");
            }
            // Otherwise, add an empty block character
            else
            {
                bar.Append(" ");
            }
        }

        // Add a closing bracket to the bar string
        bar.Append("]");

        // Add a space and a percentage label to the bar string
        bar.Append($" {percent:P0}");

        // Move the cursor to the beginning of the current line on the console
        Console.CursorLeft = 0;

        // Write the bar string on the console without a new line character
        Console.Write(bar.ToString());

        // If the progress is equal to the total, move the cursor to a new line on the console 
        if (progress == total)
        {
            Console.WriteLine();
        }
    }
}
