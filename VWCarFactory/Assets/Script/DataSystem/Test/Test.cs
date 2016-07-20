using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        try
        {
            throw (new System.Exception("aaaa"));
        }
        catch (System.Exception ex)
        {
            foreach (var item in ex.Data.Keys)
            {
                Debug.Log(item);
            }
            Debug.Log(ex.InnerException);
            Debug.Log(ex.Message);
            Debug.Log(ex.Source);
            Debug.Log(ex.StackTrace);
            Debug.Log(ex.TargetSite);

        }

        //MainData data = new MainData
        //{
        //    Name = "大众改装厂",
        //    FactoryIntroduction = "车厂介绍文字",
        //    Url = " ",
        //    CarList = new System.Collections.Generic.List<string>(),
        //    Sample = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<MainData.CarSample>>()
        //};

        //data.CarList.Add("CarData1.json");
        //data.CarList.Add("CarData2.json");
        //data.Sample.Add("SUV", new System.Collections.Generic.List<MainData.CarSample>());
        //data.Sample["SUV"].Add(new MainData.CarSample { Title = "title1 ", Tag = "tag1 ", Description = "des1 ", Image = " " });
        //data.Sample["SUV"].Add(new MainData.CarSample { Title = "title2 ", Tag = "tag2 ", Description = "des2 ", Image = " " });
        //data.Sample["SUV"].Add(new MainData.CarSample { Title = "title3 ", Tag = "tag3 ", Description = "des3 ", Image = " " });
        //data.Sample.Add("CAR", new System.Collections.Generic.List<MainData.CarSample>());
        //data.Sample["CAR"].Add(new MainData.CarSample { Title = "title1 ", Tag = "tag1 ", Description = "des1 ", Image = " " });
        //data.Sample["CAR"].Add(new MainData.CarSample { Title = "title2 ", Tag = "tag2 ", Description = "des2 ", Image = " " });
        //data.Sample["CAR"].Add(new MainData.CarSample { Title = "title3 ", Tag = "tag3 ", Description = "des3 ", Image = " " });
        //Debug.Log(JsonMapper.ToJson(data));

        //MainJsonData data2 = new MainJsonData();
        //data2 = JsonMapper.ToObject<MainJsonData>(File.ReadAllText(Application.streamingAssetsPath + "/Data/MainData.json"));
        //Debug.Log(data2.Name);
        //Debug.Log(data2.FactoryIntroduction);
        //Debug.Log(data2.Url);
        //foreach (var item in data2.CarList)
        //{
        //    Debug.Log(item);
        //}
        //foreach (var item in data2.Sample)
        //{
        //    foreach (var o in item.Value)
        //    {
        //        Debug.Log(item.Key + "  " + o.Title + " " + o.Tag + " " + o.Description + " " + o.Image);
        //    }
        //}

        //Debug.Log(AppData.GetMainData.Sample.Keys.Count);
        foreach (var item in AppData.GetSampleKeys)
        {
            Debug.Log(item);
        }


    }

    // Update is called once per frame
    void Update () {
	
	}
}
