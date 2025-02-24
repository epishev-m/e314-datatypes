using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// A high-performance list implementation that uses pooled arrays for efficient memory management.
/// Implements <see cref="IReadOnlyList{T}"/> and <see cref="IDisposable"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class FastList<T> : IReadOnlyList<T>, IDisposable
{
	private static readonly ArrayPool<T> ArrayPool = ArrayPool<T>.Shared;
	private readonly ICapacityStrategy _capacityStrategy;
	private T[] _items;
	private int _count;
	private int _capacity;

	/// <summary>
	/// Initializes a new instance of the <see cref="FastList{T}"/> class with the specified initial capacity and capacity strategy.
	/// </summary>
	/// <param name="capacity">The initial capacity of the list. Must be greater than or equal to 1.</param>
	/// <param name="capacityStrategy">The strategy for managing the capacity of the list. If null, a default <see cref="CapacityStrategy"/> is used.</param>
	/// <exception cref="E314.Exceptions.ArgOutOfRangeException"> Thrown if <paramref name="capacity"/> is less than 1 or greater than <see cref="int.MaxValue"/>. </exception>
	public FastList(int capacity = 1, ICapacityStrategy capacityStrategy = null)
	{
		Requires.InRange(capacity, 1, int.MaxValue, nameof(capacity));
		_capacityStrategy = capacityStrategy ?? new CapacityStrategy();
		_items = ArrayPool.Rent(capacity);
		_capacity = capacity;
		_count = 0;
	}

	public int Count => _count;

	public int Capacity => _capacity;

	/// <summary>
	/// Adds an element to the end of the list.
	/// </summary>
	/// <param name="item">The element to add.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Add(T item)
	{
		var capacity = _capacityStrategy.CalculateCapacity(_capacity, _count + 1);
		if (_count == _capacity) Resize(capacity);
		_items[_count++] = item;
	}

	/// <summary>
	/// Removes the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to remove.</param>
	/// <returns><c>true</c> if the element was successfully removed; otherwise, <c>false</c>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool RemoveAt(int index)
	{
		if ((uint)index >= (uint)_count) return false;
		_count--;
		_items[index] = _items[_count];
		_items[_count] = default;
		return true;
	}

	/// <summary>
	/// Gets or sets the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get or set.</param>
	/// <returns>The element at the specified index.</returns>
	public T this[int index]
	{
		get
		{
			Requires.InRange(index, 0, _count, nameof(index));
			return _items[index];
		}
		set
		{
			Requires.InRange(index, 0, _count, nameof(index));
			_items[index] = value;
		}
	}

	/// <summary>
	/// Clears all elements from the list.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Clear()
	{
		Array.Clear(_items, 0, _count);
		_count = 0;
	}

	/// <summary>
	/// Resizes the internal array to accommodate the specified number of elements.
	/// </summary>
	/// <param name="newSize">The new size of the internal array.</param>
	private void Resize(int newSize)
	{
		var newArray = ArrayPool.Rent(newSize);
		Array.Copy(_items, newArray, _count);
		ArrayPool.Return(_items, clearArray: true);
		_items = newArray;
		_capacity = newSize;
	}

	/// <summary>
	/// Releases the resources used by the list.
	/// </summary>
	public void Dispose()
	{
		foreach (var item in _items) Dispose(item);
		ArrayPool.Return(_items, clearArray: true);
		_items = Array.Empty<T>();
		_capacity = 0;
		_count = 0;
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Returns an enumerator that iterates through the list.
	/// </summary>
	/// <returns>An enumerator for the list.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Enumerator GetEnumerator() => new(this);

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

	/// <inheritdoc cref="IEnumerable.GetEnumerator"/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void Dispose(T value)
	{
		if (value is IDisposable disposable) disposable.Dispose();
	}

	/// <summary>
	/// Enumerates the elements of a <see cref="FastList{T}"/>.
	/// </summary>
	public struct Enumerator : IEnumerator<T>
	{
		private readonly FastList<T> _list;
		private int _index;

		/// <summary>
		/// Initializes a new instance of the <see cref="Enumerator"/> struct.
		/// </summary>
		/// <param name="list">The list to enumerate.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Enumerator(FastList<T> list)
		{
			_list = list;
			_index = 0;
			Current = default;
		}

		/// <summary>
		/// Gets the element at the current position of the enumerator.
		/// </summary>
		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		/// <summary>
		/// Advances the enumerator to the next element of the list.
		/// </summary>
		/// <returns><c>true</c> if the enumerator was successfully advanced; otherwise, <c>false</c>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MoveNext()
		{
			if ((uint)_index >= (uint)_list._count) return false;
			Current = _list._items[_index++];
			return true;
		}

		/// <summary>
		/// Resets the enumerator to its initial position.
		/// </summary>
		public void Reset() => _index = 0;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}
	}
}

}