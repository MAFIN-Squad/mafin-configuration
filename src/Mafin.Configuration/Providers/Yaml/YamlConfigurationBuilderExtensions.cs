using Mafin.Configuration.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.FileProviders;
using YamlDotNet.Core;

namespace Mafin.Configuration.Providers.Yaml;

/// <summary>
/// Provides extensions to <see cref="IConfigurationBuilder"/> for adding <see cref="YamlConfigurationSource"/>.
/// </summary>
public static class YamlConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path)
    {
        return AddYamlFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
    }

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path, bool optional)
    {
        return AddYamlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
    }

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
    {
        return AddYamlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
    }

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, IFileProvider? provider, string path, bool optional, bool reloadOnChange)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder), $"'{nameof(builder)}' cannot be null");
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        return builder.AddYamlFile(s =>
        {
            s.FileProvider = provider;
            s.Path = path;
            s.Optional = optional;
            s.ReloadOnChange = reloadOnChange;
            s.ResolveFileProvider();
        });
    }

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configureSource">Configures the source.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, Action<YamlConfigurationSource> configureSource)
        => builder.Add(configureSource);

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="stream">The <see cref="Stream"/> to read the json configuration data from.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddYamlStream(this IConfigurationBuilder builder, Stream stream)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder), $"'{nameof(builder)}' cannot be null");
        }

        IDictionary<string, string?> data;

        try
        {
            data = new YamlParser().Parse(stream);
        }
        catch (YamlException e)
        {
            throw new FormatException("Unable to parse configuration file in YAML format.", e);
        }

        var source = new MemoryConfigurationSource() { InitialData = data };
        return builder.Add(source);
    }
}
