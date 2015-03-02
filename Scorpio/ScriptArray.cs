using System;
using System.Collections.Generic;
using System.Text;
using Scorpio.CodeDom;
using Scorpio.Exception;
namespace Scorpio
{
    //�ű���������
    public class ScriptArray : ScriptObject
    {
        public override ObjectType Type { get { return ObjectType.Array; } }
        public List<ScriptObject> m_listObject = new List<ScriptObject>();
        public ScriptArray(Script script) : base(script) { }
        public override ScriptObject GetValue(object index)
        {
            if (!(index is double || index is int || index is long))
                throw new ExecutionException("Array GetValueֻ֧��Number����");
            int i = Util.ToInt32(index);
            if (i < 0 || i >= m_listObject.Count)
                throw new ExecutionException("Array GetValue����С��0���߳������ֵ");
            return m_listObject[i];
        }
        public override void SetValue(object index, ScriptObject obj)
        {
            if (!(index is double || index is int || index is long))
                throw new ExecutionException("Array SetValueֻ֧��Number����");
            int i = Util.ToInt32(index);
            if (i < 0 || i >= m_listObject.Count)
                throw new ExecutionException("Array SetValue����С��0���߳������ֵ");
            m_listObject[i] = obj;
        }
        public void Add(ScriptObject obj)
        {
            m_listObject.Add(obj);
        }
        public void Insert(int index, ScriptObject obj)
        {
            m_listObject.Insert(index, obj);
        }
        public void Remove(ScriptObject obj)
        {
            m_listObject.Remove(obj);
        }
        public void RemoveAt(int index)
        {
            m_listObject.RemoveAt(index);
        }
        public bool Contains(ScriptObject obj)
        {
            return m_listObject.Contains(obj);
        }
        public int IndexOf(ScriptObject obj)
        {
            return m_listObject.IndexOf(obj);
        }
        public int LastIndexOf(ScriptObject obj)
        {
            return m_listObject.LastIndexOf(obj);
        }
        public void Clear()
        {
            m_listObject.Clear();
        }
        public int Count()
        {
            return m_listObject.Count;
        }
        public ScriptObject First()
        {
            if (m_listObject.Count > 0)
                return m_listObject[0];
            return ScriptNull.Instance;
        }
        public ScriptObject Last()
        {
            if (m_listObject.Count > 0)
                return m_listObject[m_listObject.Count - 1];
            return ScriptNull.Instance;
        }
        public List<ScriptObject>.Enumerator GetIterator()
        {
            return m_listObject.GetEnumerator();
        }
        public override ScriptObject Clone()
        {
            ScriptArray ret = Script.CreateArray();
            for (int i = 0; i < m_listObject.Count; ++i) {
                ret.m_listObject.Add(m_listObject[i].Clone());
            }
            return ret;
        }
        public override string ToString() { return "Array"; }
        public override string ToJson()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < m_listObject.Count;++i )
            {
                if (i != 0) builder.Append(",");
                builder.Append(m_listObject[i].ToJson());
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
