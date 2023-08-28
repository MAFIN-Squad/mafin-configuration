namespace Mafin.Configuration.Meta;

/// <summary>
/// Represents the principle of separation of environment dependent configurations.
/// </summary>
public enum EnvironmentConfigurationSplit
{
    /// <summary>
    /// Split configuration by folder.
    /// </summary>
    ByFolder,

    /// <summary>
    /// Split configuration by file name.
    /// </summary>
    ByFileName,
}
