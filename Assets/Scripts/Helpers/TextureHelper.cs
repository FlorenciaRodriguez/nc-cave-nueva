namespace Assets.Scripts.Helpers
{
  using System;
  using System.IO;

  using UnityEngine;

  public static class TextureHelper
  {
    public enum TextureEncodeEnum
    {
      PNG,
      JPG,
      TGA
    }

    public static Texture2D LoadTexture(string path)
    {
      Texture2D texture = null;

      if (File.Exists(path))
      {
        var fileData = File.ReadAllBytes(path);
        texture = new Texture2D(2, 2) { name = Path.GetFileNameWithoutExtension(path) };
        texture.LoadImage(fileData);
      }

      return texture;
    }

    public static void SaveTexture(Texture texture, string path, TextureEncodeEnum textureEncode)
    {
      var texture2D = texture.ToTexture2D();
      byte[] bytes;

      switch (textureEncode)
      {
        case TextureEncodeEnum.PNG:
          bytes = texture2D.EncodeToPNG();
          break;
        case TextureEncodeEnum.JPG:
          bytes = texture2D.EncodeToJPG();
          break;
        case TextureEncodeEnum.TGA:
          bytes = texture2D.EncodeToTGA();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(textureEncode), textureEncode, null);
      }

      var folder = Directory.GetParent(path).FullName;
      if (!Directory.Exists(folder))
      {
        Directory.CreateDirectory(folder);
      }

      File.WriteAllBytes(path, bytes);
    }

    public static Texture2D ToTexture2D(this Texture texture)
    {
      var texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

      var currentRenderTexture = RenderTexture.active;
      var renderTexture = new RenderTexture(texture.width, texture.height, 32);

      Graphics.Blit(texture, renderTexture);

      RenderTexture.active = renderTexture;

      texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
      texture2D.Apply();

      RenderTexture.active = currentRenderTexture;

      return texture2D;
    }
  }
}