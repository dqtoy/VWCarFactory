

[UniWeibo]

UniWeibo是新浪微博的Unity插件;纯c#实现，支持iOS，Android以及PC/Mac standalone。Web build因为Crossdomain privacy的原因暂不支持，正在想解决办法。

完整的文档以及技术支持请访问 http://unisocial.sinaapp.com/

特点：

1. 使用简单, 自动解析所有返回的Json文本；

2. 跨平台支持: iOS，Android，PC/Mac standalone；

3. 集成可在iOS上运行的JsonFx，你可以单独地使用它；

4. 内置一个http封装类, 支持https以及OAuth2.0，同时可以在iOS运行；
你可以单独地使用它。

5. 支持同步、异步两种调用模式；

怎样运行demo：

1. 去http://open.weibo.com创建一个新浪微博应用，创建成功后你会得到应用的App Key和App Secret.
设置应用的回调地址，必须跟你在UniWeibo调用的时候一致;；
2. 如果你的微博应用未通过审核，请确保你在UniWeibo中使用的测试用户已加到你的微博应用的测试用户列表里了，否则无法成功；

3. 在DemoScene中设置好App Key, App Secret, Callback url, User Name, 以及Password correctly,现在应该可以运行了；

如有疑问，可发e-mail至unisocial@126.com或登录官方论坛提问。（http://unisocial.sinaapp.com/?q=forum）

[UniWeibo]

UniWeibo is a Sina Weibo(the most popular social network in China) plugin for Unity;

It's all written in c#, and can run both on iOS, Android and PC/Mac standalone;
Web build is not supported yet because of crossdomain privacy; 

Full documation and tech support please visit http://unisocial.sinaapp.com/

Features:

1. Easy to use, auto deserialize returned json for you.

2. Crossplatform support: iOS，Android，PC/Mac standalone;

3. Integrated with JsonFx which still works on iOS, you can use it separately;

4. Integrated with a http wrapper, which support https and OAuth2.0 ,works on iOS;
you can use it separately;

5. Both synchronous and asynchronous mode support;

How to run demo:

1. Go to http://open.weibo.com and create an App.You will get an App Key and App Secret.
And set your Callback url correct. It must be the same as is used in UniWeibo;

2. If your Sina Weibo App havn't passed review, you need to add the user you used in demo to App's testing user list;

3. Set App Key, App Secret, Callback url, User Name, and Password correctly, then the demo should run.

Please e-mail to unisocial@126.com or visit our forum http://unisocial.sinaapp.com/?q=forum, 
if you have question or suggestion.