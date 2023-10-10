using Mafin.Configuration.Attributes;

namespace Mafin.Configuration.Models;

/// <summary>
/// Represents settings for the Mafin.Configuration module.
/// </summary>
[ConfigurationSection("configuration")]
internal class ModuleConfig
{
    /// <summary>
    /// Gets or sets value with base configuration files directory.
    /// </summary>
    public string? Directory { get; set; }

    /// <summary>
    /// Gets or sets value with file extensions to be loaded.
    /// </summary>
    public List<string> FileExtensions { get; set; } = new();

    /// <summary>
    /// Gets or sets value with list of files to be loaded.
    /// </summary>
    public List<string> Files { get; set; } = new();
}
