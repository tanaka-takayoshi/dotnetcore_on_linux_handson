# Getting Started with .NET Core Project

## 1. Create Project and Run with Visual Studio Code

1. Type below command and then Visual Studio Code will open.

 ```
 $ mkdir demo
 $ cd demo
 $ dotnet new
 $ code .
 ```

2. Press `Yes` to download C# extension resources and press `Restore` to restore dotnet packages.

3. Press `F5` key to start debugging.


## 2. Adding project via NuGet

1. Edit a project.json to add Newtonsoft.JSON.

2. Edit a Program.cs. Let's type code instead of copy&paste. VS Code help you with Intelisense.

```csharp
var jsonObject = new 
{
    Name = "Yamada",
    Age = 17,
    Hobbies = new[]{"Reading", "Baseball"}
};
Console.WriteLine(JsonConvert.SerializeObject(jsonObject));
```

