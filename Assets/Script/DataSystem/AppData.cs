//0802

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

    static string m_typeColors = "变色模块", m_typeElectronicEquipment = "电子功能改装", m_typeInterior = "内饰改装", m_typeExterior = "外饰改装", m_typeOther = "其他", m_typePrivate = "private";
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
    ///// <summary>
    ///// (弃用！)获取指定车型的所有案例
    ///// </summary>
    ///// <param name="__key">车型</param>
    ///// <returns></returns>
    //public static List<CarSample> GetCarSamples(string __key)
    //{
    //    //List<CarSample> _sample = new List<CarSample>();
    //    //if (GetMainData.Sample.TryGetValue(__key,out _sample))
    //    //{
    //    //    return _sample;
    //    //}
    //    //else
    //    //{
    //    //    Debug.LogError("指定的车的案例不存在：" + __key);
    //    //    return _sample;
    //    //}
    //    return null;
    //}


    /// <summary>
    /// 获取影片案例
    /// </summary>
    /// <param name="__car"></param>
    /// <param name="__part"></param>
    /// <returns></returns>
    public static List<CarSample> GetPartMovieSamples(string __car, string __part)
    {
        CarPart _carPart = GetCarPartData(__car, __part);
        List<CarSample> _result = new List<CarSample>();
        for (int i = 0; i < _carPart.MovieDescription.Count; i++)
        {
            _result.Add(m_MainJsonData.Sample[_carPart.MovieDescription[i]]);
        }
        return _result.Count > 0 ? _result : null;
    }
    /// <summary>
    /// 获取图片案例
    /// </summary>
    /// <param name="__car"></param>
    /// <param name="__part"></param>
    /// <returns></returns>
    public static List<CarSample> GetPartTextureSamples(string __car, string __part)
    {
        CarPart _carPart = GetCarPartData(__car, __part);
        List<CarSample> _result = new List<CarSample>();
        for (int i = 0; i < _carPart.TextureDescription.Count; i++)
        {
            _result.Add(m_MainJsonData.Sample[_carPart.TextureDescription[i]]);
        }
        return _result.Count > 0 ? _result : null;
    }

    /// <summary>
    /// 获取案例
    /// </summary>
    /// <param name="__sample"></param>
    /// <returns></returns>
    public static CarSample GetSamples(string __sample)
    {
        return m_MainJsonData.Sample[__sample];
    }

    /// <summary>
    /// 获取指定车的数据
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static CarData GetCarDataByName(string __name)
    {
        CarData _carData = new CarData();
        if (m_carsData.TryGetValue(__name, out _carData))
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
    /// 获取车配件列表
    /// </summary>
    /// <param name="__name"></param>
    /// <param name="__partName"></param>
    /// <returns></returns>
    public static List<IButtonInfo> GetCarPartsByName(string __carName, string __partType)
    {
        var _parts = GetCarAllParts(__carName);
        if (_parts.ContainsKey(__partType))
        {
            return _parts[__partType];
        }
        throw new System.Exception("不存在指定的组件类型");
    }

    /// <summary>
    /// 获取所有的配件
    /// </summary>
    /// <param name="__carName"></param>
    /// <returns></returns>
    public static Dictionary<string, List<IButtonInfo>> GetCarAllParts(string __carName)
    {
        CarData _carData = GetCarDataByName(__carName);
        Dictionary<string, List<IButtonInfo>> _result = new Dictionary<string, List<IButtonInfo>>();
        _result.Add(m_typeExterior, new List<IButtonInfo>());
        _result.Add(m_typeInterior, new List<IButtonInfo>());
        _result.Add(m_typeElectronicEquipment, new List<IButtonInfo>());
        for (int i = 0; i < _carData.CustumParts.Count; i++)
        {
            //找外饰
            if (_carData.CustumParts[i].Tag == m_typeExterior)
            {
                _result[m_typeExterior].Add(_carData.CustumParts[i]);
                continue;
            }
            //找内饰
            if (_carData.CustumParts[i].Tag == m_typeInterior)
            {
                _result[m_typeInterior].Add(_carData.CustumParts[i]);
                continue;
            }
            //找电子功能
            if (_carData.CustumParts[i].Tag == m_typeElectronicEquipment)
            {
                _result[m_typeElectronicEquipment].Add(_carData.CustumParts[i]);
                continue;
            }
        }
        return _result;
    }

    /// <summary>
    /// 获取所有的颜色列表
    /// </summary>
    /// <param name="__carName"></param>
    /// <returns></returns>
    public static List<IButtonInfo> GetCarColorsByName(string __carName)
    {
        List<IButtonInfo> _result = new List<IButtonInfo>();

        CarData _carData = GetCarDataByName(__carName);

        for (int i = 0; i < _carData.CustumParts.Count; i++)
        {
            if (_carData.CustumParts[i].Tag == m_typeColors)
            {
                _result.Add(_carData.CustumParts[i]);
            }
        }
        return _result;
        //throw new System.Exception("不存在指定的组件类型");
    }



    /// <summary>
    /// 找到指定的车配件
    /// </summary>
    /// <param name="__carType">车类型</param>
    /// <param name="__partName">配件名</param>
    /// <returns>没找到返回空</returns>
    public static CarPart GetCarPartData(string __carType, string __partName)
    {
        CarData _carData = GetCarDataByName(__carType);
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

    /// <summary>
    /// 获取车的所有涂装
    /// 字典key是类型，value是涂装业务列表
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static Dictionary<string, List<IButtonInfo>> GetPaintingTemplateByName(string __carName)
    {
        CarData _carData = GetCarDataByName(__carName);
        Dictionary<string, List<IButtonInfo>> _result = new Dictionary<string, List<IButtonInfo>>();
        List<CustumCar> _cars = new List<CustumCar>();
        for (int i = 0; i < _carData.Painting.Count; i++)
        {
            _cars.Add(GetTemplateCarData(_carData.Painting[i]));
        }//获取所有的车数据
        //将数据分类
        for (int i = 0; i < _cars.Count; i++)
        {
            if (_result.ContainsKey(_cars[i].Tag))
            {
                _result[_cars[i].Tag].Add(_cars[i]);
            }
            else
            {
                _result.Add(_cars[i].Tag, new List<IButtonInfo>());
                _result[_cars[i].Tag].Add(_cars[i]);
            }
        }
        return _result;
    }

    /// <summary>
    /// 获取内置的车模板
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static Dictionary<string, List<IButtonInfo>> GetSpecialTemplateCarList(string __carName)
    {
        CarData _carData = GetCarDataByName(__carName);
        Dictionary<string, List<IButtonInfo>> _result = new Dictionary<string, List<IButtonInfo>>();
        List<CustumCar> _cars = new List<CustumCar>();
        for (int i = 0; i < _carData.TemplateCar.Count; i++)
        {
            _cars.Add(GetTemplateCarData(_carData.TemplateCar[i]));
        }
        for (int i = 0; i < _cars.Count; i++)
        {
            if (_result.ContainsKey(_cars[i].Tag))
            {
                _result[_cars[i].Tag].Add(_cars[i]);
            }
            else
            {
                _result.Add(_cars[i].Tag, new List<IButtonInfo>());
                _result[_cars[i].Tag].Add(_cars[i]);
            }
        }
        return _result;
    }
    /// <summary>
    /// 获取内置模板数据
    /// </summary>
    /// <param name="__fileName"></param>
    /// <returns></returns>
    public static CustumCar GetTemplateCarData(string __fileName)
    {
        return CarStudio.GetTemplate(__fileName);
    }

    ///// <summary>
    ///// (弃用)获取内置的车模板0801
    ///// </summary>
    ///// <param name="__name"></param>
    ///// <returns></returns>
    //public static void GetTemplateCar(string __templateName)
    //{
    //    CarStudio.LoadTemplate(__templateName);
    //    --------------
    //}


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
public class CarData : IButtonInfo
{
    /// <summary>
    /// 车名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 暂时无用
    /// </summary>
    public string Tag { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// 车的类型CAR,SUV
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    public string PdfDescription { get; set; }

    public List<string> MovieDescription { get; set; }
    public List<string> TextureDescription { get; set; }
    /// <summary>
    /// 组件列表
    /// </summary>
    public List<CarPart> CustumParts;
    public string CNG;
    public List<string> TemplateCar;
    public List<string> Painting;

}

/// <summary>
/// 车零部件
/// </summary>
public class CarPart : IButtonInfo
{
    /// <summary>
    /// 组件名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 标签 变色模块，电子模块，内饰，外饰
    /// </summary>
    public string Tag { get; set; }
    /// <summary>
    /// 按钮图标
    /// </summary>
    public string Icon { get; set; }

    public string Type { get; set; }
    
    public string Description { get; set; }
    public string PdfDescription { get; set; }

    public List<string> MovieDescription { get; set; }
    public List<string> TextureDescription { get; set; }

    public List<Asset> Assets;



}

public class Asset
{
    public string AssetPath, Target;
}
public interface IButtonInfo
{
    string Name { get; set; }
    string Tag { get; set; }
    string Icon { get; set; }
    string Type { get; set; }
    /// <summary>
    /// 文字描述
    /// </summary>
    string Description { get; set; }
    string PdfDescription { get; set; }
    /// <summary>
    /// 电影描述
    /// </summary>
    List<string> MovieDescription { get; set; }
    /// <summary>
    /// 图片描述
    /// </summary>
    List<string> TextureDescription { get; set; }
}


/// <summary>
/// 主数据
/// </summary>
public class MainJsonData : IButtonInfo
{
    /// <summary>
    /// 组件名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Tag { get; set; }
    /// <summary>
    /// 按钮图标
    /// </summary>
    public string Icon { get; set; }

    public string Type { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    public string PdfDescription { get; set; }
    public List<string> MovieDescription { get; set; }
    public List<string> TextureDescription { get; set; }

    public string Url;
    /// <summary>
    /// 车数据文件列表
    /// </summary>
    public List<string> CarList;
    public Dictionary<string, CarSample> Sample;
}
/// <summary>
/// 车型案例
/// </summary>
public class CarSample : IButtonInfo
{
    /// <summary>
    /// 组件名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Tag { get; set; }
    /// <summary>
    /// 按钮图标
    /// </summary>
    public string Icon { get; set; }

    public string Type { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    public string PdfDescription { get; set; }

    public List<string> MovieDescription { get; set; }
    public List<string> TextureDescription { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title;


    /// <summary>
    /// 资源目录
    /// </summary>
    public string Asset;
    public string AssetType;
}
#endregion