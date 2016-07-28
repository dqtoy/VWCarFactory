PDF Reader v.1.35
This plugin provides iOS native API UIDocumentInteractionController. Now you can add documentation to your projects, just like you do on your desktop. View files from local folder or remote address. In addition you can open files in third party applications like Pages, iBooks, Evernote and other applications supporting file format.

New:
- Bitcode support
- New Sample Scene with Unity 5 UI

Features:
- Now you needn't add PDFs files to Xcode project, import files directly in Unity
- Reliably viewing PDF files using native iOS code (UIWebView or UIDocumentInteractionController)
- Support iOS 6.0 or higher
- Open files in third party applications
- Cache downloaded pdf files (iOS)
- Write and read pdf from PersistentDataPath
- Support Android methods
- Callback methods on WebView (iOS)
- Render separate pages for OpenDocCG method (iOS)
- Support opening local HTML pages (iOS)
- Cached pdf data from remote server without init and draw UIWebView

!!! Import PDFs files to the Unity “StreamingAssets” folder, if folder does not exist, create it. If you want to write and read PDFs files on runtime, use folder path:
string pdfPath = PDFReader.AppDataPath + “/” + “name.pdf”; (StreamingAssets folder)

How to use (C# Script):
PDFReader.OpenDocInMenu (string docPath, bool onlyThirdPartyApp);
PDFReader.OpenDocInWebViewLocal (string docPath, string docTitle);
PDFReader.OpenDocInWebViewLocal (string docPath, string docTitle, string goName, string callbackMethod);
PDFReader.OpenDocInWebViewRemote (string docTitle, string docRemoteURL);
PDFReader.OpenDocInWebViewRemote (string docTitle, string docRemoteURL, string backTitle, string goName, string callbackMethod);
PDFReader.OpenDocCG (string docPath);
PDFReader.OpenHTMLLocal (string docPath, string navbarTitle);
bool PDFReader.AutoCache;
bool PDFReader.IsPDFCached(string docRemoteURL);
bool PDFReader.CachePDFData(string docRemoteURL)

Android methods
IEnumerator PDFReader.OpenDocLocal (string docName);
PDFReader.OpenDocRemote (string docURL);
PDFReader.OpenDocRemote (string docURL, bool useGoogleDocs);

Demo Video:
http://www.youtube.com/watch?feature=player_detailpage&v=S6pXHPbnEJE
Detail example in plugin package.
Support: https://indieyp.com/qa/
My personal blog : http://indieyp.com
RoadMap : https://trello.com/b/xXHzdj9B/unity-asset-store