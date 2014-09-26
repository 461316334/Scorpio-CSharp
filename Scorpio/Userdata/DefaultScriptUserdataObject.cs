﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Scorpio.Variable;
using Scorpio.Exception;
namespace Scorpio.Userdata
{
    //语言数据
    public class DefaultScriptUserdataObject : ScriptUserdata
    {
        private class Field
        {
            public string name;
            public Type fieldType;
            public FieldInfo field;
            public MethodInfo getMethod;
            public MethodInfo setMethod;
            public object GetValue(object obj)
            {
                if (field != null)
                    return field.GetValue(obj);
                else if (getMethod != null)
                    return getMethod.Invoke(obj, null);
                throw new ScriptException("变量 [" + name + "] 不支持GetValue"); 
            }
            public void SetValue(object obj, object val)
            {
                if (field != null)
                    field.SetValue(obj, val);
                else if (setMethod != null)
                    setMethod.Invoke(obj, new object[] { val });
                else
                    throw new ScriptException("变量 [" + name + "] 不支持SetValue");
            }
        }
        private ScorpioMethod m_Constructor;
        private Dictionary<string, Field> m_FieldInfos;                 //所有的变量 以及 get set函数
        private Dictionary<string, ScriptUserdata> m_NestedTypes;       //所有的类中类
        private Dictionary<string, ScriptFunction> m_Functions;         //所有的函数
        public DefaultScriptUserdataObject(Script script, object value)
        {
            this.m_Script = script;
            this.Value = value;
            this.ValueType = (Value is Type) ? (Type)value : value.GetType();
            m_FieldInfos = new Dictionary<string, Field>();
            m_NestedTypes = new Dictionary<string, ScriptUserdata>();
            m_Functions = new Dictionary<string, ScriptFunction>();
            m_Constructor = new ScorpioMethod(ValueType.ToString(), ValueType.GetConstructors());
        }
        public override ScriptObject Call(ScriptObject[] parameters)
        {
            return m_Script.CreateObject(m_Constructor.Call(parameters));
        }
        private Field GetField(string strName)
        {
            if (m_FieldInfos.ContainsKey(strName))
                return m_FieldInfos[strName];
            Field field = new Field();
            field.name = strName;
            FieldInfo info = ValueType.GetField(strName);
            if (info != null)
            {
                field.field = info;
                field.fieldType = info.FieldType;
                m_FieldInfos.Add(strName, field);
                return field;
            }
            MethodInfo method = ValueType.GetMethod("get_" + strName);
            if (method != null)
            {
                field.getMethod = method;
                field.fieldType = method.ReturnType;
                field.setMethod = ValueType.GetMethod("set_" + strName);
                m_FieldInfos.Add(strName, field);
                return field;
            }
            method = ValueType.GetMethod("set_" + strName);
            if (method != null)
            {
                field.setMethod = method;
                field.fieldType = method.GetParameters()[0].ParameterType;
                field.getMethod = ValueType.GetMethod("get_" + strName);
                m_FieldInfos.Add(strName, field);
                return field;
            }
            return null;
        }
        public override ScriptObject GetValue(string strName)
        {
            if (m_Functions.ContainsKey(strName))
                return m_Functions[strName];
            if (m_NestedTypes.ContainsKey(strName))
                return m_NestedTypes[strName];
            Field field = GetField(strName);
            if (field != null) return m_Script.CreateObject(field.GetValue(Value));
            MethodInfo method = ValueType.GetMethod(strName);
            if (method != null)
            {
                ScriptFunction func = m_Script.CreateFunction(new ScorpioMethod(ValueType, strName, Value));
                m_Functions[strName] = func;
                return func;
            }
            Type nestedType = ValueType.GetNestedType(strName);
            if (nestedType != null) {
                ScriptUserdata ret = m_Script.CreateUserdata(nestedType);
                m_NestedTypes.Add(strName, ret);
                return ret;
            }
            throw new ScriptException("Type[" + ValueType.ToString() + "] Variable[" + strName + "] 不存在");
        }
        public override void SetValue(string strName, ScriptObject value)
        {
            Field field = GetField(strName);
            if (field == null) throw new ScriptException("Type[" + ValueType + "] 变量 [" + strName + "] 不存在");
            field.SetValue(Value, Util.ChangeType(value, field.fieldType));
        }
    }
}
