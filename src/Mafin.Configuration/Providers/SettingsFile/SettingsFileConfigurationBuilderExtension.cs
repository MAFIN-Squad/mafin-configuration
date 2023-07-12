using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Mafin.Configuration.Providers.SettingsFile;

/// <summary>
/// Extension methods for <see cref="IConfigurationBuilder"/> to add <see cref="SettingsFileConfigurationSource"/>.
/// </summary>
public static class SettingsFileConfigurationBuilderExtension
{
    private const string ConfigurationFileName = "Mafin.Configuration.json";

    private static string DefaultSettingsFile =>
        Path.Combine(Environment.CurrentDirectory, ConfigurationFileName);

    /// <summary>
    /// Adds configuration source parameters from module settings file to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder LoadSettingsFile(this IConfigurationBuilder builder)
    {
        return LoadSettingsFile(builder, DefaultSettingsFile);
    }

    /// <summary>
    /// Adds configuration source parameters from module settings file to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder LoadSettingsFile(this IConfigurationBuilder builder, string path)
    {
        return LoadSettingsFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
    }

    /// <summary>
    /// Adds configuration source parameters from module settings file to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder LoadSettingsFile(this IConfigurationBuilder builder, string path, bool optional)
    {
        return LoadSettingsFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
    }

    /// <summary>
    /// Adds configuration source parameters from module settings file to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder LoadSettingsFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
    {
        return LoadSettingsFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
    }

    /// <summary>
    /// Adds configuration source parameters from module settings file to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
    /// <param name="path">Path relative to the base path stored in
    /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder LoadSettingsFile(this IConfigurationBuilder builder, IFileProvider? provider, string path, bool optional, bool reloadOnChange)
#pragma warning disable PH2071 // Avoid Duplicate Code
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder), $"'{nameof(builder)}' cannot be null");
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        return builder.LoadSettingsFile(s =>
        {
            s.FileProvider = provider;
            s.Path = path;
            s.Optional = optional;
            s.ReloadOnChange = reloadOnChange;
            s.ResolveFileProvider();
#pragma warning restore PH2071 // TODO: Refactor code to remove duplication
        });
    }

    /// <summary>
    /// Adds configuration source parameters from module settings file to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configureSource">Configures the source.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder LoadSettingsFile(this IConfigurationBuilder builder, Action<SettingsFileConfigurationSource> configureSource)
        => builder.Add(configureSource);
}
