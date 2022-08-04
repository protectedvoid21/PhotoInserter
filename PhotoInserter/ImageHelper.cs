using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PhotoInserter;

public class ImageHelper {
    private static List<Coordinates> GetPointsPositions(int imageCount, int width, int height) {
        List<Coordinates> points = new();

        int topRowImageCount = (imageCount + 1) / 2;
        for (int i = 0; i < topRowImageCount; i++) {
            points.Add(new Coordinates(width / topRowImageCount * i, 0));
        }
            
        int bottomRowImageCount = imageCount / 2;
        for (int i = 0; i < bottomRowImageCount; i++) {
            points.Add(new Coordinates(width / bottomRowImageCount * i, height / 2));
        }

        return points;
    }

    public static void CreateImage(int width, int height, string[] imagePaths, string savePath) {
        int imageCount = imagePaths.Length;
        List<Coordinates> coordinates = GetPointsPositions(imageCount, width, height);

        using Bitmap photoBackground = new(width, height);

        using var graphics = Graphics.FromImage(photoBackground);

        int imageWidth = width / ((imageCount + 1) / 2);
        int imageHeight = imageCount > 2 ? height / 2 : height;

        for (int i = 0; i < imageCount; i++) {
            Image userImage = Image.FromFile(imagePaths[i]);
            graphics.DrawImage(userImage, coordinates[i].X, coordinates[i].Y, imageWidth, imageHeight);
        }

        photoBackground.Save(savePath, ImageFormat.Png);
    }
}