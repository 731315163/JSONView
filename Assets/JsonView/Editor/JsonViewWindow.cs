using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Collections;
using MiniJSON;
using System.Text;

class JsonView : EditorWindow
{ 
    //json element
    List<UIElement> uielement = new List<UIElement>();
    // serization json dictionary or json list
    object serization;
    //scroll view postion
    Vector2 scrollPos;
    // reflection 
    List<Type> Types = new List<Type>();
    List<string> TypesFullName = new List<string>();
    int lastSelectType = 0;
    int selectType = 0;
    string lastTypeName = "";
    string typeName = "";
    string savefilepath;
    string openfilepath;

    int Width = 30;
    int Height = 15;


    #region 菜单栏
    [MenuItem("JsonView/JsonWindow")]
	static void  Init() {
        var window = GetWindow(typeof( JsonView));
        window.position =new Rect(0, 0, 800, 600);
        window.Show();
        
    }
    #endregion
    #region 序列化 与反序列
    void StartToUIList(string filepath)
    {
        this.serization = null;
        UIElementPool.Recover(uielement);
        this.serization = Deserialize(OperateFile.LoadFile(openfilepath));
        if(this.serization == null) {
        Debug.LogError(" Deserizaliztion fail "); Debug.LogError("序列化文件失败"); }
        if (this.serization is IDictionary<string, object>) {
          
            DicToUIList("", null, this.serization as IDictionary<string, object>, true, 0);
        }
        else if(this.serization is List<object>)
        {
            ListToUIList("", null, this.serization as List<object>, true, 0);
        }
    }
    string Serialize(object obj)
    {
        return JsonUtility.ToJson(obj);
    }
    object Deserialize(string str)
    {
         return  Json.Deserialize(str);
      
    }
    object SecondDeserialize(object obj)
    {
        return Deserialize(Serialize(obj));
    }
    #endregion
    #region 所有显示元素的纵队

