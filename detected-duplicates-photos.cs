using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

class Program
{
    // Summary:
    //     The main entry point of the program.
    // Parameters:
    //   args: The command-line arguments.
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
                // Create a dictionary to store image names and average colors
                Dictionary<string, Color> imageDict = new Dictionary<string, Color>();

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

                    // Load the image as a bitmap
                    Bitmap bmp = new Bitmap(image);

                    // Get the average color of the image
                    Color avgColor = GetAverageColor(bmp);

                    // Check if the image name and average color already exist in the dictionary
                    if (imageDict.ContainsKey(imageName) && imageDict[imageName] == avgColor)
                    {
                        // Add the image to the duplicate list
                        dupImages.Add(image);
                    }
                    else
                    {
                        // Add the image name and average color to the dictionary
                        imageDict.Add(imageName, avgColor);
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
        catch (Exception ex)
        {
            // Display an error message that something went wrong
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    // Summary:
    //     A method to get the average color of a bitmap image.
    // Parameters:
    //   bmp: The bitmap image to process.
    // Returns:
    //   A color object representing the average color of the bitmap.
    static Color GetAverageColor(Bitmap bmp)
    {
        // Initialize variables to store the sum of RGB values
        int r = 0;
        int g = 0;
        int b = 0;

        // Loop through each pixel of the bitmap
        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                // Get the color of the pixel
                Color pixel = bmp.GetPixel(x, y);

                // Add the RGB values to the sum variables
                r += pixel.R;
                g += pixel.G;
                b += pixel.B;
            }
        }

        // Calculate the total number of pixels
        int totalPixels = bmp.Width * bmp.Height;

        // Calculate the average RGB values by dividing by the total number of pixels
        r /= totalPixels;
        g /= totalPixels;
        b /= totalPixels;

        // Return a new color with the average RGB values
        return Color.FromArgb(r, g, b);
    }

    // Summary:
    //     A method to draw a progress bar on the console.
    // Parameters:
    //   progress: The current progress value.
    //   total: The total progress value.
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
