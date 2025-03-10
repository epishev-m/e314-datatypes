# E314.DataTypes

## Description

The `E314.DataTypes` module provides structures and data types.

## Content tree

- [E314.DataTypes](#e314datatypes)
  - [Description](#description)
  - [Content tree](#content-tree)
  - [CapacityStrategy](#capacitystrategy)
    - [Recommendations](#recommendations)
    - [Usage Example](#usage-example)
  - [FastList](#fastlist)
    - [Recommendations](#recommendations-1)
    - [Usage Example](#usage-example-1)
  - [Binder](#binder)
    - [Key Features](#key-features)
    - [Recommendations](#recommendations-2)
    - [Usage Example](#usage-example-2)
  - [InstanceProvider](#instanceprovider)
    - [Recommendations](#recommendations-3)
    - [Usage Example](#usage-example-3)
  - [SingletonInstanceProvider](#singletoninstanceprovider)
    - [Recommendations](#recommendations-4)
    - [Usage Example](#usage-example-4)
  - [ListInstanceProvider](#listinstanceprovider)
    - [Recommendations](#recommendations-5)
    - [Usage Example](#usage-example-5)
  - [FactoryInstanceProvider](#factoryinstanceprovider)
    - [Recommendations](#recommendations-6)
    - [Usage Example](#usage-example-6)
  - [TypeAnalyzer](#typeanalyzer)
    - [How to Use](#how-to-use)
      - [Analyzing a Type](#analyzing-a-type)
      - [Using Flags](#using-flags)
      - [Working with Results](#working-with-results)
      - [Caching](#caching)
    - [Recommendations](#recommendations-7)
    - [Usage Examples](#usage-examples)
      - [Full Type Analysis](#full-type-analysis)
      - [Selective Analysis](#selective-analysis)
  - [Factory](#factory)
    - [Key Features](#key-features-1)
    - [Recommendations](#recommendations-8)
    - [Usage Example](#usage-example-7)

## CapacityStrategy

A strategy for managing the capacity of collections.
This class provides a method to calculate capacity based on its current value and the required size.
The new capacity is selected from a predefined array containing numbers close to powers of two minus one `(2^n - 1)`.
These values ensure efficient memory allocation and minimize hash collisions.

### Recommendations

- Use for dynamic collections when you need efficient memory allocation or minimization of hash collisions.
- Estimate the required size in advance to avoid frequent memory reallocations.
- The maximum capacity is limited to 1,048,575. For larger data, use a different strategy.
- If there is a risk of exceeding the maximum capacity, handle the `InvOpException` exception.
- Implement the `ICapacityStrategy` interface if a different strategy implementation is needed.

### Usage Example

``` csharp
var strategy = new CapacityStrategy();
int requiredSize = 100;
int newCapacity = strategy.CalculateCapacity(0, requiredSize);
Console.WriteLine($"New capacity: {newCapacity}");
// New capacity: 127
```

## FastList

A high-performance list implementation.
The class provides methods for fast addition, removal, and access to elements with minimal overhead.
Uses `ArrayPool<T>`, which minimizes memory allocation and reduces pressure on the garbage collector.
Supports capacity management strategy through the `ICapacityStrategy` interface. By default, `CapacityStrategy` is used.
Supports enumeration using `foreach`.

### Recommendations

- Suitable for scenarios where the list size changes frequently.
- If the approximate number of elements is known, initialize the list with an appropriate initial capacity to avoid frequent memory reallocations.
- Ensure that indices are within valid bounds when using methods such as `RemoveAt` or accessing by index.
- Always call the `Dispose` method to return arrays to the pool and release resources.
- If the default strategy does not suit your needs, create a custom implementation of the `ICapacityStrategy` interface.

### Usage Example

``` csharp
var list = new FastList<int>();

list.Add(10);
list.Add(20);
list.Add(30);

Console.WriteLine($"Count: {list.Count}");
// Count: 3

Console.WriteLine($"Element at index 1: {list[1]}"); 
// Element at index 1: 20

bool isRemoved = list.RemoveAt(1);
// true
Console.WriteLine($"Count after removal: {list.Count}");
// Count after removal: 2

list.Clear();
Console.WriteLine($"Count after clear: {list.Count}");
// Count after clear: 0

list.Dispose();
```

## Binder

The `Binder<TKey, TValue>` class manages bindings between keys and values, implementing the `IBinder<TKey, TValue>` interface.
Provides methods for creating, retrieving, and removing bindings.
Supports capacity management strategy through the `ICapacityStrategy` interface.

### Key Features

- Creating bindings: Creates a new binding for a key or returns an existing one.
- Removing bindings: Removes the binding for a specified key and releases associated resources.
- Retrieving bindings: Returns the binding for a key if it exists, or `null` if the key is absent.
- Releasing resources: Implements the `IDisposable` interface to release all resources associated with bindings.

### Recommendations

- Suitable for scenarios where you need to create and remove relationships between objects (e.g., in event systems, dependencies, or data injection).
- If the approximate number of bindings is known, specify the initial capacity during initialization to minimize memory reallocations.
- Ensure that keys are not `null`, as this will throw an `ArgNullException`.
- Ensure that the object has not been disposed (`Dispose`) before performing operations.
- Always call the `Dispose` method to release all bindings and associated resources.
- Methods support chaining (Fluent interface), making the code more readable and compact.

### Usage Example

``` csharp
var capacityStrategy = new CapacityStrategy();
var binder = new Binder<string, object>(10, capacityStrategy);

binder.Bind("Key1").To(new object());
var binding = binder.GetBinding("Key1");
bool isUnbound = binder.Unbind("Key1");

binder.Dispose();
```

## InstanceProvider

`InstanceProvider` is designed to manage a single instance of an object.
It ensures that the instance will be properly disposed of if it implements the `IDisposable` interface.

### Recommendations

- Use when you need to ensure that an object will be correctly disposed of after use.
- Suitable for managing existing objects.
- Ensure that the passed object is not `null`, as this will throw an exception.

### Usage Example

``` csharp
var disposableObject = new DisposableResource();
using var provider = new InstanceProvider(disposableObject);
var instance = provider.GetInstance();
// ...
```

## SingletonInstanceProvider

`SingletonInstanceProvider` ensures the creation and management of a single instance of an object (Singleton pattern).
The instance is created only on the first request and reused in subsequent calls.

### Recommendations

- Use when you need to ensure that only one instance of an object exists in the system.
- Suitable for managing global resources such as configurations, loggers, or services.
- Ensure that the passed instance provider (`IInstanceProvider`) is not `null`.

### Usage Example

``` csharp
var mockProvider = new MockInstanceProvider(new object());
using var singletonProvider = new SingletonInstanceProvider(mockProvider);
var instance1 = singletonProvider.GetInstance();
var instance2 = singletonProvider.GetInstance();
// instance1 and instance2 will be the same object
```

## ListInstanceProvider

`ListInstanceProvider` creates and manages a list of instances, each provided by a separate provider.
The list is created dynamically based on the specified type.

### Recommendations

- Use when you need to combine multiple instances into a collection, such as for managing a list of dependencies or services.
- Ensure that the collection of providers is not empty and does not contain `null`.
- Specify the correct type of list elements to avoid errors during collection creation.

### Usage Example

``` csharp
var providers = new List<IInstanceProvider>
{
    new MockInstanceProvider(new object()),
    new MockInstanceProvider(new object())
};
using var listProvider = new ListInstanceProvider(providers, typeof(object));
var list = (IList)listProvider.GetInstance();
// ...
```

## FactoryInstanceProvider

`FactoryInstanceProvider` manages the creation of instances via a factory.
The factory is initialized only on the first instance request.

### Recommendations

- Use when you need to create instances "on the fly" using a factory.
- Suitable for scenarios where objects are created dynamically, such as depending on the system state.
- Ensure that the factory implements the `IFactory` interface and correctly creates instances.

### Usage Example

``` csharp
var factoryProvider = new MockInstanceProvider(new MockFactory());
using var provider = new FactoryInstanceProvider(factoryProvider);
var instance = provider.GetInstance();
// ...
```

## TypeAnalyzer

`TypeAnalyzer` is a tool for analyzing the structure of types.
It provides the ability to extract information about constructors, methods, properties, and fields of a given type.
This tool is useful when working with reflection, allowing you to dynamically inspect types at runtime.

### How to Use

`TypeAnalyzer` is used for:

- Inspecting types: Retrieving information about the structure of a class or structure.
- Filtering data: Selectively extracting only those elements of a type that are needed (e.g., only methods or only properties).
- Caching results: Optimizing performance by caching analysis results.

#### Analyzing a Type

To analyze a type, use the `Analyze` method of the `ITypeAnalyzer` interface. The method takes two parameters:

- `type`: The type to analyze.
- `flags`: Flags that determine which elements of the type to extract (constructors, methods, properties, fields).

``` csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisFlags.Methods | TypeAnalysisFlags.Properties);
```

In this example, only the methods and properties of the `MyClass` type will be extracted.

#### Using Flags

The `TypeAnalysisFlags` flags allow flexible configuration of the analysis:

- `Constructors`: Extracts the constructors of the type.
- `Methods`: Extracts the methods of the type.
- `Properties`: Extracts the properties of the type.
- `Fields`: Extracts the fields of the type.
- `All`: Extracts all elements of the type.

If you need only specific elements, combine the flags using the | operator:

``` csharp
var flags = TypeAnalysisFlags.Constructors | TypeAnalysisFlags.Fields;
var result = analyzer.Analyze(typeof(MyClass), flags);
```

#### Working with Results

The analysis result is returned as a `TypeAnalysisResult` object, which contains collections:

- `Constructors`: A list of constructors.
- `Methods`: A list of methods.
- `Properties`: A list of properties.
- `Fields`: A list of fields.

These collections can be used for further processing. For example:

``` csharp
foreach (var method in result.Methods)
{
    Console.WriteLine($"Method: {method.Name}");
}
```

#### Caching

`TypeAnalyzer` automatically caches analysis results for each type.
This avoids re-analyzing the same type, improving performance.
If you need to clear the cache (e.g., during testing), you can do so via reflection:

``` csharp
typeof(TypeAnalyzer)
    .GetField("Cache", BindingFlags.Static | BindingFlags.NonPublic)?
    .SetValue(null, new Dictionary<Type, TypeAnalysisResult>());
```

### Recommendations

- Performance Optimization
  - Use the minimal set of flags to extract only the data you actually need.
  - If you work with the same type multiple times, rely on caching to avoid redundant computations.
- Handling Empty Types
  - If the analyzed type contains no elements (e.g., an empty class), the corresponding collections in `TypeAnalysisResult` will be empty. Ensure your code handles such cases correctly.
- Input Validation
  - Before calling the `Analyze` method, ensure the passed type is not `null`. Otherwise, an `ArgNullException` will be thrown.
- Testing
  - Clear the cache before each test to avoid the influence of previous runs.
  - Test both positive scenarios (types with various elements) and negative scenarios (empty types, invalid flags).

### Usage Examples

#### Full Type Analysis

``` csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisFlags.All);
Console.WriteLine("Constructors:");
foreach (var ctor in result.Constructors)
{
    Console.WriteLine(ctor.Name);
}
Console.WriteLine("Methods:");
foreach (var method in result.Methods)
{
    Console.WriteLine(method.Name);
}
```

#### Selective Analysis

``` csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisFlags.Properties);
Console.WriteLine("Properties:");
foreach (var property in result.Properties)
{
    Console.WriteLine(property.Name);
}
```

## Factory

Factory is an implementation of the `IFactory` interface that creates objects using a factory function.

### Key Features

- Object creation using `Func<object>` delegate
- Automatic resource cleanup through `IDisposable` interface
- Null parameter validation
- Thread-safe object creation

### Recommendations

1. Use Factory when:
   - You need to encapsulate object creation logic
   - Object creation logic may change at runtime
   - Lazy object creation is required

2. Avoid using Factory when:
   - Object creation logic is simple and requires no additional steps
   - Object must be created immediately
   - Multiple different types of objects need to be created

### Usage Example

```csharp
// Creating a factory for strings
var factory = new Factory(() => "Hello, World!");

// Getting the object
var message = (string)factory.Create();

// Creating a factory for complex objects
var complexFactory = new Factory(() =>
{
    var settings = new ComplexObject();
    settings.Initialize();
    return settings;
});

// Using with 'using' statement for automatic resource cleanup
using (var factory = new Factory(() => new DisposableObject()))
{
    var obj = factory.Create();
    // Working with the object
}
```
