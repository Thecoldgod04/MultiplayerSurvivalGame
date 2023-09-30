using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    string folderName { get; }

    bool encrypted { get; }

    string fileName { get; }

    IDataService saveLoadService { get; }

    bool IsLoadedByManager { get; }

    void SaveRPC();

    void LoadRPC();

    void Save(string fileName);

    bool Load(string fileName);
}
