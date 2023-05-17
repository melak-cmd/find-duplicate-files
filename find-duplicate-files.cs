using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Get the directory path from the user
            Console.WriteLine("Enter the directory path:");
            string dirPath = Console.ReadLine();

            // Check if the directory exists
            if (Directory.Exists(dirPath))
            {
                // Create a dictionary to store file names and sizes
                Dictionary<string, long> fileDict = new Dictionary<string, long>();

                // Create a list to store duplicate files
                List<string> dupFiles = new List<string>();

                // Get all the files in the directory and its subdirectories
                string[] files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);

                // Loop through each file
                foreach (string file in files)
                {
                    // Get the file name and size
                    string fileName = Path.GetFileName(file);
                    long fileSize = new FileInfo(file).Length;

                    // Check if the file name and size already exist in the dictionary
                    if (fileDict.ContainsKey(fileName) && fileDict[fileName] == fileSize)
                    {
                        // Add the file to the duplicate list
                        dupFiles.Add(file);
                    }
                    else
                    {
                        // Add the file name and size to the dictionary
                        fileDict.Add(fileName, fileSize);
                    }
                }

                // Check if there are any duplicate files
                if (dupFiles.Count > 0)
                {
                    // Display the duplicate files
                    Console.WriteLine("The following files are duplicates:");
                    foreach (string dupFile in dupFiles)
                    {
                        Console.WriteLine(dupFile);
                    }
                }
                else
                {
                    // Display a message that no duplicate files were found
                    Console.WriteLine("No duplicate files were found.");
                }
            }
            else
            {
                // Display an error message that the directory does not exist
                Console.WriteLine("The directory does not exist.");
            }
        }
        catch (Exception ex)
        {
            // Display an error message that something went wrong
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
