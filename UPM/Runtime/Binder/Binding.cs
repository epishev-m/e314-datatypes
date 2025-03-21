using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// Represents a binding between a key and a collection of values.
/// Implements the <see cref="IBinding{TKey, TValue}"/> interface.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the values. Must be a reference type.</typeparam>
public class Binding<TKey, TValue> : IBinding<TKey, TValue>
	where TKey : class
	where TValue : class
{
	private readonly FastList<TValue> _values;
	protected bool IsDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="Binding{TKey, TValue}"/> class with the specified capacity strategy and key.
	/// </summary>
	/// <param name="capacityStrategy">The strategy for managing the capacity of the internal list. Must not be null.</param>
	/// <param name="key">The key associated with this binding. Must not be null.</param>
	/// <exception cref="E314.Exceptions.ArgNullException">Thrown if <paramref name="capacityStrategy"/> or <paramref name="key"/> is null.</exception>
	public Binding(ICapacityStrategy capacityStrategy, TKey key)
	{
		Requires.NotNull(capacityStrategy, nameof(capacityStrategy), this);
		Requires.NotNull(key, nameof(key), this);
		_values = new FastList<TValue>(1, capacityStrategy);
		Key = key;
	}

	/// <summary>
	/// Gets the key associated with this binding.
	/// </summary>
	public TKey Key { get; }

	/// <summary>
	/// Gets the collection of values associated with this binding.
	/// </summary>
	public IReadOnlyList<TValue> Values => _values;

	/// <summary>
	/// Gets the number of values in this binding.
	/// </summary>
	public int Count => _values.Count;

	/// <summary>
	/// Adds a value to this binding.
	/// </summary>
	/// <param name="value">The value to add. Must not be null.</param>
	/// <returns>The current binding instance with the added value.</returns>
	/// <exception cref="E314.Exceptions.ArgNullException">Thrown if <paramref name="value"/> is null.</exception>
	/// <exception cref="E314.Exceptions.ObjDisposedException">Thrown if `Dispose` has been called.</exception>
	public IBinding<TKey, TValue> To(TValue value)
	{
		Requires.NotDisposed(IsDisposed, this);
		Requires.NotNull(value, nameof(value), this);
		_values.Add(value);
		return this;
	}

	/// <summary>
	/// Clears all values from this binding.
	/// </summary>
	public void ClearValues()
	{
		Requires.NotDisposed(IsDisposed, this);
		_values.Clear();
	}

	/// <summary>
	/// Disposes of all resources held by this binding.
	/// </summary>
	public void Dispose()
	{
		_values.Dispose();
		IsDisposed = true;
		GC.SuppressFinalize(this);
	}
}

}