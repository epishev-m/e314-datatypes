using System;
using E314.Protect;

namespace E314.DataTypes
{

/// <summary>
/// Represents a singleton instance provider that ensures only one instance of the provided object is created and managed.
/// Implements the <see cref="IInstanceProvider"/> interface.
/// </summary>
public sealed class SingletonInstanceProvider : IInstanceProvider
{
	private readonly IInstanceProvider _instanceProvider;
	private object _instance;

	/// <summary>
	/// Initializes a new instance of the <see cref="SingletonInstanceProvider"/> class with the specified instance provider.
	/// </summary>
	/// <param name="instanceProvider">The provider used to create the instance. Must not be null.</param>
	/// <exception cref="ArgNullException">Thrown if <paramref name="instanceProvider"/> is null.</exception>
	public SingletonInstanceProvider(IInstanceProvider instanceProvider)
	{
		Requires.NotNull(instanceProvider, nameof(instanceProvider));
		_instanceProvider = instanceProvider;
	}

	/// <summary>
	/// Disposes of the managed instance and the underlying instance provider if they implement <see cref="IDisposable"/>.
	/// </summary>
	public void Dispose()
	{
		if (_instance is IDisposable disposable) disposable.Dispose();
		_instanceProvider.Dispose();
	}

	/// <summary>
	/// Retrieves the singleton instance managed by this provider.
	/// If the instance has not been created yet, it is created using the underlying instance provider.
	/// </summary>
	/// <returns>The singleton instance managed by this provider.</returns>
	public object GetInstance()
	{
		_instance ??= _instanceProvider.GetInstance();
		return _instance;
	}
}

}