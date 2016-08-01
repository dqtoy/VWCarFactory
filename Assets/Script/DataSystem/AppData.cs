///0723 CarData 添加CarBody字段  车架路径
///添加DeleteCustumCar删除自定义车型数据，GetUserCustumCarsList获取自定义车型数据
///GetTemplateCar改成GetTemplateCarList， GetTemplateCar获取模板数据GetTemplateCarList获取模版车列表

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;


public class AppData
{
    #region 公有变量
    /// <summary>
    /// 主数据
    /// </summary>
    public static MainJsonData GetMainData { get { return m_MainJsonData; } }
    /// <summary>
    /// 车型案例
    /// </summary>
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
    /// <summary>
    /// 车款式列表
    /// </summary>
    public static List<string> CarList { get { return m_MainJsonData.CarList; } }


    #endregion

    #region 私有变量
    static string m_DataPath;
    static MainJsonData m_MainJsonData;
    static List<string> m_SampleKeys;
    static Dictionary<string, CarData> m_carsData;

    static string m_typePainting = "涂装", m_typeElectronicEquipment = "电子设备", m_typeInterior = "内饰", m_typeExterior = "外饰";
    #endregion

    static AppData()
    {
        try
        {
            m_DataPath = Application.streamingAssetsPath + "/Data";
            string _dataText = File.ReadAllText(m_DataPath + "/MainData.json");
            m_MainJsonData = JsonMapper.ToObject<MainJsonData>(_dataText);
            m_carsData = new Dictionary<string, CarData>();
            foreach (var _carName in CarList)
            {
                m_carsData.Add(_carName, GetCarDataFromFile(_carName));
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("错误：" + ex.Message + "/r/n" + ex.StackTrace);
        }
        
    }


    #region 私有函数
    static CarData GetCarDataFromFile(string __name)
    {
        CarData _carData = new CarData();

        try
        {
            _carData = JsonMapper.ToObject<CarData>(File.ReadAllText(m_DataPath + "/" + __name + ".json"));
        }
        catch (System.Exception ex)
        {
            Debug.Log("错误：" + ex.Message + "/r/n" + ex.StackTrace);
            return null;
        }
        return _carData;
    }

    #endregion

    #region 公有函数
    /// <summary>
    /// 获取指定车型的所有案例
    /// </summary>
    /// <param name="__key">车型</param>
    /// <returns></returns>
    public static List<CarSample> GetCarSamples(string __key)
    {
        List<CarSample> _sample = new List<CarSample>();
        if (GetMainData.Sample.TryGetValue(__key,out _sample))
        {
            return _sample;
        }
        else
        {
            Debug.LogError("指定的车的案例不存在：" + __key);
            return _sample;
        }
    }

    /// <summary>
    /// 获取指定车的数据
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static CarData GetCarDataByName(string __name)
    {
        CarData _carData = new CarData();
        if(m_carsData.TryGetValue(__name,out _carData))
        {
            return _carData;
        }
        else
        {
            Debug.LogError("指定的车数据不存在：" + __name);
            return null;
        }
    }

    /// <summary>
    /// 获取车的所有涂装
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static List<CarPart> GetCarPaintingByName(string __carName)
    {
        CarData _carData = new CarData();
        List<CarPart> _custumBodyTexture = new List<CarPart>();
        if (m_carsData.TryGetValue(__carName, out _carData))
        {
            foreach (var item in _carData.CustumParts)
            {
                if (item.CustumType == m_typePainting)
                {
                    _custumBodyTexture.Add(item);
                }
            }
            return _custumBodyTexture;
        }
        else
        {
            Debug.LogError("指定的车数据不存在：" + __carName);
            return _custumBodyTexture;
        }
    }

    /// <summary>
    /// 获取车配件列表
    /// </summary>
    /// <param name="__name"></param>
    /// <param name="__partName"></param>
    /// <returns></returns>
    public static List<CarPart> GetCarPartsByName(string __carName,string __partType)
    {
        CarData _carData = new CarData();
        List<CarPart> _custumParts = new List<CarPart>();
        if (m_carsData.TryGetValue(__carName, out _carData))
        {
            foreach (var item in _carData.CustumParts)
            {
                if (item.CustumType == __partType)
                {
                    _custumParts.Add(item);
                }
            }
            return _custumParts;
        }
        else
        {
            Debug.LogError("指定的车数据不存在：" + __carName);
            return _custumParts;
        }
    }

    /// <summary>
    /// 找到指定的车配件
    /// </summary>
    /// <param name="__carType">车类型</param>
    /// <param name="__partName">配件名</param>
    /// <returns>没找到返回空</returns>
    public static CarPart GetCarPartData(string __carType,string __partName)
    {
        CarData _carData = new CarData();
        if (m_carsData.TryGetValue(__carType, out _carData))
        {
            foreach (var item in _carData.CustumParts)
            {
                if (item.Name == __partName)
                {
                    return item;
                }
            }
            Debug.LogError("没找到指定的车配件");
            return null;
        }
        else
        {
            Debug.LogError("指定的车数据不存在：" + __carType);
            return null;
        }
    }

    /// <summary>
    /// 获取内置的车模板
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static List<string> GetTemplateCarList(string __carName)
    {
        CarData _carData = new CarData();
        if (m_carsData.TryGetValue(__carName, out _carData))
        {
            return _carData.TemplateCar;
        }
        else
        {
            Debug.LogError("指定的车数据不存在：" + __carName);
            return new List<string>();
        }
    }

    /// <summary>
    /// 获取内置的车模板
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static void GetTemplateCar(string __templateName)
    {
        CarStudio.LoadTemplate(__templateName);
    }


    /// <summary>
    /// 获取所有的用户自定义车名字
    /// </summary>
    /// <returns></returns>
    public static List<string> GetUserCustumCarsList()
    {
        List<string> _custumCars = new List<string>();
        string[] _file = Directory.GetFiles(CustumCar.Path, "*.json");

        for (int i = 0; i < _file.Length; i++)
        {
            _custumCars.Add(Path.GetFileNameWithoutExtension(_file[i]));
        }

        return _custumCars;
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="__name"></param>
    public static void DeleteCustumCar(string __name)
    {
        string _fileName = CustumCar.Path + "/" + __name + ".json";
        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }
    }

    #endregion
}

#region 数据
public class CarData
{
    public string Name, Introduction, Type, CarBody;
    public List<CarPart> CustumParts;
    public List<string> TemplateCar;
}

/// <summary>
/// 车零部件
/// </summary>
public class CarPart
{
    public string Name;
    /// <summary>
    /// 按钮图标
    /// </summary>
    public string Icon;
    /// <summary>
    /// 零部件模型路径
    /// </summary>
    public string ModelPath;
    /// <summary>
    /// 车配件所属的改装类别 “涂装，电子配件，内饰，外饰等”
    /// </summary>
    public string CustumType;
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
#endregion