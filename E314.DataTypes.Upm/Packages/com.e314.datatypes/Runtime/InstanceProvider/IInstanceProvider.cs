using System;

namespace E314.DataTypes
{

/// <summary>
/// Represents a provider that manages the lifecycle of an instance
/// and ensures proper disposal if the instance implements <see cref="IDisposable"/>.
/// </summary>
public interface IInstanceProvider : IDisposable
{
	/// <summary>
	/// Retrieves the managed instance.
	/// </summary>
	/// <returns>The instance managed by this provider.</returns>
	object GetInstance();
}

}