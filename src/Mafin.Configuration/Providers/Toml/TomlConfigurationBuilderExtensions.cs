using Mafin.Configuration.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.FileProviders;

namespace Mafin.Configuration.Providers.Toml;

/// <summary>
/// Provides extensions to <see cref="IConfigurationBuilder"/> for adding <see cref="TomlConfigurationSource"/>.
/// </summary>
public static class TomlConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path) =>
        AddTomlFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path, bool optional) =>
        AddTomlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange) =>
        AddTomlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);

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
    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, IFileProvider? provider, string path, bool optional, bool reloadOnChange)
    {
#pragma warning disable PH2071 // Avoid Duplicate Code
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder), $"'{nameof(builder)}' cannot be null");
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        return builder.AddTomlFile(s =>
        {
            s.FileProvider = provider;
            s.Path = path;
            s.Optional = optional;
            s.ReloadOnChange = reloadOnChange;
            s.ResolveFileProvider();
        });
#pragma warning restore PH2071 // TODO: Refactor code to remove duplication
    }

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configureSource">Configures the source.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, Action<TomlConfigurationSource> configureSource) =>
        builder.Add(configureSource);

    /// <summary>
    /// Adds a TOML configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="stream">The <see cref="Stream"/> to read the json configuration data from.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddTomlStream(this IConfigurationBuilder builder, Stream stream)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder), $"'{nameof(builder)}' cannot be null");
        }

        IDictionary<string, string?> data;
        try
        {
            using StreamReader reader = new(stream);
            data = new TomlParser().Parse(reader.ReadToEnd());
        }
        catch (Exception e)
        {
            throw new FormatException("Unable to parse configuration file in TOML format.", e);
        }

        MemoryConfigurationSource source = new() { InitialData = data };
        return builder.Add(source);
    }
}
