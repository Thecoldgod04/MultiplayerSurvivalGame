using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataService 
{
    bool SaveData<T>(string directory, string fileName, T data, bool encrypted);

    T LoadData<T>(string relativePath, bool encrypted);

    string ReadFile(string relativePath, bool encrypted);
}
