using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;



public class Sharing : MonoBehaviour
{
	#region PUBLIC_VARIABLES


	[Header("Choose option for email (Warning :  To receipients not work in IOS) ")]
	[Tooltip("Not support in Ios")]
	public List<string> to;
	public string subject;
	public string message;

	[Space(10)]
	[Header("Choose File and image to be shared")]
	public List<TextAsset> files;
	public List<Texture2D> images;
	public List<string> imgPath;

	private List<string> filesPathsAll;

	[Space(10)]
	[Header("Choose app to exclude")]

	public bool excludeMail = true;
	public bool excludeMessage = true;
	public bool excludePostToFacebook = true;
	public bool excludePostToTwitter = true;
	public bool excludePostToFlickr = true;
	public bool excludeSaveToCameraRoll = true;
	public bool excludePostToWeibo = true;

	private bool excludePostToVimeo = true;
	private bool excludePrint = true;

	#if UNITY_IPHONE || UNITY_IPAD

	private bool excludeAirDrop = true;
	private bool excludeAssignToContact = true;
	private bool excludeAddToReadingList = true;
	private bool excludeCopyToPasteboard = true;
	public bool excludePostToTencentWeibo = true;

	#endif
	#if UNITY_ANDROID

	private bool excludeGooglePlus = true;
	private bool excludeTalk = true;
	private bool excludeGoogleDrive = true;
	private bool excludeYoutube = true;

	#endif

	public event EventHandler<NullEventArgs> SendPicturesFinishHandler;

	#endregion

	#region CO_ROUTINES

	#if UNITY_ANDROID

	private List<string> GetShareApplication()
	{
		List<string> excludeShareType=new List<string>();

		if (!excludeMail)
		{
			excludeShareType.Add ("com.google.android.gm");
			excludeShareType.Add ("mail");		
			excludeShareType.Add ("outlook");		
		}
		if (!excludeMessage)
		{
			excludeShareType.Add ("mms");
			excludeShareType.Add ("sms");
			excludeShareType.Add ("messaging");
		}
		if (!excludePostToFacebook)
			excludeShareType.Add ("com.facebook.katana");
		if (!excludePostToTwitter)
			excludeShareType.Add ("twitter");
		if (!excludePostToVimeo)
			excludeShareType.Add ("vimeo");
		if (!excludePrint)
			excludeShareType.Add ("print");
		if (!excludeSaveToCameraRoll)
			excludeShareType.Add ("com.google.android.apps.photos");
		if (!excludePostToWeibo)
			excludeShareType.Add ("weibo");
		if (!excludePostToFlickr)
			excludeShareType.Add ("flickr");
		if (!excludeGooglePlus)
			excludeShareType.Add ("com.google.android.apps.plus");
		if (!excludeTalk)
			excludeShareType.Add ("com.google.android.talk");
		if (!excludeGoogleDrive)
			excludeShareType.Add ("com.google.android.apps.docs");
		if (!excludeYoutube)
			excludeShareType.Add ("youtube");


		return excludeShareType;

	}

	#elif UNITY_IPHONE || UNITY_IPAD

	private List<string> GetShareApplication()
	{
		List<string> excludeShareType=new List<string>();

		if (excludeMail)
			excludeShareType.Add ("UIActivityTypeMail");
		if (excludeMessage)
			excludeShareType.Add ("UIActivityTypeMessage");
		if (excludePostToFacebook)
			excludeShareType.Add ("UIActivityTypePostToFacebook");
		if (excludePostToFlickr)
			excludeShareType.Add ("UIActivityTypePostToFlickr");
		if (excludePostToTencentWeibo)
			excludeShareType.Add ("UIActivityTypePostToTencentWeibo");
		if (excludePostToTwitter)
			excludeShareType.Add ("UIActivityTypePostToTwitter");
		if (excludePostToVimeo)
			excludeShareType.Add ("UIActivityTypePostToVimeo");
		if (excludePostToWeibo)
			excludeShareType.Add ("UIActivityTypePostToWeibo");
		if (excludePrint)
			excludeShareType.Add ("UIActivityTypePrint");
		if (excludeSaveToCameraRoll)
			excludeShareType.Add ("UIActivityTypeSaveToCameraRoll");
		if (excludeCopyToPasteboard)
			excludeShareType.Add ("UIActivityTypeCopyToPasteboard");
		if (excludeAssignToContact)
			excludeShareType.Add ("UIActivityTypeAssignToContact");
		if (excludeAirDrop)
			excludeShareType.Add ("UIActivityTypeAirDrop");
		if (excludeAddToReadingList)
			excludeShareType.Add ("UIActivityTypeAddToReadingList");

		return excludeShareType;
	}
	#endif

