namespace Mafin.Configuration.Attributes;

/// <summary>
/// Represents an attribute that contains the path to the root of the section with the configuration.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ConfigurationSectionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationSectionAttribute"/> class.
    /// </summary>
    /// <param name="sectionPath"> the path to the root of the section. </param>
    public ConfigurationSectionAttribute(string sectionPath)
    {
        if (string.IsNullOrEmpty(sectionPath))
        {
            throw new ArgumentException($"'{nameof(sectionPath)}' cannot be null or empty.", nameof(sectionPath));
        }

        SectionPath = sectionPath;
    }

    /// <summary>
    /// Gets the path to the root of the section with the configuration.
    /// </summary>
    public string SectionPath { get; }
}
