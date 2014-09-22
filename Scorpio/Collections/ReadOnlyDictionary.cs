using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Scorpio.Collections
{
    /// <summary> ֻ��map�� </summary>
    public class ReadOnlyDictionary<TKey, TValue>
        : IDictionary<TKey, TValue>
        , ICollection<KeyValuePair<TKey, TValue>>
        , IEnumerable<KeyValuePair<TKey, TValue>>
        , IDictionary
        , ICollection
        , IEnumerable
        , ISerializable
        , IDeserializationCallback
    {
        /// <summary> Returns a read only dictionary. </summary>
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly(IDictionary<TKey, TValue> dictionaryToWrap)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionaryToWrap);
        }
        private IDictionary<TKey, TValue> m_dictionaryTyped;
        private IDictionary m_dictionary;
        /// <summary> ���캯�� </summary>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionaryToWrap)
        {
            m_dictionaryTyped = dictionaryToWrap;
            m_dictionary = (IDictionary)m_dictionaryTyped;
        }
        /// <summary> Returns an enumerator that iterates through a collection. </summary>
        IEnumerator IEnumerable.GetEnumerator() { return m_dictionary.GetEnumerator(); }
        /// <summary> Returns an  object for the object. </summary>
        IDictionaryEnumerator IDictionary.GetEnumerator() { return m_dictionary.GetEnumerator(); }
        /// <summary> Returns an enumerator that iterates through the collection. </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return m_dictionaryTyped.GetEnumerator(); }
        /// <summary> ����Keyֵ���� </summary>
        ICollection IDictionary.Keys { get { return m_dictionary.Keys; } }
        /// <summary> ����Keyֵֻ������ </summary>
        public ICollection<TKey> Keys { get { return ReadOnlyICollection<TKey>.AsReadOnly(m_dictionaryTyped.Keys); } }
        /// <summary> ����Valueֵ���� </summary>
        ICollection IDictionary.Values { get { return m_dictionary.Values; } }
        /// <summary> ����Valueֵֻ������ </summary>
        public ICollection<TValue> Values { get { return ReadOnlyICollection<TValue>.AsReadOnly(m_dictionaryTyped.Values); } }

        /// <summary> ����Add </summary>
        public void Add(TKey key, TValue value) { }
        /// <summary> ����Add </summary>
        public void Add(KeyValuePair<TKey, TValue> item) { }
        /// <summary> ����Add </summary>
        public void Add(object key, object value) { }
        /// <summary> ����Remove </summary>
        public bool Remove(TKey key) { return false; }
        /// <summary> ����Remove </summary>
        public void Remove(object key) { }
        /// <summary> ����Remove </summary>
        public bool Remove(KeyValuePair<TKey, TValue> item) { return false; }
        /// <summary> ����Clear </summary>
        public void Clear() { }
        /// <summary> �Ƿ����ĳKey </summary>
        public bool ContainsKey(TKey key) { return m_dictionaryTyped.ContainsKey(key); }
        /// <summary> �Ƿ����ĳKey </summary>
        public bool Contains(object key) { return m_dictionary.Contains(key); }
        /// <summary> �Ƿ����ĳֵ </summary>
        public bool Contains(KeyValuePair<TKey, TValue> item) { return m_dictionaryTyped.Contains(item); }
        /// <summary> ���Ի��ĳֵ </summary>
        public bool TryGetValue(TKey key, out TValue value) { return m_dictionaryTyped.TryGetValue(key, out value); }
        /// <summary> ���������� </summary>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { m_dictionaryTyped.CopyTo(array, arrayIndex); }
        /// <summary> Gets the <see cref="System.Object"/> with the specified key. Set does not affect a ReadOnlyDictionary </summary>
        public object this[object key] { get { return m_dictionary[key]; } set { } }
        /// <summary> Gets the <see typeparamref="TValue"/> with the specified key. Set does not change a read only Dictionary </summary>
        public TValue this[TKey key] { get { return m_dictionaryTyped[key]; } set { } }
        /// <summary> Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index. </summary>
        public void CopyTo(Array array, int index) { }
        /// <summary> Runs when the entire object graph has been deserialized. </summary>
        public void OnDeserialization(object sender)
        {
            IDeserializationCallback callback = m_dictionaryTyped as IDeserializationCallback;
            callback.OnDeserialization(sender);
        }
        /// <summary> Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with the data needed to serialize the target object. </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ISerializable serializable = m_dictionaryTyped as ISerializable;
            serializable.GetObjectData(info, context);
        }
        /// <summary> �������� </summary>
        public int Count { get { return m_dictionaryTyped.Count; } }
        /// <summary> �Ƿ�ֻ�� </summary>
        public bool IsReadOnly { get { return true; } }
        /// <summary> Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"></see> object has a fixed size. </summary>
        public bool IsFixedSize { get { return m_dictionary.IsFixedSize; } }
        /// <summary> Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe). </summary>
        public bool IsSynchronized { get { return m_dictionary.IsSynchronized; } }
        /// <summary> Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>. </summary>
        public object SyncRoot { get { return m_dictionary.SyncRoot; } }
    }
}
