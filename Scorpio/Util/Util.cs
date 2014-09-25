﻿using System;
using System.Collections.Generic;
using System.Text;
using Scorpio.Collections;
using Scorpio.Variable;
namespace Scorpio
{
    public static class Util
    {
        private static readonly Type TYPE_BOOL = typeof(bool);
        private static readonly Type TYPE_STRING = typeof(string);
        private static readonly Type TYPE_SBYTE = typeof(sbyte);
        private static readonly Type TYPE_BYTE = typeof(byte);
        private static readonly Type TYPE_SHORT = typeof(short);
        private static readonly Type TYPE_USHORT = typeof(ushort);
        private static readonly Type TYPE_INT = typeof(int);
        private static readonly Type TYPE_UINT = typeof(uint);
        private static readonly Type TYPE_LONG = typeof(long);
        private static readonly Type TYPE_ULONG = typeof(ulong);
        private static readonly Type TYPE_FLOAT = typeof(float);
        private static readonly Type TYPE_DOUBLE = typeof(double);
        private static readonly Type TYPE_DECIMAL = typeof(decimal);

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
            } else if (!(el is ScriptNull)) {
                if ((el is ScriptNumber && el.BranchType == obj.BranchType) || (el.IsString)) {
                    el.Assign(obj);
                } else {
                    variables[str] = obj;
                }
            }
        }
        public static bool IsBool(Type type)
        {
            return type == TYPE_BOOL;
        }
        public static bool IsString(Type type)
        {
            return type == TYPE_STRING;
        }
        public static bool IsDouble(Type type)
        {
            return type == TYPE_DOUBLE;
        }
        public static bool IsLong(Type type)
        {
            return type == TYPE_LONG;
        }
        public static bool IsULong(Type type)
        {
            return type == TYPE_ULONG;
        }
        public static bool IsNumber(Type type)
        {
            return (type == TYPE_SBYTE || type == TYPE_BYTE ||
                    type == TYPE_SHORT || type == TYPE_USHORT ||
                    type == TYPE_INT || type == TYPE_UINT ||
                    type == TYPE_FLOAT || type == TYPE_DOUBLE ||
                    type == TYPE_DECIMAL || type == TYPE_LONG || type == TYPE_ULONG);
        }
        public static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }
        public static bool IsBoolObject(object obj)
        {
            return obj is bool;
        }
        public static bool IsStringObject(object obj)
        {
            return obj is string;
        }
        public static bool IsDoubleObject(object obj)
        {
            return obj is double;
        }
        public static bool IsLongObject(object obj)
        {
            return obj is long;
        }
        public static bool IsULongObject(object obj)
        {
            return obj is ulong;
        }
        public static bool IsNumberObject(object obj)
        {
            return (obj is sbyte || obj is byte ||
                    obj is short || obj is ushort ||
                    obj is int || obj is uint ||
                    obj is float || obj is double ||
                    obj is decimal || obj is long || obj is ulong);
        }
        public static bool IsEnumObject(object obj)
        {
            return obj.GetType().IsEnum;
        }
        public static object ChangeType(ScriptObject par, Type type)
        {
            if (type.IsAssignableFrom(par.GetType())) {
                return par;
            } else if (par is ScriptNumber) {
                return type.IsEnum ? Enum.ToObject(type, ((ScriptNumber)par).ToLong()) : Convert.ChangeType(par.ObjectValue, type);
            } else {
                return par.ObjectValue;
            }
        }
        public static bool CanChangeType(ScriptObject[] pars, Type[] types)
        {
            if (pars.Length != types.Length) 
                return false;
            for (int i = 0; i < pars.Length;++i ) {
                if (!CanChangeType(pars[i], types[i]))
                    return false;
            }
            return true;
        }
        public static bool CanChangeType(ScriptObject par, Type type)
        {
            if (type.IsAssignableFrom(par.GetType())) {
                return true;
            } else if (par is ScriptString && Util.IsString(type)) {
                return true;
            } else if (par is ScriptNumber && (IsNumber(type) || IsEnum(type))) {
                return true;
            } else if (par is ScriptBoolean && IsBool(type)) {
                return true;
            } else if (par is ScriptEnum && (par as ScriptEnum).EnumType == type) {
                return true;
            } else if (par is ScriptUserdata && type.IsAssignableFrom(((ScriptUserdata)par).ValueType)) {
                return true;
            }
            return false;
        }
    }
}
