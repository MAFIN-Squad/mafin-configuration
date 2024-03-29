using System.Reflection;
using Mafin.Configuration.Meta;
using Mafin.Configuration.Providers.DirectoryProbing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Mafin.Configuration.Providers.EnvironmentDependent;

/// <summary>
/// Provides extensions to <see cref="IConfigurationBuilder"/> for adding environment dependent configuration.
/// </summary>
public static class EnvironmentDependentConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(this IConfigurationBuilder builder)
    {
        var environment = Assembly.GetCallingAssembly()?
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?
            .Configuration!;

        return AddEnvironmentDependentConfiguration(builder, EnvironmentConfigurationSplit.ByFileName, environment, Environment.CurrentDirectory, optional: false, reloadOnChange: false, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="split">The <see cref="EnvironmentConfigurationSplit"/>.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplit split,
        string baseDirectory)
    {
        var environment = Assembly.GetCallingAssembly()?
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?
            .Configuration!;

        return AddEnvironmentDependentConfiguration(builder, split, environment, baseDirectory, optional: false, reloadOnChange: false, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="split">The <see cref="EnvironmentConfigurationSplit"/>.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplit split,
        string baseDirectory,
        bool optional)
    {
        var environment = Assembly.GetCallingAssembly()?
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?
            .Configuration!;

        return AddEnvironmentDependentConfiguration(builder, split, environment, baseDirectory, optional, reloadOnChange: false, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="split">The <see cref="EnvironmentConfigurationSplit"/>.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplit split,
        string baseDirectory,
        bool optional,
        bool reloadOnChange)
    {
        var environment = Assembly.GetCallingAssembly()?
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?
            .Configuration!;

        return AddEnvironmentDependentConfiguration(builder, split, environment, baseDirectory, optional, reloadOnChange, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="split">The <see cref="EnvironmentConfigurationSplit"/>.</param>
    /// <param name="environment">The <see cref="string"/> with environment.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <param name="searchOption">The <see cref="SearchOption"/> which shows the depth of the directory probe.</param>
    /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplit split,
        string environment,
        string baseDirectory,
        bool optional,
        bool reloadOnChange,
        SearchOption searchOption,
        IFileProvider? provider)
    {
        switch (split)
        {
            case EnvironmentConfigurationSplit.ByFolder:
                builder.AddDirectoryProbing(new[] { Path.Combine(environment, "*.*") }, baseDirectory, formatSourceMap: null, searchOption, provider, optional, reloadOnChange);
                break;
            case EnvironmentConfigurationSplit.ByFileName:
                builder.AddDirectoryProbing(new[] { $"*.{environment}.*" }, baseDirectory, formatSourceMap: null, searchOption, provider, optional, reloadOnChange);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(split), split, "Unsupported value");
        }

        return builder;
    }
}
