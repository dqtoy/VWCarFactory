using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniWeibo;
using System;
//using System.Web;
public class UniWeiboDemo : MonoBehaviour {
	public GUIText debugText;
	//public GUITexture imageFile;
	public string AppKey = "你的Appkey";
	public string AppSecrect = "你的AppSecrect";
	//AccessToken 登录授权后获得
	string MyAcessToken = "";
	public string CallbackUrl = "你的回调页面";
	public string UserName = "新浪微博账号";
	public string Password = "新浪微博密码";
	
	bool hasLogin = false;
	Client Sina = null;
	
	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll();
		//Login();
		//TestAccount();
		//TestComment();
		//TestStatuses();
	}

	void Login()
	{
		OAuth oauth = null;
		// load saved accesstoken
		MyAcessToken = PlayerPrefs.GetString("AccessToken");
		
		if (string.IsNullOrEmpty(MyAcessToken))	//判断配置文件中有没有保存到AccessToken，如果没有就进入授权流程
		{
			oauth = Authorize();
			if(oauth==null){
				Debug.Log("获取AccessToken失败！");
				return;
			}
			if (!string.IsNullOrEmpty(oauth.AccessToken))
			{
				Console.WriteLine("获取AccessToken{{{0}}}成功！", oauth.AccessToken);
				Debug.Log("获取AccessToken成功！"+ oauth.AccessToken);
				// save accesstoken to PlayerPrefs
				PlayerPrefs.SetString("AccessToken", oauth.AccessToken);
			}
		}
		else//如果配置文件中保存了AccesssToken
		{
			//Console.WriteLine("获取到已保存的AccessToken{{{0}}}！", AccessToken);
			oauth = new OAuth(AppKey, AppSecrect, MyAcessToken, CallbackUrl);	//用Token实例化OAuth无需再次进入验证流程
			Debug.Log("验证Token有效性...");
			TokenResult result = oauth.VerifierAccessToken();	//测试保存的AccessToken的有效性
			if (result == TokenResult.Success)
			{
				Debug.Log("AccessToken有效！");
			}
			else
			{
				Debug.Log("AccessToken无效！因为：" + result);
				oauth = Authorize();
				if (!string.IsNullOrEmpty(oauth.AccessToken))
				{
					//Console.WriteLine("获取AccessToken{{{0}}}成功！", oauth.AccessToken);
					Debug.Log("获取AccessToken成功！"+ oauth.AccessToken);
					// save accesstoken to PlayerPrefs
					PlayerPrefs.SetString("AccessToken", oauth.AccessToken);
				}				
				
			}
		}

		//授权成功了。调用接口。
		Sina = new Client(oauth);

	}

	OAuth Authorize()
	{
		OAuth o = new OAuth(AppKey, AppSecrect, string.Empty, null);
		o.CallbackUrl = CallbackUrl;
		//if login success
		if(o.ClientLogin(UserName, Password)){
			return o;
		}
		else{
			return null;
		}
		
	}
