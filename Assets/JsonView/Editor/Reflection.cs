using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Reflection;

public class Reflection {

    private Assembly[] Assemblys;
    private List< Type> Types;
    private static Reflection instance;
    public static Reflection Instance
    {
        get
        {
            if(instance != null) { return instance; }
            else { instance = new Reflection();return instance; }
        }
    }
   private Reflection()
    {
        Assemblys = AppDomain.CurrentDomain.GetAssemblies();
        Types = new List<Type>();
        for (int i = 0; i < Assemblys.Length; ++i)
        {
            Types.AddRange(Assemblys[i].GetTypes());
        }

    }
    public static List< Assembly> RetAssembly(string str)
    {
        var ret = new List<Assembly>();
        for(int i = 0;i < Instance.Assemblys.Length; ++i)
        {
            if (Regex.IsMatch(Instance.Assemblys[i].FullName,  "^"+str))
            {
                ret.Add(Instance.Assemblys[i]);
            }
        }       
        return ret ;
    }
    public static List< Type> RetType(string classname)
    {
        var ret = new List<Type>();
        for(int i = 0; i <Instance.Types.Count; ++i)
                if (Regex.IsMatch(Instance.Types[i].Name, "^"+classname,RegexOptions.IgnoreCase))
                {
                    ret.Add(Instance.Types[i]);
                }
        return ret;
    }
            
}
