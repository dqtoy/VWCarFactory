using UnityEngine;
using System.Collections;

public class WeChatShare : MonoBehaviour {
    public WeChatPluginScript m_weChatShareObject;
    public Texture2D shareImage;
    Texture2D tex;
    // Use this for initialization
    void Start () {
        tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ShareTOFriend()
    {

        StartCoroutine(ShareTexture(false));

    }
    public void Share()
    {
        StartCoroutine(ShareTexture(true));

    }
    IEnumerator ShareTexture(bool __b)
    {
        yield return new WaitForEndOfFrame();

        try
        {
            m_weChatShareObject.m_isMoments = __b;
            m_weChatShareObject.m_thumbType = WeChatPluginScript.imageUploadType.TYPE_TEXTURE;
            m_weChatShareObject.m_thumbImage = shareImage;
            m_weChatShareObject.m_title = "";
            m_weChatShareObject.m_desc = "";
            m_weChatShareObject.m_shareType = WeChatPluginScript.ShareType.SHARETYPE_IMAGE;
            m_weChatShareObject.m_contentImageType = WeChatPluginScript.imageUploadType.TYPE_TEXTURE;
            

            Texture2D _screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            _screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            _screenshot.Apply();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/ScreenshotTemp.png", _screenshot.EncodeToPNG());
            tex.LoadImage(System.IO.File.ReadAllBytes(Application.persistentDataPath + "/ScreenshotTemp.png"));

            //Texture2D _screenshot2= new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
            //_screenshot2.LoadImage(_screenshot.EncodeToPNG());
            //_screenshot. = TextureFormat.RGBA32;
            m_weChatShareObject.m_contentImage = tex;

            m_weChatShareObject.Share();
        }
        catch (System.Exception e)
        {

            Debug.LogError(e.Message);
            Debug.LogError(e.Source);
            Debug.LogError(e.StackTrace);

        }

    }
}
