using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Collections
{
    /// <summary> ֻ��list�� </summary>
    public class ReadOnlyICollection<T> : ICollection<T>
    {
        /// <summary> Returned a read only wrapper around the collectionToWrap. </summary>
        public static ReadOnlyICollection<T> AsReadOnly(ICollection<T> collectionToWrap)
        {
            return new ReadOnlyICollection<T>(collectionToWrap);
        }
        private ICollection<T> m_collection;
        /// <summary> ���캯�� </summary>
        public ReadOnlyICollection(ICollection<T> collectionToWrap) { m_collection = collectionToWrap; }
        /// <summary> ����Add </summary>
        public void Add(T item) { }
        /// <summary> ����Remove </summary>
        public bool Remove(T item) { return false; }
        /// <summary> ����Clear </summary>
        public void Clear() { }
        /// <summary> �Ƿ����ĳֵ </summary>
        public bool Contains(T item) { return m_collection.Contains(item); }
        /// <summary> ���������� </summary>
        public void CopyTo(T[] array, int arrayIndex) { m_collection.CopyTo(array, arrayIndex); }
        /// <summary> �Ƿ�ֻ�� </summary>
        public bool IsReadOnly { get { return true; } }
        /// <summary> Returns an enumerator that iterates through a collection. </summary>
        IEnumerator IEnumerable.GetEnumerator() { return m_collection.GetEnumerator(); }
        /// <summary> Returns an enumerator that iterates through the collection. </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return m_collection.GetEnumerator();
        }
        /// <summary> �������� </summary>
        public int Count { get { return m_collection.Count; } }
    }
}
