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
        string _fileName = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        string _ScreenshotPath = Application.persistentDataPath + "/Screenshot.png";

        File.WriteAllBytes(_ScreenshotPath, __tex.EncodeToPNG());

        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");


        jo.CallStatic("SaveToAlbum", _fileName);
#endif


    }

}
