using System;
using System.Collections.Generic;
using System.Text;
using Scorpio.Exception;
namespace Scorpio
{
    //�ű���������
    public class ScriptArray : ScriptObject
    {
        public List<ScriptObject> m_listObject;
        public ScriptArray()
        {
            Type = ObjectType.Array;
            m_listObject = new List<ScriptObject>();
        }
        public ScriptObject GetValue(int index)
        {
            if (index < 0 || index >= m_listObject.Count)
                throw new ExecutionException("index is < 0 or out of count ");
            return m_listObject[index];
        }
        public void SetValue(int index, ScriptObject obj)
        {
            if (index < 0 || index >= m_listObject.Count)
                throw new ExecutionException("index is < 0 or out of count ");
            m_listObject[index] = obj;
        }
        public void Add(ScriptObject obj)
        {
            m_listObject.Add(obj);
        }
        public void Insert(int index, ScriptObject obj)
        {
            m_listObject.Insert(index, obj);
        }
        public int Count()
        {
            return m_listObject.Count;
        }
        public List<ScriptObject>.Enumerator GetIterator()
        {
            return m_listObject.GetEnumerator();
        }
        public override string ToString() { return "Array"; }
    }
}
