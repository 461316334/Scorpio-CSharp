﻿using System;
using System.Collections.Generic;
using System.Text;
namespace Scorpio.Exception
{
    //执行代码异常
    class ExecutionException : ScriptException
    {
        private string m_Source = "";
        public ExecutionException(String strMessage) : base(strMessage) { }
        public ExecutionException(Script script, String strMessage) : base(strMessage) {
            StackInfo stackInfo = script.GetCurrentStackInfo();
            m_Source = stackInfo.Breviary + ":" + stackInfo.Line + " : ";
        }
        public override string Message { get { return m_Source + base.Message; } }
    }
}
