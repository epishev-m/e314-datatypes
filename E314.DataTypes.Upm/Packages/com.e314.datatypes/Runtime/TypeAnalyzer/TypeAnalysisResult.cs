using System;
using System.Collections.Generic;
using System.Reflection;

namespace E314.DataTypes
{

/// <summary>
/// Represents the result of a type analysis. Contains information about the analyzed members of a type.
/// </summary>
public sealed class TypeAnalysisResult
{
	/// <summary>
	/// Gets the list of constructors of the analyzed type.
	/// </summary>
	public IReadOnlyList<ConstructorInfo> Constructors { get; internal set; }
	/// <summary>
	/// Gets the list of methods of the analyzed type.
	/// </summary>
	public IReadOnlyList<MethodInfo> Methods { get; internal set; }
	/// <summary>
	/// Gets the list of properties of the analyzed type.
	/// </summary>
	public IReadOnlyList<PropertyInfo> Properties { get; internal set; }
	/// <summary>
	/// Gets the list of fields of the analyzed type.
	/// </summary>
	public IReadOnlyList<FieldInfo> Fields { get; internal set; }
	/// <summary>
	/// Gets the type that was analyzed.
	/// </summary>
	public Type AnalyzedType { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="TypeAnalysisResult"/> class.
	/// </summary>
	/// <param name="type">The type to analyze.</param>
	public TypeAnalysisResult(Type type)
	{
		AnalyzedType = type;
	}

	/// <summary>
	/// Updates the analysis result based on the specified flags.
	/// </summary>
	/// <param name="flags">Flags indicating which members to update.</param>
	internal void Update(TypeAnalysisFlags flags)
	{
		if (flags.HasFlag(TypeAnalysisFlags.Constructors) && Constructors == null)
			Constructors = TypeAnalyzer.GetConstructors(AnalyzedType);
		if (flags.HasFlag(TypeAnalysisFlags.Methods) && Methods == null)
			Methods = TypeAnalyzer.GetMethods(AnalyzedType);
		if (flags.HasFlag(TypeAnalysisFlags.Properties) && Properties == null)
			Properties = TypeAnalyzer.GetProperties(AnalyzedType);
		if (flags.HasFlag(TypeAnalysisFlags.Fields) && Fields == null)
			Fields = TypeAnalyzer.GetFields(AnalyzedType);
	}
}

}