using System;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// A sealed implementation of <see cref="IFactory"/> that creates instances using a factory function.
/// </summary>
public sealed class Factory : IFactory
{
	private readonly Func<object> _factoryFunc;

	/// <summary>
	/// Initializes a new instance of the <see cref="Factory"/> class with the specified factory function.
	/// </summary>
	/// <param name="factoryFunc">The function used to create instances. Must not be null.</param>
	/// <exception cref="E314.Protect.ArgNullException">Thrown if <paramref name="factoryFunc"/> is null.</exception>
	public Factory(Func<object> factoryFunc)
	{
		Requires.NotNull(factoryFunc, nameof(factoryFunc), this);
		_factoryFunc = factoryFunc;
	}

	/// <summary>
	/// Creates and returns a new object instance using the factory function.
	/// </summary>
	/// <returns>The created object instance.</returns>
	public object Create()
	{
		return _factoryFunc();
	}

	/// <summary>
	/// Disposes of resources used by this factory.
	/// </summary>
	public void Dispose()
	{
	}
}

} 