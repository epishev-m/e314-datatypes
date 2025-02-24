using System;

namespace E314.DataTypes
{

/// <summary>
/// Represents a binder that manages bindings between keys and values.
/// </summary>
/// <typeparam name="TKey">The type of the key. Must be a reference type.</typeparam>
/// <typeparam name="TValue">The type of the value. Must be a reference type.</typeparam>
public interface IBinder<TKey, TValue> : IDisposable
	where TKey : class
	where TValue : class
{
	/// <summary>
	/// Creates or retrieves an existing binding for the specified key.
	/// </summary>
	/// <param name="key">The key to bind. Must not be null.</param>
	/// <returns>An <see cref="IBinding{TKey, TValue}"/> instance associated with the key.</returns>
	IBinding<TKey, TValue> Bind(TKey key);

	/// <summary>
	/// Unbinds the specified key and disposes of its associated binding.
	/// </summary>
	/// <param name="key">The key to unbind. Must not be null.</param>
	/// <returns><c>true</c> if the key was successfully unbound; otherwise, <c>false</c>.</returns>
	bool Unbind(TKey key);

	/// <summary>
	/// Retrieves the binding associated with the specified key.
	/// </summary>
	/// <param name="key">The key to retrieve the binding for. Must not be null.</param>
	/// <returns>The <see cref="IBinding{TKey, TValue}"/> instance associated with the key, or <c>null</c> if no binding exists.</returns>
	IBinding<TKey, TValue> GetBinding(TKey key);
}

}