	#if UNITY_ANDROID
	private void AddToIntent(AndroidJavaObject intentObject  )
	{
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");

		if (filesPathsAll.Count<=1)
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		else
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND_MULTIPLE"));


		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		//intentObject.Call<AndroidJavaObject>("setType", "plain/text");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_EMAIL"), to.ToArray());
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

		AndroidJavaObject arrayURIByImage = new AndroidJavaObject("java.util.ArrayList");
		foreach (var path in filesPathsAll)
		{
			AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);// Set Image Path Here
			bool fileExist = fileObject.Call<bool>("exists");
			if (fileExist)
			{
				AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
				arrayURIByImage.Call<bool>("add",uriObject);
			}
		}


		if (filesPathsAll.Count==1)
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), arrayURIByImage.Call<AndroidJavaObject>("get",0));
		else
			intentObject.Call<AndroidJavaObject>("putParcelableArrayListExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),arrayURIByImage );
		
	}

	IEnumerator SaveAndShare ()
	{
		yield return new WaitForEndOfFrame ();	

		AndroidJavaClass plugin = new AndroidJavaClass("sharing.superdev.com.sharing.MainActivity");
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");		  
		
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		
		AddToIntent(intentObject);

		List<string> listTartgetApp = GetShareApplication();

		AndroidJavaObject targetedShareIntents = new AndroidJavaObject("java.util.ArrayList");

		AndroidJavaObject listResolve = currentActivity.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("queryIntentActivities",intentObject,0);

		if(!listResolve.Call<bool>("isEmpty"))
		{
			Debug.Log("SIZE : "+listResolve.Call<int>("size"));
			for (int i = 0; i < listResolve.Call<int>("size"); i++) 
			{
				
				AndroidJavaObject resolve = listResolve.Call<AndroidJavaObject>("get",i);

				AndroidJavaObject intentObjectTemp = new AndroidJavaObject("android.content.Intent");
				intentObjectTemp.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND_MULTIPLE"));
				intentObjectTemp.Call<AndroidJavaObject>("setType", "image/*");

				string packanageName = resolve.Get<AndroidJavaObject>("activityInfo").Get<string>("packageName").ToLower();
				Debug.Log("Pck : "+packanageName);
				if (TargetContainsPackageCurrent(listTartgetApp,packanageName)) 
				{
					Debug.Log("Pck  OK: "+packanageName);
					AddToIntent(intentObjectTemp);
					intentObjectTemp.Call<AndroidJavaObject>("setPackage",packanageName);
					targetedShareIntents.Call<bool>("add",intentObjectTemp);
				}
			}
		}

		//custom Fonction  : present in file  SharingSuperdev
		AndroidJavaObject newintent = plugin.CallStatic<AndroidJavaObject>("CreateChooserWithSpecificApp",targetedShareIntents);

		currentActivity.Call("startActivity", newintent);
	}

	private bool TargetContainsPackageCurrent(List<string> listTartgetApp,string packanageName )
	{
		foreach (var target in listTartgetApp)
		{
			if (packanageName.Contains(target))
				return true;
		}
		return false;
	}
	#endif
	#endregion
	
	#region BUTTON_CLICK_LISTENER

	public void OnShare ()
	{
        

        StartCoroutine(OnShareCoroutine());
	}
	public IEnumerator OnShareCoroutine ()
	{
        yield return new WaitForEndOfFrame();
        if (images.Count > 1)
        {
            images.Clear();
        }
        if (images.Count == 1)
        {
            images[0].ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            images[0].Apply();
        }
        else
        {
            Texture2D _screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            _screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            _screenshot.Apply();
            images.Add(_screenshot);
        }
        //Debug.Log ("Share");
		filesPathsAll = new List<string>();
		filesPathsAll.AddRange(imgPath);
		foreach (var image in images)
		{
			byte[] bytes = image.EncodeToPNG();
			string path = Application.persistentDataPath + "/"+image.name+".png";
			File.WriteAllBytes(path, bytes);
			filesPathsAll.Add(path);
		}

		foreach (var file in files)
		{
			byte[] bytes = file.bytes;
			string path = Application.persistentDataPath + "/"+file.name+".pdf";
			File.WriteAllBytes(path, bytes);
			filesPathsAll.Add(path);
		}


		#if UNITY_ANDROID

		yield return StartCoroutine (SaveAndShare ());

		#elif UNITY_IPHONE || UNITY_IPAD

		List<string> excludeShareType = GetShareApplication();
		SharingiOSBridge.Share (filesPathsAll.ToArray(),message,subject,filesPathsAll.Count,excludeShareType.ToArray(),excludeShareType.Count);

		#endif

		yield return null;
		filesPathsAll.Clear();

		if(SendPicturesFinishHandler != null)
			SendPicturesFinishHandler(this,new NullEventArgs());
	}
	#endregion
	
}
