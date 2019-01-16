using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;


public class AutoBuildCode
{
    [MenuItem("生成/创建或刷新界面")]
    public static void BuildUIScript()
    {
        //确定命名规则,并保存
        var dicUIType = new Dictionary<string, string>();
        dicUIType.Add("Img", "Image");
        dicUIType.Add("Btn", "Button");
        dicUIType.Add("Txt", "Text");
        dicUIType.Add("Tran", "Transform");
        dicUIType.Add("Input", "InputField");
        dicUIType.Add("Scroll", "ScrollRect");
        dicUIType.Add("Scrollbar", "Scrollbar");
        GameObject[] selectObjs = Selection.gameObjects;
        foreach (GameObject go in selectObjs)
        {
            //string panelName = go.name;
            //if (panelName.IndexOf("Panel") == -1)
            //{
            //    Debug.LogError("BuildUIScript is Called. but panelName.IndexOf(Panel) == -1");
            //    return;
            //}

            //if (panelName.Substring(panelName.IndexOf("Panel")) != "Panel")
            //{
            //    Debug.LogError("BuildUIScript is Called. (panelName.Substring(panelName.IndexOf(Panel)) != Panel)");
            //    return;
            //}

            //transform.root  返回最高级
            GameObject selectObj = go.transform.gameObject;

            //获取点击的所有子物体
            Transform[] _trans = selectObj.GetComponentsInChildren<Transform>(true);
            //保存所有子物体
            List<Transform> childList = new List<Transform>(_trans);

            //找到所有符合dicUIType的obj
            var mainNode = from trans in childList
                           where trans.name.Contains('_') &&
dicUIType.Keys.Contains(trans.name.Split('_')[0])
                           select trans;

            //保存每个符合条件的obj所对应的path
            var nodePathList = new Dictionary<string, string>();
            List<string> CollectEqualName = new List<string>();
            bool isErr = false;

            foreach (Transform node in mainNode)
            {
                try
                {
                    Transform tempNode = node;
                    string nodePath = "/" + tempNode.name;
                    while (tempNode != selectObj.transform) //补充该obj在selectObj下的完整路径
                    {
                        tempNode = tempNode.parent;
                        int index = nodePath.IndexOf('/');
                        if (tempNode.name != selectObj.name)
                        {
                            nodePath = nodePath.Insert(index, "/" + tempNode.name);
                        }
                        else
                        {
                            nodePath = nodePath.Substring(nodePath.IndexOf('/') + 1);
                        }
                    }

                    if (nodePathList.ContainsKey(node.name))
                    {
                        isErr = true;
                        Debug.LogError(string.Format("同名同路径。请检查! 名称:{0},路径:{1}", node.name, nodePath));
                        break;
                    }
                    nodePathList.Add(node.name, nodePath);
                }
                catch (Exception e)
                {
                    isErr = true;
                    Debug.LogError(string.Format("Err:{0}--------Exception:{1}",node,e));
                }
            }

            if (isErr)
            {
                Debug.LogError("有错误。请检查报错");
                return;
            }

            //创建变量
            string memberstring = "";

            //初始化UI相关属性（Btn，Txt等）
            string loadedcontant = "";

            //按钮监听回调
            string function = "";

            //补充
            string supplement = "";

            //添加按钮监听
            string addListenerString = "";

            //按钮集合
            List<string> btns = new List<string>();

            Dictionary<string, string> btnDic = new Dictionary<string, string>();

            //创建并初始化变量
            foreach (Transform itemtran in mainNode)
            {
                string typeStr = dicUIType[itemtran.name.Split('_')[0]];

                if (typeStr == "Button")
                {
                    if (!btns.Contains(itemtran.name))
                    {
                        btns.Add(itemtran.name);
                    }
                    else
                    {
                        isErr = true;
                        Console.WriteLine("按钮名称重复。请检查：{0}", itemtran.name);
                        break;
                    }
                }

                memberstring += "private " + typeStr + " " + itemtran.name + " = null;\r\n\t";  //根据既定的类型创建对应变量类型

                //根据既定的类型找到该obj的GetComponent
                loadedcontant += itemtran.name + " = " + "gameObject.transform.Find(\"" + nodePathList[itemtran.name] + "\").GetComponent<" + typeStr + ">();\r\n\t\t";
            }

            if (isErr)
            {
                Debug.LogError("有错误。请检查报错");
                return;
            }

            loadedcontant += "AddClicks();";

            foreach (var btn in btns)
            {
                string funcName = "On" + btn.Split('_')[1] + "Click";
                string aloneFunc = "private void " + funcName + "(){\r\n\t} \r\n\t";
                function += aloneFunc;
                btnDic[btn] = "private void " + funcName + "()"; //存储方法
                addListenerString += btn + ".onClick.AddListener(" + funcName + ");\r\n\t";
            }
            
            //code创建路径
            string scriptPath = Application.dataPath + "/Scripts/" + selectObj.name + ".cs";

            string classStr = "";
            
            if (File.Exists(scriptPath))
            {
                FileStream classFile = new FileStream(scriptPath, FileMode.Open);
                StreamReader read = new StreamReader(classFile);
                classStr = read.ReadToEnd();
                read.Close();
                classFile.Close();
                File.Delete(scriptPath);

                string splitStart = "//auto";
                string splitEnd = "//autoEnd";
                string splitMid = "//defaultFcuntion";

                foreach (var btn in btnDic)
                {
                    bool isMatch = Regex.IsMatch(classStr, btn.Value);
                    if (!isMatch)
                    {
                        supplement += btn.Value + "{\r\n\t} \r\n\t";
                    }
                    supplement = "\r\n" + supplement;

                }

                //截取splitStart以上部分内容(即所有using)
                string unChangeUsing = Regex.Split(classStr, splitStart, RegexOptions.IgnoreCase)[0];

                //截取中间部分//Fcuntion
                string unChangeDefaultFucn =
                    new Regex("(?<=(" + splitMid + "))[.\\s\\S]*?(?=(" + splitEnd + "))",
                        RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(
                            classStr).Value;

                //截取splitEnd以下部分内容（即新添加的方法）
                string unChangeAddFuc = Regex.Split(classStr, splitEnd, RegexOptions.IgnoreCase)[1];

                //正则表达式来查找匹配splitStart  splitMid之间的所有字符串
                Regex rg = new Regex("(?<=(" + splitStart + "))[.\\s\\S]*?(?=(" + splitMid + "))", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                
                //读取需要改变的部分
                string changeStr = rg.Match(AutoBuildCodeModel.UIClass).Value;

                //创建类成员及using
                StringBuilder build = new StringBuilder();
                build.Append(unChangeUsing);
                build.Append(splitStart);
                build.Append(changeStr);
                build.Append(splitMid);
                build.Append(supplement);//添加补充部分
                build.Append(unChangeDefaultFucn);
                build.Append(splitEnd);
                build.Append(unChangeAddFuc);
                classStr = build.ToString();
            }
            else
            {
                classStr = AutoBuildCodeModel.UIClass;
            }
            //创建按钮回调方法
            classStr = classStr.Replace("#CallBack#", function);
            //创建类名
            classStr = classStr.Replace("#类名#", selectObj.name);
            //创建方法内成员
            classStr = classStr.Replace("#查找#", loadedcontant);
            //创建全局变量
            classStr = classStr.Replace("#成员#", memberstring);
            //添加按钮监听事件
            classStr = classStr.Replace("#AddListener#", addListenerString);
            //////添加按钮监听事件
            //classStr = classStr.Replace("#supplement#", supplement);


            //写入完成
            FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
            StreamWriter fileSW = new StreamWriter(file, System.Text.Encoding.UTF8);
            fileSW.Write(classStr);
            fileSW.Flush();
            fileSW.Close();
            file.Close();


            Debug.LogError("创建脚本" + Application.dataPath + "/Scripts/" + selectObj.name
                + ".cs成功！");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
    }
}
