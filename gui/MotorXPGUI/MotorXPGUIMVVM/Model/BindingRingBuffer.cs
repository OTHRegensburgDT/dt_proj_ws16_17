using MotorXPGUIMVVM.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace MotorXPGUIMVVM.Model
{
    /// <summary>
    /// Represents a fixted length ring buffer to store a specified maximal count of items within.
    /// Source: http://florianreischl.blogspot.de/2010/01/generic-c-ringbuffer.html (29.11.2016 Changed)
    /// </summary>
    /// <typeparam name="T">The generic type of the items stored within the ring buffer.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class BindingRingBuffer<T> : Collection<T>, IBindingList, ICancelAddNew, IRaiseItemChangedEvents, IBindingRingBuffer<T>, ICollection<T>
    {
        /// <summary>
        /// Creates a new instance of a see cref="BindingRingBuffer{T}" with a 
        /// specified cache size.
        /// </summary>
        /// <param name="capacity">The maximal count of items to be stored within 
        /// the ring buffer.</param>
        /// <param name="raisesItemChangedEvents"></param>
        /// <exception cref="ArgumentException"></exception>
        public BindingRingBuffer(int capacity, bool raisesItemChangedEvents)
        {
            // validate capacity
            if (capacity <= 0)
                throw new ArgumentException(Resources.RingBuffer_RingBuffer_Must_be_greater_than_zero, nameof(capacity));
            // set capacity and init the cache
            Capacity = capacity;
            RaisesItemChangedEvents = raisesItemChangedEvents;
            _buffer = new T[capacity];
        }

        public BindingRingBuffer(int capacity)
        {
            // validate capacity
            if (capacity <= 0)
                throw new ArgumentException(Resources.RingBuffer_RingBuffer_Must_be_greater_than_zero, nameof(capacity));
            // set capacity and init the cache
            Capacity = capacity;
            _buffer = new T[capacity];
        }

        /// <summary>
        /// the internal buffer
        /// </summary>
        private T[] _buffer;
        /// <summary>
        /// The all-over position within the ring buffer. The position 
        /// increases continously by adding new items to the buffer. This 
        /// value is needed to calculate the current relative position within the 
        /// buffer.
        /// </summary>
        private int _position;
        /// <summary>
        /// The current version of the buffer, this is required for a correct 
        /// exception handling while enumerating over the items of the buffer.
        /// </summary>
        private long _version;

        /// <summary>
        /// Gets or sets an item for a specified position within the ring buffer.
        /// </summary>
        /// <param name="index">The position to get or set an item.</param>
        /// <returns>The fond item at the specified position within the ring buffer.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public new T this[int index]
        {
            get
            {
                // validate the index
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();
                // calculate the relative position within the rolling base array
                var index2 = (_position - Count + index) % Capacity;
                return _buffer[index2];
            }
            set { Insert(index, value); }
        }

        /// <summary>
        /// Gets the maximal count of items within the ring buffer.
        /// </summary>
        public int Capacity { get; private set; }
        /// <summary>
        /// Get the current count of items within the ring buffer.
        /// </summary>
        public new int Count { get; private set; }

        /// <summary>
        /// Adds a new item to the buffer.
        /// </summary>
        /// <param name="item">The item to be added to the buffer.</param>
        public new void Add(T item)
        {
            // add a new item to the current relative position within the
            // buffer and increase the position
            _buffer[_position++ % Capacity] = item;
            // increase the count if capacity is not yet reached
            if (Count < Capacity) Count++;
            // buffer changed; next version
            _version++;
        }

        /// <summary>
        /// Clears the whole buffer and releases all referenced objects 
        /// currently stored within the buffer.
        /// </summary>
        public new void Clear()
        {
            for (var i = 0; i < Count; i++)
                _buffer[i] = default(T);
            _position = 0;
            Count = 0;
            _version++;
        }

        /// <summary>
        /// Determines if a specified item is currently present within
        /// the buffer.
        /// </summary>
        /// <param name="item">The item to search for within the current
        /// buffer.</param>
        /// <returns>True if the specified item is currently present within 
        /// the buffer; otherwise false.</returns>
        public new bool Contains(T item)
        {
            var index = IndexOf(item);
            return index != -1;
        }

        /// <summary>
        /// Copies the current items within the buffer to a specified array.
        /// </summary>
        /// <param name="array">The target array to copy the items of 
        /// the buffer to.</param>
        /// <param name="arrayIndex">The start position witihn the target
        /// array to start copying.</param>
        public new void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                array[i + arrayIndex] = _buffer[(_position - Count + i) % Capacity];
            }
        }

        /// <summary>
        /// Gets an enumerator over the current items within the buffer.
        /// </summary>
        /// <returns>An enumerator over the current items within the buffer.
        /// </returns>
        public new IEnumerator<T> GetEnumerator()
        {
            long version = _version;
            for (int i = 0; i < Count; i++)
            {
                if (version != _version)
                    throw new InvalidOperationException("Collection changed");
                yield return this[i];
            }
        }

        /// <summary>
        /// Gets the position of a specied item within the ring buffer.
        /// </summary>
        /// <param name="item">The item to get the current position for.</param>
        /// <returns>The zero based index of the found item within the 
        /// buffer. If the item was not present within the buffer, this
        /// method returns -1.</returns>
        public new int IndexOf(T item)
        {
            // loop over the current count of items
            for (var i = 0; i < Count; i++)
            {
                // get the item at the relative position within the internal array
                T item2 = _buffer[(_position - Count + i) % Capacity];
                // if both items are null, return true
                if (null == item && null == item2)
                    return i;
                // if equal return the position
                if (item != null && item.Equals(item2))
                    return i;
            }
            // nothing found
            return -1;
        }

        /// <summary>
        /// Inserts an item at a specified position into the buffer.
        /// </summary>
        /// <param name="index">The position within the buffer to add 
        /// the new item.</param>
        /// <param name="item">The new item to be added to the buffer.</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <remarks>
        /// If the specified index is equal to the current count of items
        /// within the buffer, the specified item will be added.
        /// 
        /// <b>Warning</b>
        /// Frequent usage of this method might become a bad idea if you are 
        /// working with a large buffer capacity. The insertion of an item
        /// at a specified position within the buffer causes causes all present 
        /// items below the specified position to be moved one position.
        /// </remarks>
        public new void Insert(int index, T item)
        {
            // validate index
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException();
            // add if index equals to count
            if (index == Count)
            {
                Add(item);
                return;
            }

            // get the maximal count of items to be moved
            var count = Math.Min(Count, Capacity - 1) - index;
            // get the relative position of the new item within the buffer
            var index2 = (_position - Count + index) % Capacity;

            // move all items below the specified position
            for (var i = index2 + count; i > index2; i--)
            {
                var to = i % Capacity;
                var from = (i - 1) % Capacity;
                _buffer[to] = _buffer[from];
            }

            // set the new item
            _buffer[index2] = item;

            // adjust storage information
            if (Count < Capacity)
            {
                Count++;
                _position++;
            }
            // buffer changed; next version
            _version++;
        }

        /// <summary>
        /// Removes a specified item from the current buffer.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>True if the specified item was successfully removed
        /// from the buffer; otherwise false.</returns>
        /// <remarks>
        /// <b>Warning</b>
        /// Frequent usage of this method might become a bad idea if you are 
        /// working with a large buffer capacity. The removing of an item 
        /// requires a scan of the buffer to get the position of the specified
        /// item. If the item was found, the deletion requires a move of all 
        /// items stored abouve the found position.
        /// </remarks>
        public new bool Remove(T item)
        {
            // find the position of the specified item
            var index = IndexOf(item);
            // item was not found; return false
            if (index == -1)
                return false;
            // remove the item at the specified position
            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes an item at a specified position within the buffer.
        /// </summary>
        /// <param name="index">The position of the item to be removed.</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <remarks>
        /// <b>Warning</b>
        /// Frequent usage of this method might become a bad idea if you are 
        /// working with a large buffer capacity. The deletion requires a move 
        /// of all items stored abouve the found position.
        /// </remarks>
        public new void RemoveAt(int index)
        {
            // validate the index
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();
            // move all items above the specified position one step
            // closer to zeri
            for (int i = index; i < Count - 1; i++)
            {
                // get the next relative target position of the item
                int to = (_position - Count + i) % Capacity;
                // get the next relative source position of the item
                int from = (_position - Count + i + 1) % Capacity;
                // move the item
                _buffer[to] = _buffer[from];
            }
            // get the relative position of the last item, which becomes empty
            // after deletion and set the item as empty
            int last = (_position - 1) % Capacity;
            _buffer[last] = default(T);
            // adjust storage information
            _position--;
            Count--;
            // buffer changed; next version
            _version++;
        }

        /// <summary>
        /// Gets if the buffer is read-only. This method always returns false.
        /// </summary>
        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// See generic implementation of <see cref="GetEnumerator"/>.
        /// </summary>
        /// <returns>See generic implementation of <see cref="GetEnumerator"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public object AddNew()
        {
            throw new NotImplementedException();
        }

        public void AddIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotImplementedException();
        }

        public int Find(PropertyDescriptor property, object key)
        {
            throw new NotImplementedException();
        }

        public void RemoveIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }

        public void RemoveSort()
        {
            throw new NotImplementedException();
        }

        public object LastOrDefault()
        {
            return _buffer.LastOrDefault();
        }

        public bool AllowNew { get; }
        public bool AllowEdit { get; }
        public bool AllowRemove { get; }
        public bool SupportsChangeNotification { get; }
        public bool SupportsSearching { get; }
        public bool SupportsSorting { get; }
        public bool IsSorted { get; }
        public PropertyDescriptor SortProperty { get; }
        public ListSortDirection SortDirection { get; }
        public event ListChangedEventHandler ListChanged;
        public void CancelNew(int itemIndex)
        {
            throw new NotImplementedException();
        }

        public void EndNew(int itemIndex)
        {
            throw new NotImplementedException();
        }

        public bool RaisesItemChangedEvents { get; }
    }
}

