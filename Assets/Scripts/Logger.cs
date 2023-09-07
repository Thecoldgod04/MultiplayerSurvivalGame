using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    public static void LogError(string loggerName)
    {
        Debug.LogError("[" + loggerName + "]: Something is wrong or missing in the code!");
    }

    public static void LogError(string loggerName, string message)
    {
        Debug.LogError("[" + loggerName + "]: " + message);
    }
}
