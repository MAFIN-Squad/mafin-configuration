# MAFIN Configuration Module

---

---
## Examples

The example below shows a small code samples using this library:

#### config.yml Configuration file
``` yml
example:
  exampleSection:
    namedProperty: value
```

#### Configuration model

```cs

[ConfigurationSection("example:exampleSection")]
public class MyConfigModel
{
    public string NamedProperty { get; set; }
}
```

### Example 1: 

Given the simple class and configuration file above, we can place the data from `config.yml` file into a new instance of `MyConfigModel` class:

```cs
var configuration = new ConfigurationBuilder()
    .AddYamlFile($"config.yml")
    .Build();

var appConfig = config.GetSection<MyConfigModel>();
Console.WriteLine(appConfig.NamedProperty); // returns "value"
```

### Example 2: 

We can do the same using `DirectoryProbing` feature if we can't specify the file or list if files exactly, but find it using a pattern.

```cs
var configuration = new ConfigurationBuilder()
    .AddDirectoryProbing("*.yml")
    .Build();

var appConfig = config.GetSection<MyConfigModel>();
Console.WriteLine(appConfig.NamedProperty); // returns "value"
```

### Example 3:

#### Mafin.Configuration.json
```json
{
  "mafin": {
    "configuration": {
      "fileExtensions": [ "yml", "yaml" ]
    }
  }
}
```

We can set up this configuration library using an external configuration file (an example of such a file can be seen above):

```cs
var configuration = new ConfigurationBuilder()
    .LoadSettingsFile()
    .Build();

var appConfig = config.GetSection<MyConfigModel>();
Console.WriteLine(appConfig.NamedProperty); // returns "value"
```

### Environment dependent examples

We can also load the configuration files depending on the build configuration (e.g. Debug/Release or DEV/QA/UAT):

#### `Debug` Configuration file
``` yml
example:
  exampleSection:
    namedProperty: debug
```

#### `Release` Configuration file
``` yml
example:
  exampleSection:
    namedProperty: release
```

### Example 4:

Example where configurations are divided by folders:
```
 |
 +- config
 |  |
 |  +- debug
 |  |  +- config.yml
 |  |  +- other_config.yml
 |  |
 |  +- release
 |     +- config.yml 
 |     +- other_config.yml
```

```cs
var configuration = new ConfigurationBuilder()
    .AddEnvironmentDependentConfiguration(EnvironmentConfigurationSplitPrinciple.ByFolder, "config")
    .Build();

var appConfig = config.GetSection<MyConfigModel>();
Console.WriteLine(appConfig.NamedProperty); // returns "debug" when configuration Debug and "release" when configuration Release
```

### Example 5:

Example where configurations are divided by filenames
```
 |
 +- config
 |  +- config.debug.yml
 |  +- other_config.debug.yml
 |  +- config.release.yml
 |  +- other_config.release.yml
```

Using the file above we can set up the configuration module using an external file with the settings

```cs
var configuration = new ConfigurationBuilder()
    .AddEnvironmentDependentConfiguration(EnvironmentConfigurationSplitPrinciple.ByFileName, "config")
    .Build();

var appConfig = config.GetSection<MyConfigModel>();
Console.WriteLine(appConfig.NamedProperty); // returns "debug" when configuration Debug and "release" when configuration Release
```