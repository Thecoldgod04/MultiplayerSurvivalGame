using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class SaveLoadService : IDataService
{
    public bool SaveData<T>(string folderName, string fileName, T data, bool encrypted)
    {
        string directory = Application.persistentDataPath + "/" + folderName;

        if (!Directory.Exists(directory))
        {
            //Debug.Log("Creating stats folder...");
            Directory.CreateDirectory(directory);
        }
        
        string path = directory + "/" + fileName + ".json";

        try
        {
            if (File.Exists(path))
            {
                Debug.LogError("Data exists. Deleting old file and writing a new one!");
                File.Delete(path);
            }
            else
            {
                Debug.LogError("Writing newly created file!");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Can't save data due to: " + e.Message + " " + e.StackTrace);
            return false;
        }
    }

    public T LoadData<T>(string relativePath, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        if(!File.Exists(path))
        {
            Debug.LogError("Can't load file at: " + path);
            throw new FileNotFoundException(path + " does not exist!");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError("Can't load data due to: " + e.Message + " " + e.StackTrace);
            throw e;
        }
    }

    public string ReadFile(string relativePath, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            Debug.LogError("Can't load file at: " + path);
            throw new FileNotFoundException(path + " does not exist!");
        }

        try
        {
            string data = File.ReadAllText(path);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Can't load data due to: " + e.Message + " " + e.StackTrace);
            throw e;
        }
    }
}
