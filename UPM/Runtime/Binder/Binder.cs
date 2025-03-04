using System;
using System.Collections.Generic;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// Represents a binder that manages bindings between keys and values.
/// Implements the <see cref="IBinder{TKey, TValue}"/> interface.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the value. Must be a reference type.</typeparam>
public class Binder<TKey, TValue> : IBinder<TKey, TValue>
	where TKey : class
	where TValue : class
{
	/// <summary>
	/// A strategy for managing the capacity of the list.
	/// </summary>
	protected readonly ICapacityStrategy CapacityStrategy;
	private readonly Dictionary<TKey, IBinding<TKey, TValue>> _bindings;
	private bool _isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="Binder{TKey, TValue}"/> class with the specified initial capacity and capacity strategy.
	/// </summary>
	/// <param name="capacity">The initial capacity of the internal dictionary.</param>
	/// <param name="capacityStrategy">The strategy for managing the capacity of the internal dictionary. Must not be null.</param>
	/// <exception cref="E314.Exceptions.ArgNullException">Thrown if <paramref name="capacityStrategy"/> is null.</exception>
	public Binder(ICapacityStrategy capacityStrategy, int capacity)
	{
		Requires.NotNull(capacityStrategy, nameof(capacityStrategy));
		CapacityStrategy = capacityStrategy;
		capacity = CapacityStrategy.CalculateCapacity(0, capacity);
		_bindings = new Dictionary<TKey, IBinding<TKey, TValue>>(capacity);
	}

	/// <summary>
	/// 
	/// </summary>
	protected bool IsDisposed => _isDisposed;

	/// <summary>
	/// Creates or retrieves an existing binding for the specified key.
	/// </summary>
	/// <param name="key">The key to bind. Must not be null.</param>
	/// <returns>An <see cref="IBinding{TKey, TValue}"/> instance associated with the key.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
	public IBinding<TKey, TValue> Bind(TKey key)
	{
		Requires.NotDisposed(_isDisposed);
		Requires.NotNull(key, nameof(key));
		if (_bindings.TryGetValue(key, out var binding)) return binding;
		binding = GetRawBinding(key);
		_bindings.Add(key, binding);
		return binding;
	}

	/// <summary>
	/// Unbinds the specified key and disposes of its associated binding.
	/// </summary>
	/// <param name="key">The key to unbind. Must not be null.</param>
	/// <returns><c>true</c> if the key was successfully unbound; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
	public bool Unbind(TKey key)
	{
		Requires.NotDisposed(_isDisposed);
		Requires.NotNull(key, nameof(key));
		if (!_bindings.Remove(key, out var binding)) return false;
		binding.Dispose();
		return true;
	}

	/// <summary>
	/// Retrieves the binding associated with the specified key.
	/// </summary>
	/// <param name="key">The key to retrieve the binding for. Must not be null.</param>
	/// <returns>The <see cref="IBinding{TKey, TValue}"/> instance associated with the key, or <c>null</c> if no binding exists.</returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
	public IBinding<TKey, TValue> GetBinding(TKey key)
	{
		Requires.NotDisposed(_isDisposed);
		Requires.NotNull(key, nameof(key));
		return _bindings.GetValueOrDefault(key);
	}

	/// <summary>
	/// Disposes of all resources held by this binder.
	/// </summary>
	public virtual void Dispose()
	{
		foreach (var binding in _bindings.Values) binding.Dispose();
		_bindings.Clear();
		_isDisposed = true;
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Creates a raw binding for the specified key.
	/// This method can be overridden in derived classes to provide custom binding implementations.
	/// </summary>
	/// <param name="key">The key to create a binding for. Must not be null.</param>
	/// <returns>A new <see cref="IBinding{TKey, TValue}"/> instance associated with the key.</returns>
	protected virtual IBinding<TKey, TValue> GetRawBinding(TKey key)
	{
		return new Binding<TKey, TValue>(CapacityStrategy, key);
	}
}

}