#region Account
	// 测试账号接口
	void TestAccount(){
		try
		{
			
			//account/get_uid 获取用户uid
			string uid = Sina.API.Account.GetUID();
			debugText.text = "uid=" + uid;
			Debug.Log("uid = " + uid);
			
//			//account/get_privacy
//			UniSocial.Weibo.Entities.PrivacyEntity pe = Sina.API.Account.GetPrivacy();
//			Debug.Log(pe.ToString());
//			
//			//account/profile/school_list
//			IEnumerable<UniSocial.Weibo.Entities.SchoolEntity> schools = Sina.API.Account.SchoolList("11", "", "", "", "b", "");
//			foreach(var school in schools){
//				Debug.Log(school.Name);
//				//Debug.Log(school.ToString());
//			}
//			
//			//account/rate_limit_status
//			Debug.Log(Sina.API.Account.RateLimitStatus().ToString());
			
//			account/end_session 退出登录
//			Debug.Log(Sina.API.Account.EndSession().ToString());
			
		}		
		catch (WeiboException ex)
		{
			Debug.Log("出错啦！"+ex.Message);
		}		
	}
	//异步获取用户uid
	void TestAccountAsync(){
		try
		{
			
			debugText.text = "非主线程无法对guitext进行更改，请查看log实时监测数据。";
			Sina.AsyncInvoke<string>(
				//第一个代理中编写调用API接口的相关逻辑
			delegate()
			{
				Debug.Log("发送请求来获得用户ID...");
				//System.Threading.Thread.Sleep(8000); //等待8秒
				return Sina.API.Account.GetUID();
			},
				//第二个代理为回调函数，异步完成后将自动调用这个函数来处理结果。
			delegate(AsyncCallback<string> callback)
			{
				if (callback.IsSuccess)
				{
					Debug.Log( "获取用户ID成功，ID:" + callback.Data);
				}
				else
				{
					Debug.Log("获取用户ID失败，异常:" + callback.Error);
				}
			}
			 );			
		}		
		catch (WeiboException ex)
		{
			Debug.Log("出错啦！"+ex.Message);
		}		
	}	
#endregion
	
#region Comments
	//测试评论
	void TestComment(){
		try
		{
		//comments/to_me
			Debug.Log(Sina.API.Comments.ToMe("0", "0", 40, 1, 0, 0).ToString());
			//comments/by_me
			Debug.Log(Sina.API.Comments.ByMe("0", "0", 40, 1, 0).ToString());
			//comments/timeline
			Debug.Log(Sina.API.Comments.Timeline("0", "0", 40, 1).ToString());
			//comments/mentions
			Debug.Log(Sina.API.Comments.Mentions("0", "0", 40, 1, 0, 0).ToString());
		}		
		catch (WeiboException ex)
		{
			Debug.Log("出错啦！"+ex.Message);
		}
	}
#endregion

#region Statuses
	
	//上传图片并发布一条微博
	void TestUpload(){
		TextAsset bindata= Resources.Load("picture") as TextAsset;
		byte[] imgeBytes = bindata.bytes;
		try
		{
			Debug.Log(Sina.API.Statuses.Upload(string.Format("我用UniWeibo上传了一张图片 at{0}",DateTime.Now.ToShortTimeString()), imgeBytes, 0, 0, "").CreatedAt);	
		}
		catch (WeiboException ex)
		{
			Debug.Log("出错啦！"+ex.Message);
		}
		debugText.text = "发布成功，请登录微博查看。";
	}
	//异步上传图片并发布一条微博
	void TestUploadAsync(){
		TextAsset bindata= Resources.Load("picture") as TextAsset;
		byte[] imgeBytes = bindata.bytes;
		try
		{
			debugText.text = "非主线程无法对guitext进行更改，请查看log实时监测数据。";
			Sina.AsyncInvoke<UniWeibo.Entities.status.Entity>(
				//第一个代理中编写调用API接口的相关逻辑
			delegate()
			{
				Debug.Log("发送发布微博请求...");
				//System.Threading.Thread.Sleep(8000); //等待8秒
				return Sina.API.Statuses.Upload(string.Format("我用UniWeibo上传了一张图片 at{0}",DateTime.Now.ToShortTimeString()), imgeBytes, 0, 0, "");
			},
				//第二个代理为回调函数，异步完成后将自动调用这个函数来处理结果。
			delegate(AsyncCallback<UniWeibo.Entities.status.Entity> callback)
			{
				if (callback.IsSuccess)
				{
					Debug.Log( "发布微博成功" + callback.Data.CreatedAt);
				}
				else
				{
					Debug.Log("发布微博失败，异常:" + callback.Error);
				}
			}
			 );
		}
		catch (WeiboException ex)
		{
			Debug.Log("出错啦！"+ex.Message);
		}		
		
	}	
	//发布一条微博
	void TestStatuses(){
		try
		{
			var result = Sina.API.Statuses.Update(string.Format("我用UniWeibo 发布了一条微博，欢迎关注@UniSocial http://unisocial.sinaapp.com Time: {0}", DateTime.Now.ToShortTimeString()));	
			if(result==null){
				Debug.Log("发送失败，请检查网络或者参数是否正确！");
				return;
			}
		}
		catch (WeiboException ex)
		{
			Debug.Log("出错啦！"+ex.Message);
		}
		debugText.text = "发布成功，请登录微博查看。";
	}
	//异步发布一条微博
	void TestStatusesAsync(){
		try
		{
			debugText.text = "非主线程无法对guitext进行更改，请查看log实时监测数据。";
			Sina.AsyncInvoke<UniWeibo.Entities.status.Entity>(
				//第一个代理中编写调用API接口的相关逻辑
			delegate()
			{
				Debug.Log("发送发布微博请求...");
				//System.Threading.Thread.Sleep(8000); //等待8秒
				return Sina.API.Statuses.Update(string.Format("我用UniWeibo 发布了一条微博，欢迎关注@UniSocial http://unisocial.sinaapp.com Time: {0}", DateTime.Now.ToShortTimeString()));
			},
				//第二个代理为回调函数，异步完成后将自动调用这个函数来处理结果。
			delegate(AsyncCallback<UniWeibo.Entities.status.Entity> callback)
			{
				if (callback.IsSuccess)
				{
					Debug.Log( "发布微博成功" + callback.Data.CreatedAt);
				}
				else
				{
					Debug.Log("发布微博失败，异常:" + callback.Error);
				}
			}
			 );
		}
		catch (WeiboException ex)
		{
			Debug.Log("出错啦！"+ex.Message);
		}
		
	}	
