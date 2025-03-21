using System;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// A sealed implementation of <see cref="IInstanceProvider"/> that manages a single instance
/// and ensures its disposal if it implements <see cref="IDisposable"/>.
/// </summary>
public sealed class InstanceProvider : IInstanceProvider
{
	private readonly object _instance;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstanceProvider"/> class with the specified instance.
	/// </summary>
	/// <param name="instance">The instance to be managed. Must not be null.</param>
	/// <exception cref="E314.Protect.ArgNullException">Thrown if <paramref name="instance"/> is null.</exception>
	public InstanceProvider(object instance)
	{
		Requires.NotNull(instance, nameof(instance), this);
		_instance = instance;
	}

	/// <summary>
	/// Disposes of the managed instance if it implements <see cref="IDisposable"/>.
	/// </summary>
	public void Dispose()
	{
		if (_instance is IDisposable disposable) disposable.Dispose();
	}

	/// <summary>
	/// Retrieves the managed instance.
	/// </summary>
	/// <returns>The instance managed by this provider.</returns>
	public object GetInstance()
	{
		return _instance;
	}
}

}