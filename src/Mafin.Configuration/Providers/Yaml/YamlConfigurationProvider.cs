using Mafin.Configuration.Parsers;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;

namespace Mafin.Configuration.Providers.Yaml;

/// <summary>
/// A YAML file based <see cref="FileConfigurationProvider"/>.
/// </summary>
public class YamlConfigurationProvider : FileConfigurationProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="YamlConfigurationProvider"/> class.
    /// </summary>
    /// <param name="source">The source settings.</param>
    public YamlConfigurationProvider(FileConfigurationSource source)
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
            var parser = new YamlParser();
            Data = parser.Parse(stream);
        }
        catch (YamlException e)
        {
            throw new FormatException("Unable to parse configuration file in YAML format.", e);
        }
    }
}
