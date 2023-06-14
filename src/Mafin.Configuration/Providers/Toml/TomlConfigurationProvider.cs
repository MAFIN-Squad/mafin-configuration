using Mafin.Configuration.Parsers;
using Microsoft.Extensions.Configuration;

namespace Mafin.Configuration.Providers.Toml;

/// <summary>
/// A TOML file based <see cref="FileConfigurationProvider"/>.
/// </summary>
public class TomlConfigurationProvider : FileConfigurationProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TomlConfigurationProvider"/> class.
    /// </summary>
    /// <param name="source">The source settings.</param>
    public TomlConfigurationProvider(FileConfigurationSource source)
        : base(source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source), $"'{nameof(source)}' cannot be null");
        }
    }

    /// <inheritdoc/>
    public override void Load(Stream stream)
    {
        try
        {
            using var reader = new StreamReader(stream);
            var parser = new TomlParser();
            Data = parser.Parse(reader.ReadToEnd());
        }
        catch (Exception e)
        {
            throw new FormatException("Unable to parse configuration file in TOML format.", e);
        }
    }
}
