using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class SharingiOSBridge
{

	#if UNITY_IPHONE
	
	[DllImport("__Internal")]
	private static extern void _TAG_Share (string[] iosPath, string message,string subject,int numArray, string[] excludeActivities, int numExcludeArray);

	public static void Share (string[] imagePath, string message,string subject,int numArray, string[] excludeActivities, int numExcludeArray)
	{
		_TAG_Share (imagePath, message,subject, numArray,excludeActivities,numExcludeArray);
	}
	
	#endif
}
