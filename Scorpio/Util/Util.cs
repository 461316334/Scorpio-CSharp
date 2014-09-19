﻿using System;
using System.Collections.Generic;
using System.Text;
using Scorpio.Collections;
namespace Scorpio
{
    public static class Util
    {
        public static bool SetObject(VariableDictionary variables, string str, ScriptObject obj)
        {
            if (!variables.ContainsKey(str)) return false;
            Set_impl(variables, str, obj);
            return true;
        }
        public static void AssignObject(VariableDictionary variables, string str, ScriptObject obj)
        {
            if (!variables.ContainsKey(str))
            {
                variables.Add(str, obj);
                return;
            }
            Set_impl(variables, str, obj);
        }
        private static void Set_impl(VariableDictionary variables, string str, ScriptObject obj)
        {
            ScriptObject el = variables[str];
            if (el.Type != obj.Type) {
                variables[str] = obj;
            } else {
                if (el.IsNull || el.IsNumber || el.IsString) {
                    el.Assign(obj);
                } else {
                    variables[str] = obj;
                }
            }
        }
    }
}
