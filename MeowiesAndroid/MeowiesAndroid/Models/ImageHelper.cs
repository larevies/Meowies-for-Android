using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace MeowiesAndroid.Models;

public static class ImageHelper
{
    public static Bitmap LoadFromResource(Uri resourceUri)
    {
        return new Bitmap(AssetLoader.Open(resourceUri));
        
    }
}