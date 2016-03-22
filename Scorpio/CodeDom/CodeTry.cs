﻿using Scorpio;
using Scorpio.Runtime;
namespace Scorpio.CodeDom
{
    //try catch 语句
    public class CodeTry : CodeObject
    {
        private Script m_Script;
        public ScriptExecutable TryExecutable;      //try指令执行
        public ScriptExecutable CatchExecutable;    //catch指令执行
        public string Identifier;                   //异常对象
        public CodeTry(Script script) {
            m_Script = script;
        }
        public ScriptContext GetTryContext() {
            return new ScriptContext(m_Script, TryExecutable);
        }
        public ScriptContext GetCatchContext() {
            return new ScriptContext(m_Script, CatchExecutable);
        }
    }
}
