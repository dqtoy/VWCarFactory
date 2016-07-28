using UnityEngine;
using System;
using System.IO;
using System.Collections;
using IndieYP;

public class Example : MonoBehaviour 
{

	public string remotePdf = "http://gradcollege.okstate.edu/sites/default/files/PDF_linking.pdf";
	private string streamingPdf = "";
	private string localHTML = "";
	
	void Start()
	{
		//BUILD PDF PATH FROM STREAMING ASSETS 
		streamingPdf = PDFReader.AppDataPath + "/" +"test.pdf";
		localHTML = PDFReader.AppDataPath + "/" +"main.html";

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

	void OnGUI()
	{
		//method OpenDocInMenu using UIDocumentInteractionController (iOS API), so you can open .pdf, .doc, .txt 
		// and other formats which supports applications installed on your device
#if UNITY_IPHONE
		if (GUI.Button(new Rect(210, 80, 170, 40), "OpenInMenu"))
			PDFReader.OpenDocInMenu(streamingPdf, false );

		if (GUI.Button(new Rect(390, 80, 170, 40), "OpenInThirdPartyApp"))
			PDFReader.OpenDocInMenu(streamingPdf, true );

		// USE THESE METHODS ONLY TO OPEN PDFs FILES
		if (GUI.Button(new Rect(570, 80, 170, 40), "OpenInWebViewLocal"))
		{
			PDFReader.OpenDocInWebViewLocal(streamingPdf, "Unity Test", "Back", true);
			PDFReader.SetWebviewPageOffset(2);
		}

		if (GUI.Button(new Rect(750, 80, 170, 40), "OpenInWebViewRemote"))
			PDFReader.OpenDocInWebViewRemote("Test Title", remotePdf, "Back");

		//With callback args
		if (GUI.Button(new Rect(750, 180, 170, 40), "WithCallback"))
			PDFReader.OpenDocInWebViewLocal(streamingPdf, "Title", "Back Test", gameObject.name, "CallbackMethod");

		//experimental feature (works only in portrait mode and with files < 500Mb)
		if (GUI.Button(new Rect(210, 180, 170, 40), "OpenCG"))
			PDFReader.OpenDocCG (streamingPdf);
			//for open pages 2-4
			//PDFReader.OpenDocCG(streamingPdf, 2, 4);
			//render in custom rect
			//PDFReader.OpenDocCG(streamingPdf, rect: "100, 300, 500, 500");
			//PDFReader.OpenDocCG(streamingPdf, gameObject.name, "CallbackMethod", 2, 4);

		if (GUI.Button(new Rect(420, 180, 170, 40), "OpenHTML"))
			PDFReader.OpenHTMLLocal(localHTML, "HTML Test");

	
#endif

#if UNITY_ANDROID
		if (GUI.Button(new Rect (210, 80, 170, 40), "OpenLocal"))
			StartCoroutine(PDFReader.OpenDocLocal("test"));

		if (GUI.Button(new Rect (390, 80, 170, 40), "OpenRemote"))
			PDFReader.OpenDocRemote(remotePdf);

		if (GUI.Button(new Rect (570, 80, 170, 40), "OpenInGoogleDocs"))
			PDFReader.OpenDocRemote(remotePdf, true);
#endif
	}

#if UNITY_IPHONE
	//method call when user tap back button on WebView (iOS)
	public void CallbackMethod(string response)
	{
		if(response == PDFReader.STATUS_COMPLETE)
			Debug.Log("Back to Unity Activity done");
	}
#endif
   

}