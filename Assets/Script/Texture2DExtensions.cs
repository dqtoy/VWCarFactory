using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;


public static class Texture2DExtensions
{
    [DllImport("__Internal")]
    private static extern void _SavePhoto(string readAddr);
    /// <summary>
    /// 保存图片到相册
    /// </summary>
    /// <param name="__tex"></param>
    public static void SaveToAlbum(this Texture2D __tex)
    {
        
#if UNITY_IOS
        string _ScreenshotPath = Application.persistentDataPath + "/Screenshot.png";
        File.WriteAllBytes(_ScreenshotPath, __tex.EncodeToPNG());
        _SavePhoto(_ScreenshotPath);
#elif UNITY_ANDROID
        string _ScreenshotPath = "/mnt/sdcard/DCIM/";
        if (!Directory.Exists(_ScreenshotPath))
        {
            Directory.CreateDirectory(_ScreenshotPath);
        }
        string _Screenshot = _ScreenshotPath + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ".png";
        //string _Screenshot = _ScreenshotPath  + "1.png";

        File.WriteAllBytes(_ScreenshotPath, __tex.EncodeToPNG());
#endif


    }

}
