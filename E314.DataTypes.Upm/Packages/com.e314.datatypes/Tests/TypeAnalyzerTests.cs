using E314.Exceptions;
using NUnit.Framework;

namespace E314.DataTypes.Tests
{

[TestFixture]
internal sealed class TypeAnalyzerTests
{
	private ITypeAnalyzer _typeAnalyzer;

	[SetUp]
	public void SetUp()
	{
		_typeAnalyzer = new TypeAnalyzer();
		_typeAnalyzer.ClearCache();
	}

	[Test]
	public void Analyze_WithConstructorsFlag_ReturnsOnlyConstructors()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisFlags.Constructors);

		// Assert
		Assert.That(result.Constructors, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithMethodsFlag_ReturnsOnlyMethods()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisFlags.Methods);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithPropertiesFlag_ReturnsOnlyProperties()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisFlags.Properties);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Fields, Is.Null);
	}

	[Test]
	public void Analyze_WithFieldsFlag_ReturnsOnlyFields()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type, TypeAnalysisFlags.Fields);

		// Assert
		Assert.That(result.Constructors, Is.Null);
		Assert.That(result.Methods, Is.Null);
		Assert.That(result.Properties, Is.Null);
		Assert.That(result.Fields, Is.Not.Null.And.Not.Empty);
	}

	[Test]
	public void Analyze_WithAllFlags_ReturnsAllMembers()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result = _typeAnalyzer.Analyze(type);

		// Assert
		Assert.That(result.Constructors, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Methods, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Properties, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Fields, Is.Not.Null.And.Not.Empty);
	}

	[Test]
	public void Analyze_CachesResults()
	{
		// Arrange
		var type = typeof(SampleClass);

		// Act
		var result1 = _typeAnalyzer.Analyze(type, TypeAnalysisFlags.Constructors);
		var result2 = _typeAnalyzer.Analyze(type, TypeAnalysisFlags.Constructors);

		// Assert
		Assert.That(result1, Is.SameAs(result2));
	}

	[Test]
	public void Analyze_EmptyType_ReturnsEmptyCollections()
	{
		// Arrange
		var type = typeof(EmptyClass);

		// Act
		var result = _typeAnalyzer.Analyze(type);

		// Assert
		Assert.That(result.Constructors, Is.Not.Null.And.Not.Empty);
		Assert.That(result.Methods, Is.Not.Null.And.Empty);
		Assert.That(result.Properties, Is.Not.Null.And.Empty);
		Assert.That(result.Fields, Is.Not.Null.And.Empty);
	}

	[Test]
	public void Analyze_NullType_ThrowsException()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _typeAnalyzer.Analyze(null));
	}

	#region Nested

	private sealed class SampleClass
	{
		public int Field;
		public string Property { get; set; }

		public SampleClass()
		{
		}

		public void Method()
		{
		}
	}

	private sealed class EmptyClass
	{
	}

	#endregion
}

}