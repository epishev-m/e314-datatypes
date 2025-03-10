# E314.DataTypes

## Описание

Модуль E314.DataTypes предоставляет структуры и типы данных.

## Содержание

- [E314.DataTypes](#e314datatypes)
  - [Описание](#описание)
  - [Содержание](#содержание)
  - [CapacityStrategy](#capacitystrategy)
    - [Рекомендации](#рекомендации)
    - [Пример использования](#пример-использования)
  - [FastList](#fastlist)
    - [Рекомендации](#рекомендации-1)
    - [Пример использования](#пример-использования-1)
  - [Binder](#binder)
    - [Основные возможности](#основные-возможности)
    - [Рекомендации](#рекомендации-2)
    - [Пример использования](#пример-использования-2)
  - [InstanceProvider](#instanceprovider)
    - [Рекомендации](#рекомендации-3)
    - [Пример использования](#пример-использования-3)
  - [SingletonInstanceProvider](#singletoninstanceprovider)
    - [Рекомендации](#рекомендации-4)
    - [Пример использования](#пример-использования-4)
  - [ListInstanceProvider](#listinstanceprovider)
    - [Рекомендации](#рекомендации-5)
    - [Пример использования](#пример-использования-5)
  - [FactoryInstanceProvider](#factoryinstanceprovider)
    - [Рекомендации](#рекомендации-6)
    - [Пример использования](#пример-использования-6)
  - [TypeAnalyzer](#typeanalyzer)
    - [Как использовать](#как-использовать)
      - [Анализ типа](#анализ-типа)
      - [Использование флагов](#использование-флагов)
      - [Работа с результатами](#работа-с-результатами)
      - [Кэширование](#кэширование)
    - [Рекомендации](#рекомендации-7)
    - [Пример использования](#пример-использования-7)
      - [Полный анализ типа](#полный-анализ-типа)
      - [Выборочный анализ](#выборочный-анализ)
  - [Factory](#factory)
    - [Основные возможности](#основные-возможности-1)
    - [Рекомендации](#рекомендации-8)
    - [Пример использования](#пример-использования-8)

## CapacityStrategy

Стратегия управления емкостью коллекций.
Этот класс предоставляет метод для вычисления емкости на основе ее текущего значения и требуемого размера.
Новая емкость выбирается из предопределенного массива, содержащего числа, близкие к степени два минус один `(2^n - 1)`.
Эти значения обеспечивают эффективное распределение памяти и минимизируют коллизии хэшей.

### Рекомендации

- Используйте для динамических коллекций, когда нужно эффективное распределение памяти или минимизация коллизии хэшей.
- Оцените требуемый размер заранее, чтобы избежать частых перераспределений памяти.
- Максимальная емкость ограничена значением 1,048,575. Для больших данных используйте другую стратегию.
- Если существует риск превышения максимального значения емкости, обработайте исключение `InvOpException`.
- Реализуйте интерфейс `ICapacityStrategy`, если нужна другая реализация стратегии.

### Пример использования

``` csharp
var strategy = new CapacityStrategy();
int requiredSize = 100;
int newCapacity = strategy.CalculateCapacity(0, requiredSize);
Console.WriteLine($"New capacity: {newCapacity}");
// New capacity: 127
```

## FastList

Реализация быстрого списка.
Класс предоставляет методы для быстрого добавления, удаления и доступа к элементам с минимальными накладными расходами.
Использует `ArrayPool<T>`, что минимизирует выделение памяти и уменьшает нагрузку на сборщик мусора.
Поддерживает стратегию управления емкостью через интерфейс `ICapacityStrategy`. По умолчанию используется `CapacityStrategy`.
Поддерживает перебор значений при помощи `foreach`.

### Рекомендации

- Подходит для сценариев, где требуется частое изменение размера списка.
- Если известно примерное количество элементов, инициализируйте список с соответствующей начальной емкостью, чтобы избежать частых перераспределений памяти.
- Убедитесь, что индексы находятся в допустимых пределах при использовании методов, таких как RemoveAt или доступ по индексу.
- Всегда вызывайте метод Dispose, чтобы вернуть массивы в пул и освободить ресурсы.
- Если стандартная стратегия не подходит, создайте собственную реализацию интерфейса ICapacityStrategy.

### Пример использования

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

Класс `Binder<TKey, TValue>` управляет связями между ключами и значениями, реализуя интерфейс `IBinder<TKey, TValue>`.
Предоставляет методы для создания, получения и удаления привязок (bindings).
Поддерживает стратегию управления емкостью через интерфейс `ICapacityStrategy`.

### Основные возможности

- Создание привязок: Создает новую привязку для ключа или возвращает существующую.
- Удаление привязок: Удаляет привязку для указанного ключа и освобождает связанные ресурсы.
- Получение привязок: Возвращает привязку для ключа, если она существует, или `null`, если ключ отсутствует.
- Освобождение ресурсов: Реализует интерфейс `IDisposable` для освобождения всех ресурсов, связанных с привязками.

### Рекомендации

- Подходит для сценариев, где требуется создавать и удалять связи между объектами (например, в системах событий, зависимостей или инъекции данных).
- Если известно примерное количество привязок, укажите начальную емкость при инициализации, чтобы минимизировать перераспределения памяти.
- Убедитесь, что ключи не являются `null`, так как это вызовет исключение `ArgNullException`.
- Убедитесь, что объект не был освобожден (`Dispose`), перед выполнением операций.
- Всегда вызывайте метод `Dispose`, чтобы освободить все привязки и связанные ресурсы.
- Методы поддерживают цепочку вызовов (Fluent-интерфейс), что делает код более читаемым и компактным.

### Пример использования

``` csharp
var capacityStrategy = new CapacityStrategy();
var binder = new Binder<string, object>(10, capacityStrategy);

binder.Bind("Key1").To(new object());
var binding = binder.GetBinding("Key1");
bool isUnbound = binder.Unbind("Key1");

binder.Dispose();
```

## InstanceProvider

`InstanceProvider` предназначен для управления одиночным экземпляром объекта.
Он гарантирует, что экземпляр будет корректно освобожден, если он реализует интерфейс `IDisposable`.

### Рекомендации

- Используйте, когда нужно гарантировать, что объект будет правильно освобожден после использования.
- Подходит для управления существующим объектом.
- Убедитесь, что передаваемый объект не является `null`, так как это вызовет исключение.

### Пример использования

``` csharp
var disposableObject = new DisposableResource();
using var provider = new InstanceProvider(disposableObject);
var instance = provider.GetInstance();
// ...
```

## SingletonInstanceProvider

`SingletonInstanceProvider` обеспечивает создание и управление единственным экземпляром объекта (паттерн Singleton).
Экземпляр создается только при первом запросе и повторно используется в последующих вызовах.

### Рекомендации

- Используйте, когда требуется гарантировать, что в системе существует только один экземпляр объекта.
- Подходит для управления глобальными ресурсами, такими как конфигурации, логгеры или сервисы.
- Убедитесь, что передаваемый провайдер экземпляров (`IInstanceProvider`) не является `null`.

### Пример использования

``` csharp
var mockProvider = new MockInstanceProvider(new object());
using var singletonProvider = new SingletonInstanceProvider(mockProvider);
var instance1 = singletonProvider.GetInstance();
var instance2 = singletonProvider.GetInstance();
// instance1 и instance2 будут одним и тем же объектом
```

## ListInstanceProvider

`ListInstanceProvider` создает и управляет списком экземпляров, каждый из которых предоставляется отдельным провайдером.
Список создается динамически на основе указанного типа.

### Рекомендации

- Используйте, когда требуется объединить несколько экземпляров в коллекцию, например, для управления списком зависимостей или сервисов.
- Убедитесь, что коллекция провайдеров не пуста и не содержит `null`.
- Указывайте правильный тип элементов списка, чтобы избежать ошибок при создании коллекции.

### Пример использования

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

`FactoryInstanceProvider` управляет созданием экземпляров через фабрику.
Фабрика инициализируется только при первом запросе экземпляра.

### Рекомендации

- Используйте, когда требуется создавать экземпляры "на лету" с помощью фабрики.
- Подходит для сценариев, где объекты создаются динамически, например, в зависимости от состояния системы.
- Убедитесь, что фабрика реализует интерфейс `IFactory` и правильно создает экземпляры.

### Пример использования

``` csharp
var factoryProvider = new MockInstanceProvider(new MockFactory());
using var provider = new FactoryInstanceProvider(factoryProvider);
var instance = provider.GetInstance();
// ...
```

## TypeAnalyzer

`TypeAnalyzer` — это инструмент для анализа структуры типов.
Он предоставляет возможность извлекать информацию о конструкторах, методах, свойствах и полях заданного типа.
Этот инструмент полезен при работе с рефлексией, когда требуется динамически исследовать типы во время выполнения.

### Как использовать

`TypeAnalyzer` используется для:

- Инспекции типов : Получение информации о структуре класса или структуры.
- Фильтрации данных : Выборочное извлечение только тех элементов типа, которые необходимы (например, только методы или только свойства).
- Кэширования результатов : Оптимизация производительности за счет кэширования результатов анализа.

#### Анализ типа

Для анализа типа используйте метод Analyze интерфейса ITypeAnalyzer. Метод принимает два параметра:

- `type`: Тип, который нужно проанализировать.
- `flags`: Флаги, определяющие, какие элементы типа следует извлечь (конструкторы, методы, свойства, поля).

``` csharp
var analyzer = new TypeAnalyzer();
var result = analyzer.Analyze(typeof(MyClass), TypeAnalysisFlags.Methods | TypeAnalysisFlags.Properties);
```

В этом примере будут извлечены только методы и свойства типа `MyClass`.

#### Использование флагов

Флаги `TypeAnalysisFlags` позволяют гибко настраивать анализ:

- `Constructors`: Извлекает конструкторы типа.
- `Methods`: Извлекает методы типа.
- `Properties`: Извлекает свойства типа.
- `Fields`: Извлекает поля типа.
- `All`: Извлекает все элементы типа.

Если вам нужны только определенные элементы, комбинируйте флаги с помощью оператора |:

``` csharp
var flags = TypeAnalysisFlags.Constructors | TypeAnalysisFlags.Fields;
var result = analyzer.Analyze(typeof(MyClass), flags);
```

#### Работа с результатами

Результат анализа возвращается в виде объекта `TypeAnalysisResult`, который содержит коллекции:

- `Constructors`: Список конструкторов.
- `Methods`: Список методов.
- `Properties`: Список свойств.
- `Fields`: Список полей.

Эти коллекции можно использовать для дальнейшей обработки. Например:

``` csharp
foreach (var method in result.Methods)
{
    Console.WriteLine($"Method: {method.Name}");
}
```

#### Кэширование

`TypeAnalyzer` автоматически кэширует результаты анализа для каждого типа.
Это позволяет избежать повторного анализа одного и того же типа, что повышает производительность.

Если вам нужно очистить кэш (например, при тестировании), вы можете сделать это через рефлексию:

``` csharp
typeof(TypeAnalyzer)
    .GetField("Cache", BindingFlags.Static | BindingFlags.NonPublic)?
    .SetValue(null, new Dictionary<Type, TypeAnalysisResult>());
```

### Рекомендации

- Оптимизация производительности
  - Используйте минимальный набор флагов, чтобы извлекать только те данные, которые действительно нужны.
  - Если вы работаете с одним и тем же типом несколько раз, полагайтесь на кэширование, чтобы избежать лишних вычислений.
- Обработка пустых типов
  - Если анализируемый тип не содержит элементов (например, пустой класс), соответствующие коллекции в `TypeAnalysisResult` будут пустыми. Убедитесь, что ваш код корректно обрабатывает такие случаи.
- Проверка входных данных
  - Перед вызовом метода `Analyze` убедитесь, что передаваемый тип не равен `null`. В противном случае будет выброшено исключение `ArgNullException`.
- Тестирование
  - Очищайте кэш перед каждым тестом, чтобы избежать влияния предыдущих запусков.
  - Проверяйте как положительные сценарии (тип с различными элементами), так и отрицательные (пустой тип, неверные флаги).

### Пример использования

#### Полный анализ типа

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

#### Выборочный анализ

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

Factory - это реализация интерфейса `IFactory`, которая создает объекты с помощью функции-фабрики.

### Основные возможности

- Создание объектов с помощью делегата `Func<object>`
- Автоматическое освобождение ресурсов через интерфейс `IDisposable`
- Проверка входных параметров на null

### Рекомендации

1. Используйте Factory, когда:
   - Нужно инкапсулировать логику создания объектов
   - Логика создания объекта может меняться во время выполнения
   - Требуется отложенное создание объектов

2. Избегайте использования Factory, когда:
   - Логика создания объекта проста и не требует дополнительных действий
   - Объект должен быть создан немедленно
   - Требуется создание нескольких разных типов объектов

### Пример использования

```csharp
// Создание фабрики для строк
var factory = new Factory(() => "Hello, World!");

// Получение объекта
var message = (string)factory.Create();

// Создание фабрики для сложных объектов
var complexFactory = new Factory(() =>
{
    var settings = new ComplexObject();
    settings.Initialize();
    return settings;
});

// Использование в using для автоматического освобождения ресурсов
using (var factory = new Factory(() => new DisposableObject()))
{
    var obj = factory.Create();
    // Работа с объектом
}
```
