# Установка

## Содержание

- [Установка](#установка)
  - [Содержание](#содержание)
  - [Совместимость](#совместимость)
  - [Unity Package Manager. Git URL](#unity-package-manager-git-url)
  - [Unity Package Manager. OpenUPM](#unity-package-manager-openupm)
  - [NuGet](#nuget)

## Совместимость

- Модуль протестирован с Unity 2022.3 LTS и выше.
- Совместим с .NET Standard 2.0 и выше.

## Unity Package Manager. Git URL

```ps1
https://github.com/epishev-m/e314-datatypes.git?path=E314.DataTypes.Upm/Packages/com.e314.datatypes/#release/1.0.0
```

1. Открыть Window → Package Manager.
2. Нажать на + → Add package from git URL...
3. Ввести url и нажать Add.

## Unity Package Manager. OpenUPM

```ps1
https://openupm.com/packages/com.e314.datatypes.html
```

1. Открыть Edit → Project Settings → Package Manager.
2. Зарегистрировать новый реестр OpenUPM, если это еще не сделано.
3. Добавьте `com.e314` в  Scopes.
4. Нажать Apply.
5. Открыть Window → Package Manager.
6. Нажать на + button → Add package by name...
7. Введите Имя `com.e314.datatypes` и Версию `1.0.0`.
8. Нажмите Add.

## NuGet

```ps1
https://www.nuget.org/packages/E314.DataTypes
```

1. Открыть командную строку
2. Перейти в каталог, в котором находится файл проекта.
3. Выполнить команду для установки пакета NuGet:

```sh
dotnet add package E314.DataTypes -v 1.0.0
```
