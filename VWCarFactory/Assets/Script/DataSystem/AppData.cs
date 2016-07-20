using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;


public class AppData
{
    #region 公有变量
    public static MainJsonData GetMainData { get { return m_MainJsonData; } }
    public static List<string> GetSampleKeys
    {
        get
        {
            if (m_SampleKeys == null)
            {
                m_SampleKeys = new List<string>();
                var enumerator = AppData.GetMainData.Sample.Keys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    m_SampleKeys.Add(enumerator.Current);
                }
            }
            return m_SampleKeys;
        }
    }

    #endregion

    #region 私有变量
    static string m_DataPath;
    static MainJsonData m_MainJsonData;
    static List<string> m_SampleKeys;
    #endregion

    static AppData()
    {
        try
        {
            m_DataPath = Application.streamingAssetsPath + "/Data";
            string _dataText = File.ReadAllText(m_DataPath + "/MainData.json");
            m_MainJsonData = JsonMapper.ToObject<MainJsonData>(_dataText);
        }
        catch (System.Exception ex)
        {
            Debug.Log("错误：" + ex.Message + "/r/n" + ex.StackTrace);
        }
        
    }


    #region 私有函数


    #endregion

    #region 公有函数
    /// <summary>
    /// 获取指定车型的所有案例
    /// </summary>
    /// <param name="__key">车型</param>
    /// <returns></returns>
    public static List<CarSample> GetCarSamples(string __key)
    {
        return GetMainData.Sample[__key];
    }

    #endregion
}


public class CarData
{
    public string Name, Introduction, Type;
    public List<string> CustumBodyTexture;
    public Dictionary<string, CarPart> CustumParts;
}

/// <summary>
/// 主数据
/// </summary>
public class MainJsonData
{
    public string Name, FactoryIntroduction, Url;
    /// <summary>
    /// 车数据文件列表
    /// </summary>
    public List<string> CarList;
    public Dictionary<string, List<CarSample>> Sample;
}

/// <summary>
/// 车零部件
/// </summary>
public class CarPart
{
    /// <summary>
    /// 零部件模型路径
    /// </summary>
    public string ModelPath;
    /// <summary>
    /// 车配件所属的改装类别 “电子配件，内饰，外饰等”
    /// </summary>
    public string CustumType;
}

/// <summary>
/// 车型案例
/// </summary>
public class CarSample
{
    /// <summary>
    /// Title
    /// </summary>
    public string Title;
    /// <summary>
    /// tag
    /// </summary>
    public string Tag;
    /// <summary>
    /// 描述
    /// </summary>
    public string Description;
    /// <summary>
    /// 案例图片目录
    /// </summary>
    public string Image;
}
