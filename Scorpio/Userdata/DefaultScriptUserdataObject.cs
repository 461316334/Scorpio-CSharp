﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Scorpio;
using Scorpio.Variable;
using Scorpio.Exception;
namespace Scorpio.Userdata
{
    /// <summary> 普通Object类型 </summary>
    public class DefaultScriptUserdataObject : ScriptUserdata
    {
        private UserdataType m_Type;
        public DefaultScriptUserdataObject(Script script, object value, UserdataType type) : base(script)
        {
            this.Value = value;
            this.ValueType = value.GetType();
            this.m_Type = type;
        }
        public override ScriptObject GetValue(object key)
        {
            if (!(key is string)) throw new ExecutionException(Script, "Object GetValue只支持String类型");
            return Script.CreateObject(m_Type.GetValue(Value, (string)key));
        }
        public override void SetValue(object key, ScriptObject value)
        {
            if (!(key is string)) throw new ExecutionException(Script, "Object SetValue只支持String类型");
            m_Type.SetValue(Value, (string)key, value);
        }
    }
}
