﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Userdata
{
    public class DefaultScriptUserdataFactory : IScriptUserdataFactory
    {
        public ScriptUserdata create(Script script, object obj)
        {
            Type type = obj as Type;
            if (type != null)
            {
                if (Util.IsEnum(type))
                    return new DefaultScriptUserdataEnum(script, type);
                else if (Util.IsDelegate(type))
                    return new DefaultScriptUserdataDelegateType(script, type);
            }
            if (obj is Delegate)
                return new DefaultScriptUserdataDelegate(script, (Delegate)obj);
            return new DefaultScriptUserdataObject(script, obj);
        }
    }
}
