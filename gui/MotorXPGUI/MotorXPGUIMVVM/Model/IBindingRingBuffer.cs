using System.Collections.Generic;
using System.ComponentModel;

namespace MotorXPGUIMVVM.Model
{
    public interface IBindingRingBuffer<T>
    {
        T this[int index] { get; set; }

        bool AllowEdit { get; }
        bool AllowNew { get; }
        bool AllowRemove { get; }
        int Capacity { get; }
        int Count { get; }
        bool IsSorted { get; }
        bool RaisesItemChangedEvents { get; }
        ListSortDirection SortDirection { get; }
        PropertyDescriptor SortProperty { get; }
        bool SupportsChangeNotification { get; }
        bool SupportsSearching { get; }
        bool SupportsSorting { get; }

        event ListChangedEventHandler ListChanged;

        void Add(T item);
        void AddIndex(PropertyDescriptor property);
        object AddNew();
        void ApplySort(PropertyDescriptor property, ListSortDirection direction);
        void CancelNew(int itemIndex);
        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        void EndNew(int itemIndex);
        int Find(PropertyDescriptor property, object key);
        IEnumerator<T> GetEnumerator();
        int IndexOf(T item);
        void Insert(int index, T item);
        bool Remove(T item);
        void RemoveAt(int index);
        void RemoveIndex(PropertyDescriptor property);
        void RemoveSort();
        object LastOrDefault();
    }
}