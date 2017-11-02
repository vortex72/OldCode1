using System;
using System.IO;

namespace EPWI.Components.Utility
{
  public static class FileUtility
  {
    public static byte[] ResizeImage(string OriginalFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
    {
      System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

      // Prevent using images internal thumbnail
      FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
      FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

      if (OnlyResizeIfWider)
      {
        if (FullsizeImage.Width <= NewWidth)
        {
          NewWidth = FullsizeImage.Width;
        }
      }

      int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
      if (NewHeight > MaxHeight)
      {
        // Resize with height instead
        NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
        NewHeight = MaxHeight;
      }

      System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

      // Clear handle to original file so that we can overwrite it if necessary
      FullsizeImage.Dispose();

      var ms = new MemoryStream();
      // Save resized picture
      NewImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
      return ms.ToArray();
    }


  }
}
