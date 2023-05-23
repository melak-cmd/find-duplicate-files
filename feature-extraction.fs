open System
open System.Drawing

let comparePicturesWithFeatureExtraction (imagePath1: string) (imagePath2: string) =
    let image1 = new Bitmap(imagePath1)
    let image2 = new Bitmap(imagePath2)

    // Convert images to grayscale
    let grayImage1 = new Bitmap(image1.Width, image1.Height)
    let grayImage2 = new Bitmap(image2.Width, image2.Height)

    let graphics1 = Graphics.FromImage(grayImage1)
    graphics1.DrawImage(image1, 0, 0, image1.Width, image1.Height)
    let graphics2 = Graphics.FromImage(grayImage2)
    graphics2.DrawImage(image2, 0, 0, image2.Width, image2.Height)

    // Perform Harris corner detection on the grayscale images
    let harris1 = new HarrisCornersDetector()
    let harris2 = new HarrisCornersDetector()

    let corners1 = harris1.ProcessImage(grayImage1)
    let corners2 = harris2.ProcessImage(grayImage2)

    // Compare the number of detected corners
    let cornerCountDiff = abs(corners1.Length - corners2.Length)

    cornerCountDiff

[<EntryPoint>]
let main args =
    printf "Enter path to the first image: "
    let imagePath1 = Console.ReadLine()

    printf "Enter path to the second image: "
    let imagePath2 = Console.ReadLine()

    let cornerCountDiff = comparePicturesWithFeatureExtraction imagePath1 imagePath2
    printfn "Difference in corner counts: %d" cornerCountDiff

    0
