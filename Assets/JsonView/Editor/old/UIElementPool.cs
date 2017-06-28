using UnityEngine;
using System.Collections.Generic;

 class UIElementPool  {
    private static UIElementPool instance;
    public static UIElementPool Instance {
        get
        {
            if(instance != null) { return instance; }
            else { instance = new UIElementPool(); return instance; }
        }
    }
    private  Stack<UIElement> stack = new Stack<UIElement>();
    public static UIElement Pop(string name, bool flag,UIElement fatherobj,object obj,int level)
    {
        var stack = Instance.stack;
        if(stack.Count > 0)
        {   
            UIElement ret = stack.Pop();
            ret.name = name;
            ret.flag = flag;
            ret.father = fatherobj;
            ret.obj = obj;
            ret.level = level;
            return ret;
        }
        else
        {
            return new UIElement(name,flag,fatherobj,obj,level);
        }
    }
   
   
    public static void Push(UIElement ui)
    {
        if (ui == null) return;
        var stack = Instance.stack;
        stack.Push(ui);
    }
    public static void Recover(List<UIElement> list)
    {
        if (list == null) return;
        var stack = Instance.stack;
        for (int i = 0; i< list.Count; ++i)
        {
            stack.Push(list[i]);
        }
        list.Clear();
    }
}
