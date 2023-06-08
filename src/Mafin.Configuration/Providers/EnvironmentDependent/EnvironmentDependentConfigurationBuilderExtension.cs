using System.Reflection;
using Mafin.Configuration.Meta;
using Mafin.Configuration.Providers.DirectoryProbing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Mafin.Configuration.Providers.EnvironmentDependent;

/// <summary>
/// Extension methods for <see cref="IConfigurationBuilder"/> to add environment dependent configuration.
/// </summary>
public static class EnvironmentDependentConfigurationBuilderExtension
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

        return AddEnvironmentDependentConfiguration(builder, EnvironmentConfigurationSplitPrinciple.ByFileName, environment, Environment.CurrentDirectory, optional: false, reloadOnChange: false, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="principle">The <see cref="EnvironmentConfigurationSplitPrinciple"/>.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplitPrinciple principle,
        string baseDirectory)
    {
        var environment = Assembly.GetCallingAssembly()?
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?
            .Configuration!;

        return AddEnvironmentDependentConfiguration(builder, principle, environment, baseDirectory, optional: false, reloadOnChange: false, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="principle">The <see cref="EnvironmentConfigurationSplitPrinciple"/>.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplitPrinciple principle,
        string baseDirectory,
        bool optional)
    {
        var environment = Assembly.GetCallingAssembly()?
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?
            .Configuration!;

        return AddEnvironmentDependentConfiguration(builder, principle, environment, baseDirectory, optional, reloadOnChange: false, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="principle">The <see cref="EnvironmentConfigurationSplitPrinciple"/>.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplitPrinciple principle,
        string baseDirectory,
        bool optional,
        bool reloadOnChange)
    {
        var environment = Assembly.GetCallingAssembly()?
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?
            .Configuration!;

        return AddEnvironmentDependentConfiguration(builder, principle, environment, baseDirectory, optional, reloadOnChange, SearchOption.AllDirectories, provider: null);
    }

    /// <summary>
    /// Adds a environment dependent configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="principle">The <see cref="EnvironmentConfigurationSplitPrinciple"/>.</param>
    /// <param name="environment">The <see cref="string"/> with environment.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <param name="searchOption">The <see cref="SearchOption"/> which shows the depth of the directory probe.</param>
    /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEnvironmentDependentConfiguration(
        this IConfigurationBuilder builder,
        EnvironmentConfigurationSplitPrinciple principle,
        string environment,
        string baseDirectory,
        bool optional,
        bool reloadOnChange,
        SearchOption searchOption,
        IFileProvider? provider)
    {
        switch (principle)
        {
            case EnvironmentConfigurationSplitPrinciple.ByFolder:
                builder.AddDirectoryProbing(new[] { Path.Combine(environment, "*.*") }, baseDirectory, formatSourceMap: null, searchOption, provider, optional, reloadOnChange);
                break;
            case EnvironmentConfigurationSplitPrinciple.ByFileName:
                builder.AddDirectoryProbing(new[] { $"*.{environment}.*" }, baseDirectory, formatSourceMap: null, searchOption, provider, optional, reloadOnChange);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(principle), principle, "Unsupported value");
        }

        return builder;
    }
}
