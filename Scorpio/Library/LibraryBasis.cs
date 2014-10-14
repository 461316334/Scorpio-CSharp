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
        private class ArrayPairs : ScorpioHandle
        {
            Script m_Script;
            List<ScriptObject>.Enumerator m_Enumerator;
            int m_Index = 0;
            ScriptTable m_Table;
            public ArrayPairs(Script script, ScriptArray obj)
            {
                m_Script = script;
                m_Index = 0;
                m_Table = m_Script.CreateTable();
                m_Enumerator = obj.GetIterator();
            }
            public object Call(ScriptObject[] args)
            {
                if (m_Enumerator.MoveNext())
                {
                    m_Table.SetValue("key", m_Script.CreateObject(m_Index++));
                    m_Table.SetValue("value", m_Enumerator.Current);
                    return m_Table;
                }
                return null;
            }
        }
        private class ArrayKPairs : ScorpioHandle
        {
            Script m_Script;
            List<ScriptObject>.Enumerator m_Enumerator;
            int m_Index = 0;
            public ArrayKPairs(Script script, ScriptArray obj)
            {
                m_Script = script;
                m_Index = 0;
                m_Enumerator = obj.GetIterator();
            }
            public object Call(ScriptObject[] args)
            {
                if (m_Enumerator.MoveNext())
                    return m_Index++;
                return null;
            }
        }
        private class ArrayVPairs : ScorpioHandle
        {
            Script m_Script;
            List<ScriptObject>.Enumerator m_Enumerator;
            public ArrayVPairs(Script script, ScriptArray obj)
            {
                m_Script = script;
                m_Enumerator = obj.GetIterator();
            }
            public object Call(ScriptObject[] args)
            {
                if (m_Enumerator.MoveNext())
                    return m_Enumerator.Current;
                return null;
            }
        }
        private class TablePairs : ScorpioHandle
        {
            Script m_Script;
            TableDictionary.Enumerator m_Enumerator;
            ScriptTable m_Table;
            public TablePairs(Script script, ScriptTable obj)
            {
                m_Script = script;
                m_Table = m_Script.CreateTable();
                m_Enumerator = obj.GetIterator();
            }
            public object Call(ScriptObject[] args)
            {
                if (m_Enumerator.MoveNext())
                {
                    KeyValuePair<object, ScriptObject> v = m_Enumerator.Current;
                    m_Table.SetValue("key", m_Script.CreateObject(v.Key));
                    m_Table.SetValue("value", v.Value);
                    return m_Table;
                }
                return null;
            }
        }
        private class TableKPairs : ScorpioHandle
        {
            Script m_Script;
            TableDictionary.Enumerator m_Enumerator;
            public TableKPairs(Script script, ScriptTable obj)
            {
                m_Script = script;
                m_Enumerator = obj.GetIterator();
            }
            public object Call(ScriptObject[] args)
            {
                if (m_Enumerator.MoveNext())
                    return m_Enumerator.Current.Key;
                return null;
            }
        }
        private class TableVPairs : ScorpioHandle
        {
            Script m_Script;
            TableDictionary.Enumerator m_Enumerator;
            public TableVPairs(Script script, ScriptTable obj)
            {
                m_Script = script;
                m_Enumerator = obj.GetIterator();
            }
            public object Call(ScriptObject[] args)
            {
                if (m_Enumerator.MoveNext())
                    return m_Enumerator.Current.Value;
                return null;
            }
        }
        private class UserdataPairs : ScorpioHandle
        {
            Script m_Script;
            System.Collections.IEnumerator m_Enumerator;
            public UserdataPairs(Script script, ScriptUserdata obj)
            {
                m_Script = script;
                object value = obj.Value;
                System.Collections.IEnumerable ienumerable = value as System.Collections.IEnumerable;
                if (ienumerable == null) throw new ExecutionException("pairs 只支持继承 IEnumerable 的类");
                m_Enumerator = ienumerable.GetEnumerator();
            }
            public object Call(ScriptObject[] args)
            {
                if (m_Enumerator.MoveNext())
                    return m_Enumerator.Current;
                return null;
            }
        }
        public static void Load(Script script)
        {
            script.SetObjectInternal("print", script.CreateFunction(new print()));
            script.SetObjectInternal("pairs", script.CreateFunction(new pairs(script)));
            script.SetObjectInternal("kpairs", script.CreateFunction(new kpairs(script)));
            script.SetObjectInternal("vpairs", script.CreateFunction(new vpairs(script)));
            script.SetObjectInternal("type", script.CreateFunction(new type()));
            script.SetObjectInternal("branchtype", script.CreateFunction(new branchtype()));
            script.SetObjectInternal("typeof", script.CreateFunction(new userdatatype()));
            script.SetObjectInternal("tonumber", script.CreateFunction(new tonumber(script)));
            script.SetObjectInternal("tolong", script.CreateFunction(new tolong(script)));
            script.SetObjectInternal("tostring", script.CreateFunction(new tostring(script)));
            script.SetObjectInternal("clone", script.CreateFunction(new clone()));
            script.SetObjectInternal("load_assembly", script.CreateFunction(new load_assembly(script)));
            script.SetObjectInternal("load_assembly_safe", script.CreateFunction(new load_assembly_safe(script)));
            script.SetObjectInternal("import_type", script.CreateFunction(new import_type(script)));
        }
        private class print : ScorpioHandle
        {
            public object Call(ScriptObject[] args)
            {
                for (int i = 0; i < args.Length; ++i) {
                    Console.WriteLine(args[i].ToString());
                }
                return null;
            }
        }
        private class pairs : ScorpioHandle
        {
            private Script m_script;
            public pairs(Script script)
            {
                m_script = script;
            }
            public object Call(ScriptObject[] args)
            {
                ScriptObject obj = args[0];
                if (obj is ScriptArray)
                    return m_script.CreateFunction(new ArrayPairs(m_script, (ScriptArray)obj));
                else if (obj is ScriptTable)
                    return m_script.CreateFunction(new TablePairs(m_script, (ScriptTable)obj));
                else if (obj is ScriptUserdata)
                    return m_script.CreateFunction(new UserdataPairs(m_script, (ScriptUserdata)obj));
                throw new ExecutionException("pair必须用语table或array类型");
            }
        }
        private class kpairs : ScorpioHandle
        {
            private Script m_script;
            public kpairs(Script script)
            {
                m_script = script;
            }
            public object Call(ScriptObject[] args)
            {
                ScriptObject obj = args[0];
                if (obj is ScriptArray)
                    return m_script.CreateFunction(new ArrayKPairs(m_script, (ScriptArray)obj));
                else if (obj is ScriptTable)
                    return m_script.CreateFunction(new TableKPairs(m_script, (ScriptTable)obj));
                throw new ExecutionException("pair必须用语table或array类型");
            }
        }
        private class vpairs : ScorpioHandle
        {
            private Script m_script;
            public vpairs(Script script)
            {
                m_script = script;
            }
            public object Call(ScriptObject[] args)
            {
                ScriptObject obj = args[0];
                if (obj is ScriptArray)
                    return m_script.CreateFunction(new ArrayVPairs(m_script, (ScriptArray)obj));
                else if (obj is ScriptTable)
                    return m_script.CreateFunction(new TableVPairs(m_script, (ScriptTable)obj));
                throw new ExecutionException("pair必须用语table或array类型");
            }
        }
        private class type : ScorpioHandle
        {
            public object Call(ScriptObject[] args)
            {
                return args[0].Type;
            }
        }
        private class branchtype : ScorpioHandle
        {
            public object Call(ScriptObject[] args)
            {
                return args[0].BranchType;
            }
        }
        private class userdatatype : ScorpioHandle
        {
            public object Call(ScriptObject[] args)
            {
                return ((ScriptUserdata)args[0]).ValueType;
            }
        }
        private class tonumber : ScorpioHandle
        {
            private Script m_script;
            public tonumber(Script script)
            {
                m_script = script;
            }
            public object Call(ScriptObject[] args)
            {
                ScriptObject obj = args[0];
                if (obj is ScriptNumber || obj is ScriptString || obj is ScriptEnum)
                    return m_script.CreateNumber(Util.ToDouble(obj.ObjectValue));
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
            public object Call(ScriptObject[] args)
            {
                ScriptObject obj = args[0];
                if (obj is ScriptNumber || obj is ScriptString || obj is ScriptEnum)
                    return m_script.CreateNumber(Util.ToInt64(obj.ObjectValue));
                throw new ExecutionException("不能从类型 " + obj.Type + " 转换成Long类型");
            }
        }
        private class tostring : ScorpioHandle
        {
            private Script m_script;
            public tostring(Script script)
            {
                m_script = script;
            }
            public object Call(ScriptObject[] args)
            {
                ScriptObject obj = args[0];
                if (obj is ScriptString) return obj;
                return m_script.CreateString(obj.ToString());
            }
        }
        private class clone : ScorpioHandle
        {
            public object Call(ScriptObject[] args)
            {
                return args[0].Clone();
            }
        }
        private class load_assembly : ScorpioHandle
        {
            private Script m_script;
            public load_assembly(Script script)
            {
                m_script = script;
            }
            public object Call(ScriptObject[] args)
            {
                ScriptString str = args[0] as ScriptString;
                if (str == null) throw new ExecutionException("load_assembly 参数必须是 string");
                m_script.PushAssembly(Assembly.Load(str.Value));
                return null;
            }
        }
        private class load_assembly_safe : ScorpioHandle
        {
            private Script m_script;
            public load_assembly_safe(Script script)
            {
                m_script = script;
            }
            public object Call(ScriptObject[] args)
            {
                try {
                    ScriptString str = args[0] as ScriptString;
                    if (str == null) throw new ExecutionException("load_assembly 参数必须是 string");
                    m_script.PushAssembly(Assembly.Load(str.Value));
                } catch (System.Exception ) { }
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
            public object Call(ScriptObject[] args)
            {
                ScriptString str = args[0] as ScriptString;
                if (str == null) throw new ExecutionException("import_type 参数必须是 string");
                return m_script.LoadType(str.Value);
            }
        }
    }
}
