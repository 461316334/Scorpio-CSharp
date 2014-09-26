﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Scorpio;
using Scorpio.Exception;
using Scorpio.Collections;
namespace Scorpio.Library
{
    public class LibraryBasis
    {
        private class ArrayPair : ScorpioHandle
        {
            List<ScriptObject>.Enumerator m_ListEnumerator;
            public ArrayPair(ScriptObject obj)
            {
                m_ListEnumerator = ((ScriptArray)obj).GetIterator();
            }
            public object Call(object[] args)
            {
                if (m_ListEnumerator.MoveNext())
                    return m_ListEnumerator.Current;
                return null;
            }
        }
        private class TablePair : ScorpioHandle
        {
            Script m_Script;
            TableDictionary.Enumerator m_TableEnumerator;
            public TablePair(Script script, ScriptObject obj)
            {
                m_Script = script;
                m_TableEnumerator = ((ScriptTable)obj).GetIterator();
            }
            public object Call(object[] args)
            {
                if (m_TableEnumerator.MoveNext())
                {
                    KeyValuePair<object, ScriptObject> v = m_TableEnumerator.Current;
                    ScriptTable table = new ScriptTable();
                    table.SetValue("key", m_Script.CreateObject(v.Key));
                    table.SetValue("value", v.Value);
                    return table;
                }
                return null;
            }
        }
        public static void Load(Script script)
        {
            script.SetObjectInternal("print", script.CreateFunction(new print()));
            script.SetObjectInternal("pair", script.CreateFunction(new pair(script)));
            script.SetObjectInternal("tonumber", script.CreateFunction(new tonumber(script)));
            script.SetObjectInternal("tolong", script.CreateFunction(new tolong(script)));
            script.SetObjectInternal("toulong", script.CreateFunction(new toulong(script)));
            script.SetObjectInternal("tostring", script.CreateFunction(new tostring(script)));
            script.SetObjectInternal("clone", script.CreateFunction(new clone()));
            script.SetObjectInternal("load_assembly", script.CreateFunction(new load_assembly(script)));
            script.SetObjectInternal("import_type", script.CreateFunction(new import_type(script)));
        }
        private class pair : ScorpioHandle
        {
            private Script m_script;
            public pair(Script script)
            {
                m_script = script;
            }
            public object Call(object[] args)
            {
                ScriptObject obj = args[0] as ScriptObject;
                if (obj is ScriptArray)
                    return m_script.CreateFunction(new ArrayPair(obj));
                else if (obj is ScriptTable)
                    return m_script.CreateFunction(new TablePair(m_script, obj));
                throw new ExecutionException("pair必须用语table或array类型");
            }
        }
        private class print : ScorpioHandle
        {
            public object Call(object[] args)
            {
                for (int i = 0; i < args.Length; ++i) {
                    Console.WriteLine(args[i].ToString());
                }
                return null;
            }
        }
        private class tonumber : ScorpioHandle
        {
            private Script m_script;
            public tonumber(Script script)
            {
                m_script = script;
            }
            public object Call(object[] args)
            {
                ScriptObject obj = args[0] as ScriptObject;
                if (obj is ScriptNumber || obj is ScriptString)
                    return m_script.CreateNumber(Convert.ToDouble(obj.ObjectValue));
                throw new ExecutionException("不能从类型 " + obj.Type + " 转换成Number类型");
            }
        }
        private class tolong : ScorpioHandle
        {
            private Script m_script;
            public tolong(Script script)
            {
                m_script = script;
            }
            public object Call(object[] args)
            {
                ScriptObject obj = args[0] as ScriptObject;
                if (obj is ScriptNumber || obj is ScriptString)
                    return m_script.CreateNumber(Convert.ToInt64(obj.ObjectValue));
                throw new ExecutionException("不能从类型 " + obj.Type + " 转换成Long类型");
            }
        }
        private class toulong : ScorpioHandle
        {
            private Script m_script;
            public toulong(Script script)
            {
                m_script = script;
            }
            public object Call(object[] args)
            {
                ScriptObject obj = args[0] as ScriptObject;
                if (obj is ScriptNumber || obj is ScriptString)
                    return m_script.CreateNumber(Convert.ToUInt64(obj.ObjectValue));
                throw new ExecutionException("不能从类型 " + obj.Type + " 转换成ULong类型");
            }
        }
        private class tostring : ScorpioHandle
        {
            private Script m_script;
            public tostring(Script script)
            {
                m_script = script;
            }
            public object Call(object[] args)
            {
                ScriptObject obj = args[0] as ScriptObject;
                if (obj is ScriptString) return obj;
                return m_script.CreateString(obj.ToString());
            }
        }
        private class clone : ScorpioHandle
        {
            public object Call(object[] args)
            {
                return ((ScriptObject)args[0]).Clone();
            }
        }
        private class load_assembly : ScorpioHandle
        {
            private Script m_script;
            public load_assembly(Script script)
            {
                m_script = script;
            }
            public object Call(object[] args)
            {
                ScriptString str = args[0] as ScriptString;
                if (str == null) throw new ExecutionException("load_assembly 参数必须是 string");
                string assemblyName = str.Value;
                Assembly assembly = null;
                try {
                    assembly = Assembly.Load(assemblyName);
                } catch (BadImageFormatException) {
                    // The assemblyName was invalid.  It is most likely a path.
                }
                if (assembly == null) {
                    assembly = Assembly.Load(AssemblyName.GetAssemblyName(assemblyName));
                }
                m_script.PushAssembly(assembly);
                return null;
            }
        }
        private class import_type : ScorpioHandle
        {
            private Script m_script;
            public import_type(Script script)
            {
                m_script = script;
            }
            public object Call(object[] args)
            {
                ScriptString str = args[0] as ScriptString;
                if (str == null) throw new ExecutionException("import_type 参数必须是 string");
                return m_script.LoadType(str.Value);
            }
        }
    }
}
