using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using FullSerializer;
public class SaveController  {
  
    public delegate void SaveDelegate();
    public static event SaveDelegate OnSaveEvent;
    public delegate void LoadDelegate();
    public static event LoadDelegate OnLoadEvent;
    private static readonly fsSerializer _serializer = new fsSerializer();
    public string directoryName = "Saves";
    private string fileName;
    public void FireSaveEvent()
    {
        if(OnSaveEvent!=null)
             OnSaveEvent();
       
    }
    public void FireLoadEvent()
    {
        if (OnLoadEvent != null)
            OnLoadEvent();
     
    }
   
    public void SaveData(string path, object obj)
    {
        
        FireSaveEvent();
        string json = Serialize(obj.GetType(), obj);
        string directory = Path.Combine(Application.persistentDataPath, directoryName);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        fileName = Path.Combine(directory, path);
        StreamWriter sw = File.CreateText(fileName); // if file doesnt exist, make the file in the specified path
        sw.Close();
        File.WriteAllText(fileName, json);
        
        
  
    }
    public object LoadData(string path, object obj)
    {
        string directory = Path.Combine(Application.persistentDataPath, directoryName);
        fileName = Path.Combine(directory, path);
        if (!File.Exists(fileName)) return 0;
        string inside = File.ReadAllText(fileName);
        return Deserialize(obj.GetType(), inside);
       
    }
    public static string Serialize(Type type, object value)
    {
        // serialize the data
        fsData data;
        _serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

        // emit the data via JSON
        return fsJsonPrinter.CompressedJson(data);
    }

    public static object Deserialize(Type type, string serializedState)
    {
        // step 1: parse the JSON data
        fsData data = fsJsonParser.Parse(serializedState);

        // step 2: deserialize the data
        object deserialized = null;
        _serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

        return deserialized;
    }
    
}

