using System;
using System.Collections.Generic;

namespace E314.DataTypes
{

/// <summary>
/// Represents a binding between a key and a collection of values.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the values. Must be a reference type.</typeparam>
public interface IBinding<out TKey, TValue> : IDisposable
	where TKey : class
	where TValue : class
{
	/// <summary>
	/// Gets the key associated with this binding.
	/// </summary>
	TKey Key { get; }

	/// <summary>
	/// Gets the collection of values associated with this binding.
	/// </summary>
	IReadOnlyList<TValue> Values { get; }

	/// <summary>
	/// Gets the number of values in this binding.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Adds a value to this binding.
	/// </summary>
	/// <param name="value">The value to add. Must not be null.</param>
	/// <returns>The current binding instance with the added value.</returns>
	IBinding<TKey, TValue> To(TValue value);

	/// <summary>
	/// Clears all values from this binding.
	/// </summary>
	void ClearValues();
}

}