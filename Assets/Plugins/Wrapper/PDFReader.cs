using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace IndieYP
{

public class PDFReader : MonoBehaviour 
{
	#region PUBLIC_VARIABLES
	public const string STATUS_COMPLETE = "OK";
	public enum Anchor
		{
			Default,
			Bottom
		}
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="IndieYP.PDFReader"/> auto cache.
	/// </summary>
	/// <value><c>true</c> if auto cache; otherwise, <c>false</c>.</value>
	public static bool AutoCache { get { return autoCache; } set { autoCache = value; }}
	#endregion

	#region PRIVATE_VARIABLES
	private static bool autoCache;
	#endregion
	
	[DllImport("__Internal")]
	private static extern void OpenDocumentInMenu(string docPath, bool onlyThirdPartyApp, Anchor anchor);
	[DllImport("__Internal")]
	private static extern void OpenDocumentInWebViewLocal(string docPath, string docTitle, string backTitle, bool share);
	[DllImport("__Internal")]
	private static extern void OpenDocumentInWebViewLocalWithCallback(string docPath, string docTitle, string backTitle, bool share, string goName, string callbackMethod);
	[DllImport("__Internal")]
	private static extern void OpenDocumentInWebViewRemote(string docTitle, string docRemoteURL, string backTitle, bool share);
	[DllImport("__Internal")]
	private static extern void OpenDocumentInWebViewRemoteWithCallback(string docTitle, string docRemoteURL, string backTitle, bool share, string goName, string callbackMethod);
	[DllImport("__Internal")]
	private static extern void OpenDocumentCG(string docPath, int from, int to, string rect);
	[DllImport("__Internal")]
	private static extern void OpenDocumentCGWithCallback(string docPath, int from, int to, string rect, string goName, string callbackMethod);
	[DllImport("__Internal")]
	private static extern void OpenHTML(string docPath, string navbarTitle);
	[DllImport("__Internal")]
	private static extern bool CacheData(string docRemoteURL);
	[DllImport("__Internal")]
	private static extern bool IsCached(string docRemoteURL);
	[DllImport("__Internal")]
	private static extern void SetWebViewContentOffset(int page);

#if UNITY_IPHONE

	public static void OpenDocInMenu(string docPath, bool onlyThirdPartyApp, Anchor anchor = Anchor.Bottom)
	{
		OpenDocumentInMenu(docPath, onlyThirdPartyApp, anchor);
	}

	public static void OpenDocInWebViewLocal(string docPath, string docTitle, string backTitle = "Back", bool share = false)
	{
		OpenDocumentInWebViewLocal(docPath, docTitle, backTitle, share);
	}

	public static void OpenDocInWebViewLocal(string docPath, string docTitle, string backTitle, string goName, string callbackMethod, bool share = false)
	{
		OpenDocumentInWebViewLocalWithCallback(docPath, docTitle, backTitle, share, goName, callbackMethod);
	}

	public static void OpenDocInWebViewRemote(string docTitle, string docRemoteURL, string backTitle = "Back", bool share = false)
	{
		if(autoCache)
			CacheData(docRemoteURL);
		OpenDocumentInWebViewRemote(docTitle, docRemoteURL, backTitle, share);
	}

	public static void OpenDocInWebViewRemote(string docTitle, string docRemoteURL, string backTitle, string goName, string callbackMethod, bool share = false)
	{
		if(autoCache)
			CacheData(docRemoteURL);
		OpenDocumentInWebViewRemoteWithCallback(docTitle, docRemoteURL, backTitle, share, goName, callbackMethod);
	}

	public static void OpenDocCG(string docPath, int from = -7, int to = 0, string rect = "")
	{
		if(string.IsNullOrEmpty(rect))
			rect = "0,0," + Screen.width.ToString() + "," + Screen.height.ToString();
		OpenDocumentCG(docPath, from, to, rect);
	}
	public static void OpenDocCG(string docPath, string goName, string callbackMethod, int from = -7, int to = 0, string rect = "")
	{
		if(string.IsNullOrEmpty(rect))
			rect = "0,0," + Screen.width.ToString() + "," + Screen.height.ToString();
		OpenDocumentCGWithCallback(docPath, from, to, rect, goName, callbackMethod);
	}
	public static void OpenHTMLLocal(string docPath, string navbarTitle)
	{
		OpenHTML(docPath, navbarTitle);
	}
	/// <summary>
	/// Cached pdf data from remote server without init and draw UIWebView
	/// </summary>
	/// <returns><c>true</c>, if PDF data was cached, <c>false</c> otherwise.</returns>
	/// <param name="docRemoteURL">Document remote URL.</param>
	public static bool CachePDFData(string docRemoteURL)
	{
		return CacheData(docRemoteURL);
	}
	public static bool IsPDFCached(string docRemoteURL)
	{
		return IsCached(docRemoteURL);
	}
	/// <summary>
	/// Sets the webview page offset. Page offset = WebView height
	/// </summary>
	/// <param name="page">Page.</param>
	public static void SetWebviewPageOffset(int page)
	{
		SetWebViewContentOffset(page);			
	}

#endif

#if UNITY_ANDROID
	private static AndroidJavaClass pdfJavaClass;
	private static AndroidJavaObject activity;
	private static void InitJava()
	{
		if(pdfJavaClass == null)
		{
			AndroidJavaClass unityJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			activity = unityJC.GetStatic<AndroidJavaObject>("currentActivity");
			pdfJavaClass = new AndroidJavaClass("com.SlavaObninsk.pdfreader.Logic");
		}
	}

	public static void OpenDocRemote (string docRemoteURL)
	{
		InitJava();
		pdfJavaClass.CallStatic("OpenDocRemote", activity, docRemoteURL);
	}
	public static void OpenDocRemote (string docRemoteURL, bool useGoogleDocs)
	{
		if(useGoogleDocs)
		{
			InitJava();
			pdfJavaClass.CallStatic("OpenInGoogleDocs", activity, docRemoteURL);
		}
		else
		{
			OpenDocRemote(docRemoteURL);
		}
	}


	public static IEnumerator OpenDocLocal (string pdfName)
	{
		string fromPath = Application.streamingAssetsPath +"/";
		string toPath =  Application.persistentDataPath + "/";
		string file = pdfName + ".pdf";
		WWW www = new WWW ( fromPath + file);
		yield return www;

		string tempPath =  toPath + file;

		if (!File.Exists(tempPath))
			File.WriteAllBytes(tempPath, www.bytes);
		
		PDFReader.OpenDocLocalNative(tempPath);
	}

	private static void OpenDocLocalNative (string docLocalURL)
	{
		InitJava();
		pdfJavaClass.CallStatic("OpenDocLocal", activity, docLocalURL);
	}


#endif
	
#region IO METHODS

	/// <summary>
	/// Gets the app streaming assets data path.
	/// </summary>
	/// <value>The app data path.</value>
	public static Uri AppDataPath
	{
		get 
		{
			UriBuilder uriBuilder = new UriBuilder();      
			uriBuilder.Scheme = "file";
			uriBuilder.Path = System.IO.Path.Combine(appDataPath, "Raw");
			return uriBuilder.Uri;
		}
	}

	public static Uri PersistentDataPath
	{
		get 
		{
			UriBuilder uriBuilder = new UriBuilder();      
			uriBuilder.Scheme = "file";
			uriBuilder.Path = persistentDataPath /*System.IO.Path.Combine(appDataPath, "Raw")*/;
			return uriBuilder.Uri;
		}
	}

	private static string appDataPath
	{
		get 
		{               
			return Application.dataPath;
		}
	}

	private static string persistentDataPath
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

#endregion

#region UTILS
	public static IEnumerator DownloadPDF(string url, string saveAs)
	{
		string file = saveAs + ".pdf";
		string destination =  Application.persistentDataPath + "/" + file;
		WWW www = new WWW(url);
		yield return www;
		File.WriteAllBytes(destination, www.bytes);
	}
#endregion

}

}