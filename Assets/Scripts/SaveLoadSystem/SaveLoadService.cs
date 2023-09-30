using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

public class SaveLoadService : IDataService
{
    private const string KEY = "fGVIHpGub4r9TKnXvD2GIuGZO706i97S/1WeHc38uAo=";
    private const string IV = "XRIIrwJi8Vc3MPmO3ceggw==";
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

            if(encrypted)
            {
                WriteEncryptedData(data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Can't save data due to: " + e.Message + " " + e.StackTrace);
            return false;
        }
    }

    private void WriteEncryptedData<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            stream,
            cryptoTransform,
            CryptoStreamMode.Write
        );

        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

        //Debug.LogError("KEY: " + Convert.ToBase64String(aesProvider.Key));
        //Debug.LogError("IV: " + Convert.ToBase64String(aesProvider.IV));
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
            T data;
            
            if(encrypted)
            {
                data = ReadEncryptedData<T>(path);
            }
            else
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError("Can't load data due to: " + e.Message + " " + e.StackTrace);
            throw e;
        }
    }

    private T ReadEncryptedData<T>(string path)
    {
        byte[] fileBytes = File.ReadAllBytes(path);
        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            aesProvider.Key,
            aesProvider.IV
        );

        using MemoryStream decryptionStream = new MemoryStream(fileBytes);

        using CryptoStream cryptoStream = new CryptoStream(
            decryptionStream,
            cryptoTransform,
            CryptoStreamMode.Read
        );

        using StreamReader reader = new StreamReader(cryptoStream);

        string result = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<T>(result);
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
            string data;
            
            if(encrypted)
            {
                byte[] fileBytes = File.ReadAllBytes(path);
                using Aes aesProvider = Aes.Create();

                aesProvider.Key = Convert.FromBase64String(KEY);
                aesProvider.IV = Convert.FromBase64String(IV);

                using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
                    aesProvider.Key,
                    aesProvider.IV
                );

                using MemoryStream decryptionStream = new MemoryStream(fileBytes);

                using CryptoStream cryptoStream = new CryptoStream(
                    decryptionStream,
                    cryptoTransform,
                    CryptoStreamMode.Read
                );

                using StreamReader reader = new StreamReader(cryptoStream);

                string result = reader.ReadToEnd();

                data = result;
            }
            else
            {
                data = File.ReadAllText(path);
            }
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Can't load data due to: " + e.Message + " " + e.StackTrace);
            throw e;
        }
    }
}
