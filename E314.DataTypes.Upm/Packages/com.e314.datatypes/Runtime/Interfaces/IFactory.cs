using System;

namespace E314.DataTypes
{

/// <summary>
/// Represents a factory that creates objects.
/// Implements <see cref="IDisposable"/> to ensure proper resource cleanup.
/// </summary>
public interface IFactory : IDisposable
{
	/// <summary>
	/// Creates and returns a new object instance.
	/// </summary>
	/// <returns>The created object instance.</returns>
	object Create();
}

/// <summary>
/// Represents a generic factory that creates objects of type <typeparamref name="T"/>.
/// Implements <see cref="IDisposable"/> to ensure proper resource cleanup.
/// </summary>
/// <typeparam name="T">The type of object to create.</typeparam>
public interface IFactory<out T> : IDisposable
{
	/// <summary>
	/// Creates and returns a new instance of type <typeparamref name="T"/>.
	/// </summary>
	/// <returns>The created instance of type <typeparamref name="T"/>.</returns>
	T Create();
}

}