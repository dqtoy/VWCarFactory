using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using IndieYP;

public class ExampleNewUI : MonoBehaviour
{

		public string remotePdf = "http://gradcollege.okstate.edu/sites/default/files/PDF_linking.pdf";
		private string streamingPdf = "";
		private string localHTML = "";

		[Header ("Panels")]
		[SerializeField]
		private GameObject _iOSPanel;
		[SerializeField]
		private GameObject _androidPanel;

		[Header ("iOS Buttons")]
		[SerializeField]
		private Button _openInMenuBtn;
		[SerializeField]
		private Button _openInThirdPartyAppBtn;
		[SerializeField]
		private Button _openInWebViewLocalBtn;
		[SerializeField]
		private Button _openInWebViewRemoteBtn;
		[SerializeField]
		private Button _withCallbackBtn;
		[SerializeField]
		private Button _openCgBtn;
		[SerializeField]
		private Button _openHtmlBtn;
		[Header ("Android Buttons")]
		[SerializeField]
		private Button _openLocalBtn;
		[SerializeField]
		private Button _openRemoteBtn;
		[SerializeField]
		private Button _openInGoogleDocsBtn;

		void Awake ()
		{
				#if UNITY_IPHONE
				_iOSPanel.gameObject.SetActive (true);

				_openInMenuBtn.onClick.AddListener (OnOpenInMenuBtnClick);
				_openInThirdPartyAppBtn.onClick.AddListener (OnOpenInThirdPartyAppBtnClick);
				_openInWebViewLocalBtn.onClick.AddListener (OnOpenInWebViewLocalBtnClick);
				_openInWebViewRemoteBtn.onClick.AddListener (OnOpenInWebViewRemoteBtnClick);
				_withCallbackBtn.onClick.AddListener (OnWithCallbackBtnClick);
				_openCgBtn.onClick.AddListener (OnOpenCgBtnClick);
				_openHtmlBtn.onClick.AddListener (OnOpenHtmlBtnClick);
				#endif

				#if UNITY_ANDROID
				_androidPanel.gameObject.SetActive(true);
				_openLocalBtn.onClick.AddListener(OnOpenLocalBtnClick);
				_openRemoteBtn.onClick.AddListener(OnOpenRemoteBtnClick);
				_openInGoogleDocsBtn.onClick.AddListener(OnOpenInGoogleDocsBtnClick);
				#endif
		}

		void Start ()
		{
				//BUILD PDF PATH FROM STREAMING ASSETS 
				streamingPdf = PDFReader.AppDataPath + "/" + "test.pdf";
				localHTML = PDFReader.AppDataPath + "/" + "main.html";

				/*Download from remote server and save to PersistentDataPath
		StartCoroutine(PDFReader.DownloadPDF(remotePdf, "remoteName") );
		--- READ FROM PersistentDataPath ---
		PDFReader.OpenDocInWebViewLocal(PDFReader.PersistentDataPath + "/" + "remoteName.pdf", "Title");*/


				//Set Auto cache pdf files loaded from remote server
				PDFReader.AutoCache = true;

				//Manually cache pdf file
				//PDFReader.AutoCache = false;
				//bool status = PDFReader.CachePDFData(remotePdf);
				//Debug.Log("Cache data: " + status);
		}

		void OnDestroy ()
		{
				#if UNITY_IPHONE
				_openInMenuBtn.onClick.RemoveListener (OnOpenInMenuBtnClick);
				_openInThirdPartyAppBtn.onClick.RemoveListener (OnOpenInThirdPartyAppBtnClick);
				_openInWebViewLocalBtn.onClick.RemoveListener (OnOpenInWebViewLocalBtnClick);
				_openInWebViewRemoteBtn.onClick.RemoveListener (OnOpenInWebViewRemoteBtnClick);
				_withCallbackBtn.onClick.RemoveListener (OnWithCallbackBtnClick);
				_openCgBtn.onClick.RemoveListener (OnOpenCgBtnClick);
				_openHtmlBtn.onClick.RemoveListener (OnOpenHtmlBtnClick);
				#endif

				#if UNITY_ANDROID
				_openLocalBtn.onClick.RemoveListener(OnOpenLocalBtnClick);
				_openRemoteBtn.onClick.RemoveListener(OnOpenRemoteBtnClick);
				_openInGoogleDocsBtn.onClick.RemoveListener(OnOpenInGoogleDocsBtnClick);
				#endif
		}

		#region BUTTONS_IMPLEMENTATIONS

		//method OpenDocInMenu using UIDocumentInteractionController (iOS API), so you can open .pdf, .doc, .txt
		// and other formats which supports applications installed on your device
		#if UNITY_IPHONE

		void OnOpenInMenuBtnClick ()
		{
				PDFReader.OpenDocInMenu (streamingPdf, false);
		}

		void OnOpenInThirdPartyAppBtnClick ()
		{
				PDFReader.OpenDocInMenu (streamingPdf, true);
		}

		// USE THESE METHODS ONLY TO OPEN PDFs FILES

		void OnOpenInWebViewLocalBtnClick ()
		{
				PDFReader.OpenDocInWebViewLocal (streamingPdf, "Unity Test", "Back", true);
				//PDFReader.SetWebviewPageOffset(2);
		}

		void OnOpenInWebViewRemoteBtnClick ()
		{
				PDFReader.OpenDocInWebViewRemote ("Test Title", remotePdf, "Back");
		}

		//With callback args
		void OnWithCallbackBtnClick ()
		{
				PDFReader.OpenDocInWebViewLocal (streamingPdf, "Title", "Back Test", gameObject.name, "CallbackMethod");
		}

		//experimental feature (works only in portrait mode and with files < 500Mb)
		void OnOpenCgBtnClick ()
		{
				PDFReader.OpenDocCG (streamingPdf);
				//for open pages 2-4
				//PDFReader.OpenDocCG(streamingPdf, 2, 4);
				//render in custom rect
				//PDFReader.OpenDocCG(streamingPdf, rect: "100, 300, 500, 500");
				//PDFReader.OpenDocCG(streamingPdf, gameObject.name, "CallbackMethod", 2, 4);
		}

		void OnOpenHtmlBtnClick ()
		{
				PDFReader.OpenHTMLLocal (localHTML, "HTML Test");
		}
		#endif

		//Android methods
		#if UNITY_ANDROID
		void OnOpenLocalBtnClick()
		{
				StartCoroutine(PDFReader.OpenDocLocal("test"));
		}

		void OnOpenRemoteBtnClick()
		{
				PDFReader.OpenDocRemote(remotePdf);
		}

		void OnOpenInGoogleDocsBtnClick()
		{
				PDFReader.OpenDocRemote(remotePdf, true);
		}
		#endif

		#endregion

		#if UNITY_IPHONE
		//method call when user tap back button on WebView (iOS)
		public void CallbackMethod (string response)
		{
				if (response == PDFReader.STATUS_COMPLETE)
						Debug.Log ("Back to Unity Activity done");
		}
		#endif
   

}