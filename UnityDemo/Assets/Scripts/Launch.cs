﻿using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Scorpio;
using Scorpio.Userdata;
public class DelegateFactory : DelegateTypeFactory
{
    public Delegate CreateDelegate(Type type, ScriptFunction func)
    {
        if (type == typeof(UIEventListener.VoidDelegate))
            return new UIEventListener.VoidDelegate((go) => { func.call(go); });
        return null;
    }
}
public class Launch : MonoBehaviour {
    public static Script Script;
    public GameObject obj;
	void Start () {
        try
        {
            DefaultScriptUserdataDelegateType.SetFactory(new DelegateFactory());
            List<string> scripts = new List<string>();
            scripts.Add("window");
            Script script = new Script();
            Launch.Script = script;
            script.LoadLibrary();
            script.PushAssembly(typeof(int).Assembly);
            script.PushAssembly(typeof(GameObject).Assembly);
            script.PushAssembly(GetType().Assembly);
            script.SetObject("print", new ScorpioFunction(Print));
            for (int i = 0; i < scripts.Count; ++i)
            {
                script.LoadString(scripts[i], (Resources.Load(scripts[i]) as TextAsset).text);
            }
            Util.AddComponent(obj, (ScriptTable)script.GetValue("window"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Stack : " + Script.GetStackInfo());
            Debug.LogError("Start is error " + ex.ToString());
        }
	}
    object Print(object[] args)
    {
        for (int i = 0; i < args.Length; ++i)
        {
            Debug.Log(args[i].ToString());
        }
        return null;
    }
}