#endregion
	

	void OnGUI(){
		// setting
		GUI.Label (new Rect (10, 40, 50, 20), "AppKey");
		AppKey = GUI.TextField (new Rect (70, 40, 200, 20), AppKey, 50);
		GUI.Label (new Rect (10, 80, 50, 20), "AppSecrect");
		AppSecrect = GUI.TextField (new Rect (70, 80, 200, 20), AppSecrect, 50);
		GUI.Label (new Rect (10, 120, 50, 20), "CallbackUrl");
		CallbackUrl = GUI.TextField (new Rect (70, 120, 200, 20), CallbackUrl, 50);
		
		// login
		if(!hasLogin){
			GUI.Label (new Rect (350, 300, 50, 20), "用户名：");
			UserName = GUI.TextField (new Rect (400, 300, 200, 30), UserName, 50);
			GUI.Label (new Rect (350, 350, 50, 20), "密码：");
			Password = GUI.TextField (new Rect (400, 350, 200, 30), Password, 50);
			
			if(GUI.Button(new Rect(420, 400, 100, 30), "登录")){
				Login();
				hasLogin = true;
			}
		}
		// test api
		else{
			// 获取uid
			if(GUI.Button(new Rect(350, 200, 200, 40), "获取uid")){
				TestAccount();
			}
			// 异步获取uid
			if(GUI.Button(new Rect(570, 200, 200, 40), "异步获取uid")){
				TestAccountAsync();
			}			
			// 发微博
			if(GUI.Button(new Rect(350, 250, 200, 40), "发微博")){
				TestStatuses();
			}
			// 异步发微博
			if(GUI.Button(new Rect(570, 250, 200, 40), "异步发微博")){
				TestStatusesAsync();
			}				
			// 上传图片并发微博
			if(GUI.Button(new Rect(350, 300, 200, 40), "上传图片并发微博")){
				TestUpload();
			}		
			// 异步上传图片并发微博
			if(GUI.Button(new Rect(570, 300, 200, 40), "异步上传图片并发微博")){
				TestUploadAsync();
			}			
		}
	}

}
