﻿using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;
using Scorpio.Exception;
namespace Scorpio.Compiler
{
    /// <summary> 脚本语法解析 </summary>
    public class ScriptLexer
    {
        public enum LexState
        {
            /// <summary> 没有关键字 </summary>
            None,
            /// <summary> = 等于或者相等 </summary>
            AssignOrEqual,
            /// <summary> / 注释或者除号 </summary>
            CommentOrDivideOrAssignDivide,
            /// <summary> 行注释 </summary>
            LineComment,
            /// <summary> 区域注释开始 </summary>
            BlockCommentStart,
            /// <summary> 区域注释结束 </summary>
            BlockCommentEnd,
            /// <summary> + 或者 ++ 或者 += </summary>
            PlusOrIncrementOrAssignPlus,
            /// <summary> - 或者 -= </summary>
            MinusOrDecrementOrAssignMinus,
            /// <summary> * 或者 *= </summary>
            MultiplyOrAssignMultiply,
            /// <summary> % 或者 %= </summary>
            ModuloOrAssignModulo,
            /// <summary> & 并且 </summary>
            And,
            /// <summary> | 或者 </summary>
            Or,
            /// <summary> ! 非或者不等于 </summary>
            NotOrNotEqual,
            /// <summary> > 大于或者大于等于 </summary>
            GreaterOrGreaterEqual,
            /// <summary> < 小于或者小于等于 </summary>
            LessOrLessEqual,
            /// <summary> " 字符串 </summary>
            String,
            /// <summary> "\ 换行字符串 </summary>
            StringEscape,
            /// <summary> 数字 </summary>
            Number,
            /// <summary> 描述符 </summary>
            Identifier,
        }
        private LexState m_lexState;
        private LexState lexState {
            get { return m_lexState; }
            set { m_lexState = value; if (m_lexState == LexState.None) { m_strToken = ""; } }
        }
        private String m_strToken = null;
        private int m_iSourceLine;
        private int m_iSourceChar;
        private List<String> m_listSourceLines = new List<String>();
        public ScriptLexer(String buffer)
        {
            String strSource = buffer.Replace("\r\n", "\r");
            string[] strLines = strSource.Split('\r');
            foreach (String strLine in strLines)
                m_listSourceLines.Add(strLine + "\r\n");
            m_iSourceLine = 0;
            m_iSourceChar = 0;
            lexState = LexState.None;
        }
        /// <summary> 获得整段字符串的摘要 </summary>
        public String GetBreviary()
        {
            if (m_listSourceLines.Count > 0)
                return m_listSourceLines[0];
            return "";
        }
        bool EndOfSource { get { return m_iSourceLine >= m_listSourceLines.Count; } }
        bool EndOfLine { get { return m_iSourceChar >= m_listSourceLines[m_iSourceLine].Length; } }
        char ReadChar()
        {
            if (EndOfSource)
                throw new LexerException("End of source reached.", m_iSourceLine);
            char ch = m_listSourceLines[m_iSourceLine][m_iSourceChar++];
            if (m_iSourceChar >= m_listSourceLines[m_iSourceLine].Length)
            {
                m_iSourceChar = 0;
                ++m_iSourceLine;
            }
            return ch;
        }
        void UndoChar()
        {
            if (m_iSourceLine == 0 && m_iSourceChar == 0)
                throw new LexerException("Cannot undo char beyond start of source.", m_iSourceLine);
            --m_iSourceChar;
            if (m_iSourceChar < 0)
            {
                --m_iSourceLine;
                m_iSourceChar = m_listSourceLines[m_iSourceLine].Length - 1;
            }
        }
        void IgnoreLine()
        {
            ++m_iSourceLine;
            m_iSourceChar = 0;
        }
        void ThrowInvalidCharacterException(char ch)
        {
            throw new ScriptException("Unexpected character [" + ch + "]  Line:" + (m_iSourceLine + 1) +" Column:" + m_iSourceChar + " [" + m_listSourceLines[m_iSourceLine] + "]");
        }
        public List<Token> GetTokens()
        {
            m_iSourceLine = 0;
            m_iSourceChar = 0;
            lexState = LexState.None;
            List<Token> listTokens = new List<Token>();
            while (!EndOfSource)
            {
                if (EndOfLine)
                {
                    IgnoreLine();
                    continue;
                }
                char ch = ReadChar();
                switch (lexState)
                {
                    case LexState.None:
                        switch (ch)
                        {
                            case ' ':
                            case '\t':
                            case '\n':
                            case '\r':
                                break;
                            case '(':
                                listTokens.Add(new Token(TokenType.LeftPar, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case ')':
                                listTokens.Add(new Token(TokenType.RightPar, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case '[':
                                listTokens.Add(new Token(TokenType.LeftBracket, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case ']':
                                listTokens.Add(new Token(TokenType.RightBracket, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case '{':
                                listTokens.Add(new Token(TokenType.LeftBrace, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case '}':
                                listTokens.Add(new Token(TokenType.RightBrace, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case ',':
                                listTokens.Add(new Token(TokenType.Comma, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case ':':
                                listTokens.Add(new Token(TokenType.Colon, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case ';':
                                listTokens.Add(new Token(TokenType.SemiColon, ch, m_iSourceLine, m_iSourceChar));
                                break;
                            case '+':
                                lexState = LexState.PlusOrIncrementOrAssignPlus;
                                break;
                            case '-':
                                lexState = LexState.MinusOrDecrementOrAssignMinus;
                                break;
                            case '*':
                                lexState = LexState.MultiplyOrAssignMultiply;
                                break;
                            case '/':
                                lexState = LexState.CommentOrDivideOrAssignDivide;
                                break;
                            case '%':
                                lexState = LexState.ModuloOrAssignModulo;
                                break;
                            case '.':
                                listTokens.Add(new Token(TokenType.Period, ".", m_iSourceLine, m_iSourceChar));
                                break;
                            case '=':
                                lexState = LexState.AssignOrEqual;
                                break;
                            case '&':
                                lexState = LexState.And;
                                break;
                            case '|':
                                lexState = LexState.Or;
                                break;
                            case '!':
                                lexState = LexState.NotOrNotEqual;
                                break;
                            case '>':
                                lexState = LexState.GreaterOrGreaterEqual;
                                break;
                            case '<':
                                lexState = LexState.LessOrLessEqual;
                                break;
                            case '\"':
                                lexState = LexState.String;
                                break;
                            default:
                                if (ch == '_' || char.IsLetter(ch)) {
                                    lexState = LexState.Identifier;
                                    m_strToken = "" + ch;
                                } else if (char.IsDigit(ch)) {
                                    lexState = LexState.Number;
                                    m_strToken = "" + ch;
                                } else {
                                    ThrowInvalidCharacterException(ch);
                                }
                                break;
                        }
                        break;
                    case LexState.PlusOrIncrementOrAssignPlus:
                        if (ch == '+') {
                            listTokens.Add(new Token(TokenType.Increment, "++", m_iSourceChar, m_iSourceChar));
                            lexState = LexState.None;
                        } else if (ch == '=') {
                            listTokens.Add(new Token(TokenType.AssignPlus, "+=", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Plus, "+", m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.MinusOrDecrementOrAssignMinus:
                        if (ch == '-') {
                            listTokens.Add(new Token(TokenType.Decrement, "--", m_iSourceChar, m_iSourceChar));
                            lexState = LexState.None;
                        } else if (ch == '=') {
                            listTokens.Add(new Token(TokenType.AssignMinus, "-=", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Minus, "-", m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.MultiplyOrAssignMultiply:
                        if (ch == '=') {
                            listTokens.Add(new Token(TokenType.AssignMultiply, "*=", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Multiply, "*", m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.CommentOrDivideOrAssignDivide:
                        switch (ch) {
                            case '/':
                                lexState = LexState.LineComment;
                                break;
                            case '*':
                                lexState = LexState.BlockCommentStart;
                                break;
                            case '=':
                                listTokens.Add(new Token(TokenType.AssignDivide, "/=", m_iSourceLine, m_iSourceChar));
                                lexState = LexState.None;
                                break;
                            default:
                                listTokens.Add(new Token(TokenType.Divide, ch, m_iSourceLine, m_iSourceChar));
                                UndoChar();
                                lexState = LexState.None;
                                break;
                        }
                        break;
                    case LexState.ModuloOrAssignModulo:
                        if (ch == '=') {
                            listTokens.Add(new Token(TokenType.AssignModulo, "%=", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Modulo, "%", m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.LineComment:
                        if (ch == '\n')
                            lexState = LexState.None;
                        break;
                    case LexState.BlockCommentStart:
                        if (ch == '*')
                            lexState = LexState.BlockCommentEnd;
                        break;
                    case LexState.BlockCommentEnd:
                        if (ch == '/')
                            lexState = LexState.None;
                        else
                            lexState = LexState.BlockCommentStart;
                        break;
                    case LexState.AssignOrEqual:
                        if (ch == '=') {
                            listTokens.Add(new Token(TokenType.Equal, "==", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Assign, "=", m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.And:
                        if (ch == '&') {
                            listTokens.Add(new Token(TokenType.And, "&&", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            ThrowInvalidCharacterException(ch);
                        }
                        break;
                    case LexState.Or:
                        if (ch == '|') {
                            listTokens.Add(new Token(TokenType.Or, "||", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            ThrowInvalidCharacterException(ch);
                        }
                        break;
                    case LexState.NotOrNotEqual:
                        if (ch == '=') {
                            listTokens.Add(new Token(TokenType.NotEqual, "!=",m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Not, "!",m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.GreaterOrGreaterEqual:
                        if (ch == '=') {
                            listTokens.Add(new Token(TokenType.GreaterOrEqual, ">=", m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Greater, ">", m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.LessOrLessEqual:
                        if (ch == '=') {
                            listTokens.Add(new Token(TokenType.LessOrEqual, "<=",m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else {
                            listTokens.Add(new Token(TokenType.Less, "<",m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.String:
                        if (ch == '\"') {
                            listTokens.Add(new Token(TokenType.String, m_strToken, m_iSourceLine, m_iSourceChar));
                            lexState = LexState.None;
                        } else if (ch == '\\') {
                            lexState = LexState.StringEscape;
                        } else if (ch == '\r' || ch == '\n') {
                            ThrowInvalidCharacterException(ch);
                        } else {
                            m_strToken += ch;
                        }
                        break;
                    case LexState.StringEscape:
                        if (ch == '\\' || ch == '\"') {
                            m_strToken += ch;
                            lexState = LexState.String;
                        } else if (ch == 't') {
                            m_strToken += '\t';
                            lexState = LexState.String;
                        } else if (ch == 'r') {
                            m_strToken += '\r';
                            lexState = LexState.String;
                        } else if (ch == 'n') {
                            m_strToken += '\n';
                            lexState = LexState.String;
                        } else {
                            ThrowInvalidCharacterException(ch);
                        }
                        break;
                    case LexState.Number:
                        if (char.IsDigit(ch)) {
                            m_strToken += ch;
                        } else {
                            double value = double.Parse(m_strToken);
                            listTokens.Add(new Token(TokenType.Number, value, m_iSourceLine, m_iSourceChar));
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                    case LexState.Identifier:
                        if (ch == '_' || char.IsLetterOrDigit(ch)) {
                            m_strToken += ch;
                        } else {
                            TokenType tokenType;
                            if (m_strToken == "global")
                                tokenType = TokenType.Global;
                            else if (m_strToken == "var" || m_strToken == "local")
                                tokenType = TokenType.Var;
                            else if (m_strToken == "if")
                                tokenType = TokenType.If;
                            else if (m_strToken == "else")
                                tokenType = TokenType.Else;
                            else if (m_strToken == "elseif" || m_strToken == "elif")
                                tokenType = TokenType.ElseIf;
                            else if (m_strToken == "while")
                                tokenType = TokenType.While;
                            else if (m_strToken == "for")
                                tokenType = TokenType.For;
                            else if (m_strToken == "foreach")
                                tokenType = TokenType.Foreach;
                            else if (m_strToken == "in")
                                tokenType = TokenType.In;
                            else if (m_strToken == "break")
                                tokenType = TokenType.Break;
                            else if (m_strToken == "continue")
                                tokenType = TokenType.Continue;
                            else if (m_strToken == "function")
                                tokenType = TokenType.Function;
                            else if (m_strToken == "return")
                                tokenType = TokenType.Return;
                            else if (m_strToken == "null" || m_strToken == "nil")
                                tokenType = TokenType.Null;
                            else if (m_strToken == "true" || m_strToken == "false")
                                tokenType = TokenType.Boolean;
                            else
                                tokenType = TokenType.Identifier;
                            if (tokenType == TokenType.Boolean) {
                                listTokens.Add(new Token(tokenType, m_strToken == "true", m_iSourceLine, m_iSourceChar));
                            } else if (tokenType == TokenType.Null) {
                                listTokens.Add(new Token(tokenType, ScriptNull.Instance, m_iSourceLine, m_iSourceChar));
                            } else {
                                listTokens.Add(new Token(tokenType, m_strToken, m_iSourceLine, m_iSourceChar));
                            }
                            UndoChar();
                            lexState = LexState.None;
                        }
                        break;
                }
            }
            listTokens.Add(new Token(TokenType.Finished, null, m_iSourceLine, m_iSourceChar));
            return listTokens;
        }
    }
}
