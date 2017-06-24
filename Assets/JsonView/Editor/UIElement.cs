using UnityEngine;
using System.Collections;

public class UIElement
{  
    //Fileds
    public string name = "";
    public bool flag = false;
    public bool extend = false;
    public object obj;
    public UIElement father;
    public int level;
    public Rect rect;
    public UIElement(string name, bool flag, UIElement fatherobj, object obj, int level)
    {
        this.name = name;
        this.flag = flag;
        this.father = fatherobj;
        this.obj = obj;
        this.level = level;
    }
    public UIElement(string name, UIElement fatherobj, object obj, int level)
    {
        this.name = name;
        this.father = fatherobj;
        this.obj = obj;
        this.level = level;
    }
    public UIElement(bool flag, UIElement fatherobj, object obj, int level)
    {

        this.flag = flag;
        this.father = fatherobj;
        this.obj = obj;
        this.level = level;
    }
    public UIElement(UIElement fatherobj, object obj, int level)
    {
        this.father = fatherobj;
        this.obj = obj;
        this.level = level;
    }
    // Protiles
    public bool Active() {
       
            if (father != null  )
            {

            if (father.flag == true && father.extend == true) { this.flag = true; return true; }
                else { this.flag = false; return false; }
               
            }
            else if( flag ==true && extend == true )
            {               
                return true;
            }
            else
            {              
                return false;
            }
        
    }
}