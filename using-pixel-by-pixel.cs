using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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
            // Create a dictionary to store image names and pixel arrays
            Dictionary<string, Color[,]> imageDict = new Dictionary<string, Color[,]>();

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

                // Resize the image to 16x16 pixels
                bmp = ResizeImage(bmp, 16, 16);

                // Get the pixel array of the image
                Color[,] pixels = GetPixelArray(bmp);

                // Check if the image name already exists in the dictionary
                if (imageDict.ContainsKey(imageName))
                {
                    // Get the pixel array of the existing image
                    Color[,] existingPixels = imageDict[imageName];

                    // Compare the pixel arrays of the two images
                    double deviation = ComparePixelArrays(pixels, existingPixels);

                    // If the deviation is less than a threshold, consider them as duplicates
                    if (deviation < 10)
                    {
                        // Add the image to the duplicate list
                        dupImages.Add(image);
                    }
                }
                else
                {
                    // Add the image name and pixel array to the dictionary
                    imageDict.Add(imageName, pixels);
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

    // A method to resize an image to a given width and height
    static Bitmap ResizeImage(Bitmap bmp, int width, int height)
    {
        // Create a new bitmap with the given dimensions
        Bitmap resizedBmp = new Bitmap(width, height);

        // Create a graphics object from the new bitmap
        using (Graphics g = Graphics.FromImage(resizedBmp))
        {
            // Draw the original bitmap on the new bitmap with scaling and interpolation mode
            g.DrawImage(bmp, 0, 0, width, height);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        }

        // Return the new bitmap
        return resizedBmp;
    }

    // A method to get a two-dimensional array of colors from a bitmap image
    static Color[,] GetPixelArray(Bitmap bmp)
    {
        // Get the width and height of the bitmap
        int width = bmp.Width;
        int height = bmp.Height;

        // Create a two-dimensional array of colors with the same dimensions as the bitmap
        Color[,] pixels = new Color[width, height];

        // Loop through each pixel of the bitmap and store its color in the array
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                pixels[x, y] = bmp.GetPixel(x, y);
            }
        }

        // Return the pixel array
        return pixels;
    }

    // A method to compare two pixel arrays and return their average color deviation
    static double ComparePixelArrays(Color[,] pixels1, Color[,] pixels2)
    {
        // Initialize a variable to store the sum of color deviations
        double deviationSum = 0;

        // Get the width and height of the pixel arrays (assuming they are equal)
        int width = pixels1.GetLength(0);
        int height = pixels1.GetLength(1);

        // Loop through each pixel of both arrays and calculate their color deviation 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Get the colors of both pixels at (x,y) position 
                Color color1 = pixels1[x, y];
                Color color2 = pixels2[x, y];

                // Calculate their RGB differences 
                int rDiff = Math.Abs(color1.R - color2.R);
                int gDiff = Math.Abs(color1.G - color2.G);
                int bDiff = Math.Abs(color1.B - color2.B);

                // Calculate their average color difference 
                double avgDiff = (rDiff + gDiff + bDiff) / 3.0;

                // Add their average color difference to the sum of deviations 
                deviationSum += avgDiff;
            }
        }

        // Calculate the total number of pixels 
        int totalPixels = width * height;

        // Calculate and return the average color deviation by dividing by the total number of pixels 
        return deviationSum / totalPixels;
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
