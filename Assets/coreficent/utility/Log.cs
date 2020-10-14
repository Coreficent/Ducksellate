using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log
{
    public static bool DEBUG = false;
    public static void Output(params object[] inputs)
    {
        string output = "";
        foreach (object input in inputs)
        {
            output += "::";
            output += input;
        }
        Debug.Log("Debug::" + output);
    }

}
