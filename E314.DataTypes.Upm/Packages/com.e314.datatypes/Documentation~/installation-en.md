# Installation

## Content tree

- [Installation](#installation)
  - [Content tree](#content-tree)
  - [Compatibility](#compatibility)
  - [Unity Package Manager. Git URL](#unity-package-manager-git-url)
    - [Unity Package Manager. OpenUPM](#unity-package-manager-openupm)
  - [NuGet](#nuget)

## Compatibility

- The module has been tested with Unity 2022.3 LTS and above.
- Compatible with .NET Standard 2.0 and above.

## Unity Package Manager. Git URL

```ps1
https://github.com/epishev-m/e314-datatypes.git?path=E314.DataTypes.Upm/Packages/com.e314.datatypes/#release/1.0.1
```

1. Open Window → Package Manager.
2. Click on + → Add package from git URL...
3. Enter the URL and click Add.

### Unity Package Manager. OpenUPM

```ps1
https://openupm.com/packages/com.e314.datatypes.html
```

1. Open Edit → Project Settings → Package Manager.
2. Register a new OpenUPM registry if it hasn't been done yet.
3. Add com.e314 to Scopes.
4. Click Apply.
5. Open Window → Package Manager.
6. Click on the + button → Add package by name...
7. Enter the Name com.e314.datatypes and Version 1.0.1.
8. Click Add.

## NuGet

```ps1
https://www.nuget.org/packages/E314.DataTypes
```

1. Open the command line.
2. Navigate to the directory containing the project file.
3. Run the command to install the NuGet package:

```sh
dotnet add package E314.DataTypes -v 1.0.1
```
