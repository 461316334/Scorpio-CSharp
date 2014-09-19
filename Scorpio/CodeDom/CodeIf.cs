﻿using System;
using System.Collections.Generic;
using System.Text;
using Scorpio.CodeDom.Temp;

namespace Scorpio.CodeDom
{
    public class CodeIf : CodeObject
    {
        internal TempCondition If;
        internal TempCondition Else;
        internal List<TempCondition> ElseIf = new List<TempCondition>();
        internal void AddElseIf(TempCondition con)
        {
            ElseIf.Add(con);
        }
    }
}
