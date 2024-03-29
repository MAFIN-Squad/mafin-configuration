using Mafin.Configuration.Parsers;
using Microsoft.Extensions.Configuration;

namespace Mafin.Configuration.Providers.Toml;

/// <summary>
/// A TOML file based <see cref="FileConfigurationProvider"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TomlConfigurationProvider"/> class.
/// </remarks>
/// <param name="source">The source settings.</param>
public class TomlConfigurationProvider(FileConfigurationSource source) : FileConfigurationProvider(source)
{
    /// <inheritdoc/>
    public override void Load(Stream stream)
    {
        try
        {
            using StreamReader reader = new(stream);
            TomlParser parser = new();
            Data = parser.Parse(reader.ReadToEnd());
        }
        catch (Exception e)
        {
            throw new FormatException("Unable to parse configuration file in TOML format.", e);
        }
    }
}
