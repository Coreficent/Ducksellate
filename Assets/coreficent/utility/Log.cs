using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log
{
    public static void Output(object input)
    {
        Debug.Log("Debug::" + input);
    }

}
