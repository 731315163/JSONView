using System.IO;
using System;
using UnityEngine;
public static class OperateFile 
    {
      
        public static void CreateNewFile(string path,string info)
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path );
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
                
            }
            else
            {
                //如果此文件存在则删除
                DeleteFile(path);
                sw = t.CreateText();
            }
            //写入信息
          sw.WriteLine(info);
        //关闭流
         sw.Close();
            //销毁流
            sw.Dispose();
        }
        public static  string LoadFile(string path)
        {
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path );
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                //路径与名称未找到文件则直接返回空
                return null;
            }
            string line = sr.ReadToEnd();

            //关闭流
            sr.Close();
            //销毁流
            sr.Dispose();
            //将数组链表容器返回
            return line;
        }
    public static  void DeleteFile(string path)
        {
            File.Delete(path);

        }
       
    }