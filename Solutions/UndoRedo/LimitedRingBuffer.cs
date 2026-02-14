using System;
using System.Collections;
using System.Collections.Generic;

namespace DevKit.UndoRedo
{
    public class LimitedRingBuffer<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable,
        IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        private readonly T[] _items;
        private int _head;
        private int _tail;
        private int _count;
        private readonly int _capacity;

        public int Count => _count;
        public int Capacity => _capacity;
        public bool IsEmpty => _count == 0;
        public bool IsFull => _count == Capacity;
        public bool IsReadOnly => false;

        public LimitedRingBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0");

            _capacity = capacity;
            _items = new T[capacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        public void AddFirst(T item)
        {
            if (IsFull)
            {
                _tail = (_tail - 1 + Capacity) % Capacity;
                _count--;
            }

            _head = (_head - 1 + Capacity) % Capacity;
            _items[_head] = item;
            _count++;
        }

        public void AddLast(T item)
        {
            if (IsFull)
            {
                _head = (_head + 1) % Capacity;
                _count--;
            }

            _items[_tail] = item;
            _tail = (_tail + 1) % Capacity;
            _count++;
        }

        public T RemoveFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            var item = _items[_head];
            _items[_head] = default;

            _head = (_head + 1) % Capacity;
            _count--;

            return item;
        }

        public T RemoveLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            _tail = (_tail - 1 + Capacity) % Capacity;
            var item = _items[_tail];
            _items[_tail] = default;

            _count--;

            return item;
        }

        public T GetFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            return _items[_head];
        }

        public T GetLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Buffer is empty");

            return _items[(_tail - 1 + Capacity) % Capacity];
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        public T[] ToArray()
        {
            var result = new T[_count];

            if (_count == 0)
                return result;

            if (_head < _tail)
            {
                Array.Copy(_items, _head, result, 0, _count);
            }
            else
            {
                int firstPart = Capacity - _head;
                Array.Copy(_items, _head, result, 0, firstPart);
                Array.Copy(_items, 0, result, firstPart, _tail);
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
                int index = (_head + i) % Capacity;
                result[_count - 1 - i] = _items[index];
            }

            return result;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                int actualIndex = (_head + index) % Capacity;
                return _items[actualIndex];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                int actualIndex = (_head + index) % Capacity;
                _items[actualIndex] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                int index = (_head + i) % Capacity;
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
                int index = (_head + i) % Capacity;
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

            if (_head < _tail)
            {
                Array.Copy(_items, _head, array, arrayIndex, _count);
            }
            else
            {
                int firstPart = Capacity - _head;
                Array.Copy(_items, _head, array, arrayIndex, firstPart);
                Array.Copy(_items, 0, array, arrayIndex + firstPart, _tail);
            }
        }

        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < _count; i++)
            {
                int index = (_head + i) % Capacity;
                if (comparer.Equals(_items[index], item))
                    return i;
            }
            return -1;
        }

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
        void ICollection<T>.Add(T item) => AddLast(item);
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    }
}