using System;

namespace E314.DataTypes
{

/// <summary>
/// Flags for type analysis. Specifies which members of a type should be analyzed.
/// </summary>
[Flags]
public enum TypeAnalysisFlags
{
	/// <summary>
	/// No members will be analyzed.
	/// </summary>

	None = 0,

	/// <summary>
	/// Analyze constructors of the type.
	/// </summary>

	Constructors = 1 << 0,

	/// <summary>
	/// Analyze methods of the type.
	/// </summary>
	Methods = 1 << 1,

	/// <summary>
	/// Analyze properties of the type.
	/// </summary>
	Properties = 1 << 2,

	/// <summary>
	/// Analyze fields of the type.
	/// </summary>
	Fields = 1 << 3,

	/// <summary>
	/// Analyze all members of the type (constructors, methods, properties, and fields).
	/// </summary>
	All = Constructors | Methods | Properties | Fields
}

}