    void DicToUIList(string name, UIElement fatherobj, IDictionary<string,object> dic ,bool flag ,int level)
    {
        int childlevel = level+1;
        UIElement dictarget = UIElementPool.Pop(name, flag, fatherobj, dic, level);          
        uielement.Add(dictarget);
       
        foreach(var item in dic.Keys)
        {
            if(dic[item] is IDictionary<string,object>)
            {
                DicToUIList(item, dictarget, dic[item] as IDictionary<string, object>, false,childlevel) ;
            }
           else  if(dic[item] is List<object>)
            {
                ListToUIList(item, dictarget, dic[item] as List<object>,  false,childlevel);
            }
            else
            {
                DotObjectToUIList(item, dictarget,  dic[item],childlevel);
            }
        }
    }
    void DicInsertUIList(string name,UIElement fatherobj, IDictionary<string, object> dic, bool flag, int index)
    {   
        
        int childlevel = fatherobj.level + 1;
        int nextindex = index + 1;
        UIElement dictarget = UIElementPool.Pop(name, flag, fatherobj, dic, childlevel);
        uielement.Insert(index,dictarget);

        foreach (var item in dic.Keys)
        {
            if (dic[item] is IDictionary<string, object>)
            {
                DicInsertUIList(item, dictarget, dic[item] as IDictionary<string, object>, false, nextindex);
            }
            else if (dic[item] is List<object>)
            {
                ListInsertUIList(item, dictarget, dic[item] as List<object>, false, nextindex);
            }
            else
            {
                DotObjectInsertUIList(item, dictarget, dic[item], nextindex);
            }
        }
    }
    void ListToUIList(string name, UIElement fatherobj,List<object> list , bool flag, int level)
    {
        int childlevel = level+1;
        UIElement listelement =  UIElementPool.Pop(name, flag, fatherobj, list, level);
        uielement.Add(listelement);
        for(int i = 0; i < list.Count; ++i)
        {
            if(list[i] is List<object>)
            {
                ListToUIList(i.ToString(), listelement, list[i] as List<object>,  false,childlevel);
            }
            else if(list[i] is IDictionary<string, object>)
            {
                DicToUIList(i.ToString(), listelement, list[i] as IDictionary<string, object>, false,childlevel);
            }
            else
            {
                DotObjectToUIList(i.ToString(), listelement, list[i],  childlevel);
            }
        }
    }
    void ListInsertUIList(string name, UIElement fatherobj, List<object> list, bool flag, int index)
    {
        int nextindex = index + 1; 
        UIElement listelement = UIElementPool.Pop(name, flag, fatherobj, list, fatherobj.level +1);
        uielement.Insert(index,listelement);
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] is List<object>)
            {
                ListInsertUIList(i.ToString(), listelement, list[i] as List<object>, false, nextindex);
            }
            else if (list[i] is IDictionary<string, object>)
            {
                DicInsertUIList(i.ToString(), listelement, list[i] as IDictionary<string, object>, false,nextindex);
            }
            else
            {
                DotObjectInsertUIList(i.ToString(), listelement, list[i], nextindex);
            }
        }
    }
    void DotObjectToUIList(string name,UIElement fatherobj,  object obj, int level)
    {
        uielement.Add( UIElementPool.Pop(name,false,fatherobj,obj, level));

    }
    void DotObjectInsertUIList(string name, UIElement fatherobj, object obj, int index)
    {
        uielement.Insert(index,UIElementPool.Pop(name, false, fatherobj, obj, fatherobj.level+1));

    }
    void ReToUIList( object serization,object UIobj)
    {
      
           UIElementPool.Recover(uielement);
        if(serization is IDictionary<string, object>)
        {
            DicToUIList("", null, serization as IDictionary<string,object>, true, 0);
        }
        else if (this.serization is List<object>)
        {
            ListToUIList("", null, this.serization as List<object>, true, 0);
        }
        if (UIobj == null) return;
        for (int i =0; i < uielement.Count; ++i)
            {
                if(uielement[i].obj == UIobj) {
                    if(UIobj is List<object> || UIobj is IDictionary<string, object>)
                    {
                        uielement[i].flag = true;
                        uielement[i].extend = true;
                    }
                    else
                    {
                        uielement[i].flag = true;
                    }
                    UIElement father = uielement[i].father;
                    while(father != null)
                    {
                        father.extend = true;
                        father.flag = true;
                        father = father.father;
                    }
                }
            }
        
        
    }
    void ReToUIList(object obj)
    {
        ReToUIList(obj, null);
    }
    #endregion 
    // GUI 更新函数
    void OnGUI() {
        
        DrawToolBar();
        UIElementActive(uielement);
        UIElementRect(uielement);
        DrawGenericMenu();
        Draw(uielement);

    }
    void DrawToolBar()
    {
        if(GUI.Button(new Rect(0,0, 50, 15), "Open"))
        {
           
           
            if(openfilepath == null)
            {
                openfilepath = EditorUtility.OpenFilePanel("Open Json", "", "json");
            }
            else
            {
                string[] filename = openfilepath.Split('/');
                StringBuilder dirc = new StringBuilder();
                for (int i = 0; i < filename.Length - 1; ++i)
                {
                    dirc.Append(filename[i]);
                }
                openfilepath = EditorUtility.OpenFilePanel("Open Json", dirc.ToString(), "json");
                
            }
            StartToUIList(openfilepath);          
        }
        if (GUI.Button(new Rect(50, 0, 50, 15), "Save"))
        {
            
            if (savefilepath == null)
            {
                savefilepath = EditorUtility.SaveFilePanel("Save Json", "", ".json", "json");
            }
            else
            {
                string[] filename = savefilepath.Split( '/');
                StringBuilder dirc = new StringBuilder();
                for(int i = 0; i < filename.Length-1; ++i) {
                    dirc.Append(filename[i]);
                }
                savefilepath = EditorUtility.SaveFilePanel("Save Json", dirc.ToString(), filename[filename.Length-1], "json");
            }
            OperateFile.CreateNewFile(savefilepath, Json.Serialize( this.serization));
        }
        if(GUI.Button(new Rect(100, 0, 100, 15), "SerizeNewType"))
        {
            try
            {
                this.serization = SecondDeserialize(Activator.CreateInstance(Types[selectType]));
                UIElementPool.Recover(uielement);
                ReToUIList(this.serization);
            }
            catch(Exception e)
            {
                Debug.LogError("Class Type Error  " + e);
                Debug.LogError(" 类型错误   " + e);
              
            }
        }
        typeName = EditorGUI.TextField(new Rect(200f, 0f, 300f, 15f), "TypeName", typeName);
        selectType = EditorGUI.Popup(new Rect(500f, 0f, 300f, 15f), " SelectType", selectType, TypesFullName.ToArray());
        if (typeName == null || typeName == "") { return; }      
        else if(  lastSelectType != selectType) {
            typeName = TypesFullName[selectType];
            lastSelectType = selectType;
            lastTypeName = typeName;
            return;
        }
        else if(lastTypeName != typeName )
        {
           this.Types =  Reflection.RetType(typeName);
            TypesFullName.Clear();
            for(int i = 0; i < Types.Count; ++i)
            {
                TypesFullName.Add(Types[i].FullName);
            }
            lastTypeName = typeName;
        }        
    
    }
    void DrawGenericMenu()
    {
            Event evt = Event.current;           
            Vector2 mousePos = evt.mousePosition;           
        if (evt.type == EventType.ContextClick)
        {
            // Now create the menu, add items and show it
            //现在创建菜单，添加项目并显示
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Del"), false, Del, mousePos);
            menu.AddItem(new GUIContent("AddInt"), false, AddInt, mousePos);
            menu.AddItem(new GUIContent("AddFloat"), false, AddFloat, mousePos);
            menu.AddItem(new GUIContent("AddString"), false, AddString, mousePos);
            menu.AddItem(new GUIContent("AddCustomClass"), false, AddCustomClass, mousePos);
            menu.ShowAsContext();
            evt.Use();
        }
        
    }
    #region DrawGenericMenu 的回调函数
    void Action(Vector2 pos, Action<UIElement> objislist, Action<UIElement> fatherislist) {
        UIElement UI = GetUIElement(pos);
        if (UI.obj is List<object>)
        {
            objislist(UI);
        }
        else if (UI.father.obj is List<object>)
        {
            fatherislist(UI);
        }
        else
        {
            Debug.Log("Don't Support Key-Value Type ,This Is Support Array Type Of Adjunction And Deletion.");
            Debug.Log(" 不支持键值对的添加与删除，目前支持数组类型的添加与删除。");
        }
    }
    void AddAction(Vector2 pos, object obj)
    {
        Action(pos,
            delegate (UIElement UI) {
                List<object> list = (UI.obj as List<object>);
                list.Add(obj);
                int index = list.Count;
                DotObjectInsertUIList((index - 1).ToString(), UI, list[index - 1], uielement.IndexOf(UI) + index);
                  UI.extend = true;
                  UI.flag = true;
            },
            delegate (UIElement UI) {
                var father = (UI.father.obj as List<object>);
                int gloindex = uielement.IndexOf(UI);
                int index = int.Parse(UI.name);
                father.Insert(index, obj);
                DotObjectInsertUIList(UI.name, UI.father, father[index], gloindex);
                while (uielement[gloindex + 1].father == uielement[gloindex].father)
                {
                    gloindex++;
                    index++;
                    uielement[gloindex].name = index.ToString();
                    if (gloindex + 1 >= uielement.Count) return;
                }
            }
            );
    }
    void Del(object obj)
    {
        Action((Vector2)obj,
            delegate(UIElement UI) {
                (UI.obj as List<object>).Clear();
                if(UI.obj is List<object> || UI.obj is IDictionary<string, object>) {
                    ReToUIList(serization, UI.obj); }
                else
                {
                    ReToUIList(serization);
                  
                }
            }, delegate (UIElement UI) {
                if(UI.obj is List<object> || UI.obj is IDictionary<string, object>)
                {
                    (UI.father.obj as List<object>).Remove(UI.obj);
                    ReToUIList(serization, UI.father.obj);
                    return;
                }
                int gloindex = uielement.IndexOf(UI);
                int index = int.Parse(UI.name);
                var father = UI.father.obj as List<object>;
                uielement.Remove(UI);
                father.Remove(father[index]);
                if (index == father.Count) return;
                else
                {
                    while(uielement[ gloindex].father == UI.father)
                    {
                        uielement[gloindex].name = index.ToString();
                        gloindex++;
                        index++;
                        if (gloindex >= uielement.Count) return;

                    }
                 }
               
            }
            );

    }
    void AddInt(object obj)
    {
        AddAction((Vector2)obj, 0);
    }
    void AddFloat(object obj)
    {
        AddAction((Vector2)obj, 0.1f);
    }
    void AddString(object obj)
    {
        AddAction((Vector2)obj,"");
    }
    void AddCustomClass(object obj)
    {
        object addobj = Activator.CreateInstance(this.Types[selectType]);
        if (addobj == null) {         
            Debug.LogError("Please Select Being Class Type ");
            Debug.LogError("请选择怎正确的类型");
            return;
        }
        object secobj = SecondDeserialize(addobj);
        Action((Vector2)obj, delegate (UIElement UI) {
            (UI.obj as List<object>).Add(secobj);
            ReToUIList(serization, UI.obj);
        }, delegate (UIElement UI) {
            var father = (UI.father.obj as List<object>);
            father.Insert(father.IndexOf(UI.obj), secobj);
            ReToUIList(serization, father);
        });

    }
    #endregion
    void Draw(List<UIElement> UI)
    { 
    //滚动条
        scrollPos =
             GUI.BeginScrollView(new Rect(0, Height, position.width, position.height), scrollPos, new Rect(0, Height, position.width, UI.Count * Height));
      
        if (UI == null) return;
        for (int i = 0; i < UI.Count; i++)
        {
            if (UI[i].flag == true)
            {
                if (UI[i].obj is IDictionary<string, object> || UI[i].obj is List<object>)
                {
                    Drawobject(UI[i]);
                }
                else if (UI[i].obj is string || UI[i].obj is char)
                {
                    Drawstring(UI[i]);
                }
                else if(UI[i].obj is bool)
                {
                    Drawbool(UI[i]);
                }
                else
                {
                    Drawnumber(UI[i]);
                }
            }
        }

        GUI.EndScrollView();
    }
    void Drawobject( UIElement UI)
    {
        UI.extend = EditorGUI.Foldout(UI.rect, UI.extend, UI.name);
    }
    void Drawstring( UIElement UI )
    {

    UI.obj =    EditorGUI.TextField(UI.rect, UI.name, UI.obj as string);

    }
    void Drawbool(UIElement UI)
    {
        if (UI.father.obj is List<object>)
        {
            (UI.father.obj as List<object>)[int.Parse(UI.name)] = EditorGUI.Toggle(UI.rect, UI.name, (bool)(UI.father.obj as List<object>)[int.Parse(UI.name)] );
        }
        else if (UI.father.obj is IDictionary<string, object>)
        {
            (UI.father.obj as IDictionary<string, object>)[UI.name] = EditorGUI.Toggle(UI.rect, UI.name, (bool)(UI.father.obj as IDictionary<string, object>)[UI.name] );
        }
    }
    void Drawnumber(UIElement UI )
    {
        var value = UI.obj;
        if(value is int
                  || value is uint
                  || value is long
                  || value is sbyte
                  || value is byte
                  || value is short
                  || value is ushort
                  || value is ulong)
            {
            if (UI.father.obj is List<object>)
            {
                (UI.father.obj as List<object>)[int.Parse(UI.name)] = EditorGUI.IntField(UI.rect, UI.name, (int)(UI.father.obj as List<object>)[int.Parse(UI.name)]);
            }
            else if (UI.father.obj is IDictionary<string, object>)
            {
                (UI.father.obj as IDictionary<string, object>)[UI.name] = EditorGUI.IntField(UI.rect, UI.name, (int)(UI.father.obj as IDictionary<string, object>)[UI.name]);
            }
        }
        else if (value is double
                   || value is decimal
                   || value is float)
        {
            if (UI.father.obj is List<object>)
            {
                (UI.father.obj as List<object>)[int.Parse(UI.name)] = EditorGUI.FloatField(UI.rect, UI.name, (float)(UI.father.obj as List<object>)[int.Parse(UI.name)]);
            }
            else if (UI.father.obj is IDictionary<string, object>)
            {
                (UI.father.obj as IDictionary<string, object>)[UI.name] = EditorGUI.FloatField(UI.rect, UI.name, (float)(UI.father.obj as IDictionary<string, object>)[UI.name]);
            }
        }
    }
    void UIElementActive(List<UIElement> list) {
        if (list == null) return;      
        for(int i = 0; i < list.Count; ++i)
        {
             list[i].Active();
        }
    }
    void UIElementRect(List<UIElement> list)
    {
        int count = 1;
        for (int i = 0; i < list.Count; ++i)
        {
           
            if (list[i].flag == true)
            {
                list[i].rect = new Rect(list[i].level * Width, count * Height, position.width - 20, Height);
                count++;
            }

        }
    }
    UIElement GetUIElement(Vector2 pos)
    {  
        for(int i = 0; i < uielement.Count; ++i)
        {
            if (uielement[i].flag == true && uielement[i].rect.Contains(pos))
            {
                return uielement[i];
            }
        }
        return null;
    }
    void OnInspectorUpdate() {
        this.Repaint();
    }
}