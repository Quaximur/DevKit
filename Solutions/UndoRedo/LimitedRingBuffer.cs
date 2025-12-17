using System;
using System.Collections;
using System.Collections.Generic;

namespace DevKit.UndoRedo
{
    public class LimitedRingBuffer<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable,
        IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        private readonly T[] _items;
        private int _endIndex;
        private int _startIndex;
        private int _count;

        public int Count => _count;
        public int Capacity => _items.Length;
        public bool IsEmpty => _count == 0;
        public bool IsFull => _count == Capacity;
        public bool IsReadOnly => false;

        public LimitedRingBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0");

            _items = new T[capacity];
            _endIndex = 0;
            _startIndex = -1;
            _count = 0;
        }

        public void AddFirst(T item)
        {
            if (IsFull)
            {
                _endIndex = (_endIndex + 1) % Capacity;
                _count--;
            }

            _startIndex = (_startIndex + 1) % Capacity;
            _items[_startIndex] = item;
            _count++;
        }

        public void AddLast(T item)
        {
            if (IsFull)
            {
                _startIndex = (_startIndex - 1 + Capacity) % Capacity;
                _count--;
            }

            _endIndex = (_endIndex - 1 + Capacity) % Capacity;
            _items[_endIndex] = item;
            _count++;
        }

        public T RemoveFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            var item = _items[_startIndex];
            _items[_startIndex] = default;

            _startIndex = (_startIndex - 1 + Capacity) % Capacity;
            _count--;

            return item;
        }

        public T RemoveLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            var item = _items[_endIndex];
            _items[_endIndex] = default;

            _endIndex = (_endIndex + 1) % Capacity;
            _count--;

            return item;
        }

        public T GetFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            return _items[_startIndex];
        }

        public T GetLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            return _items[_endIndex];
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _endIndex = 0;
            _startIndex = -1;
            _count = 0;
        }

        public bool TryAddFirst(T item)
        {
            AddFirst(item);
            return true;
        }

        public bool TryAddLast(T item)
        {
            AddLast(item);
            return true;
        }

        public bool TryRemoveFirst(out T result)
        {
            if (IsEmpty)
            {
                result = default;
                return false;
            }

            result = RemoveFirst();
            return true;
        }

        public bool TryRemoveLast(out T result)
        {
            if (IsEmpty)
            {
                result = default;
                return false;
            }

            result = RemoveLast();
            return true;
        }

        public bool TryGetFirst(out T result)
        {
            if (IsEmpty)
            {
                result = default;
                return false;
            }

            result = GetFirst();
            return true;
        }

        public bool TryGetLast(out T result)
        {
            if (IsEmpty)
            {
                result = default;
                return false;
            }

            result = GetLast();
            return true;
        }

        public T[] ToArray()
        {
            var result = new T[_count];

            if (_count == 0)
                return result;

            if (_endIndex <= _startIndex)
            {
                Array.Copy(_items, _endIndex, result, 0, _count);
            }
            else
            {
                int firstPart = Capacity - _endIndex;
                Array.Copy(_items, _endIndex, result, 0, firstPart);
                Array.Copy(_items, 0, result, firstPart, _startIndex + 1);
            }

            return result;
        }

        public T[] ToReversedArray()
        {
            var result = new T[_count];

            if (_count == 0)
                return result;

            for (int i = 0; i < _count; i++)
            {
                int index = (_startIndex - i + Capacity) % Capacity;
                result[i] = _items[index];
            }

            return result;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                int actualIndex = (_endIndex + index) % Capacity;
                return _items[actualIndex];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                int actualIndex = (_endIndex + index) % Capacity;
                _items[actualIndex] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                int index = (_endIndex + i) % Capacity;
                yield return _items[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < _count; i++)
            {
                int index = (_endIndex + i) % Capacity;
                if (comparer.Equals(_items[index], item))
                    return true;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (array.Length - arrayIndex < _count)
                throw new ArgumentException("Destination array is not long enough");

            if (_count == 0)
                return;

            if (_endIndex <= _startIndex)
            {
                Array.Copy(_items, _endIndex, array, arrayIndex, _count);
            }
            else
            {
                int firstPart = Capacity - _endIndex;
                Array.Copy(_items, _endIndex, array, arrayIndex, firstPart);
                Array.Copy(_items, 0, array, arrayIndex + firstPart, _startIndex + 1);
            }
        }

        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < _count; i++)
            {
                int index = (_endIndex + i) % Capacity;
                if (comparer.Equals(_items[index], item))
                    return i;
            }
            return -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException("Insert operation is not supported for ring buffer");
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("RemoveAt operation is not supported for ring buffer");
        }

        void ICollection<T>.Add(T item)
        {
            AddFirst(item);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException("Remove operation is not supported for ring buffer");
        }
    }
}
