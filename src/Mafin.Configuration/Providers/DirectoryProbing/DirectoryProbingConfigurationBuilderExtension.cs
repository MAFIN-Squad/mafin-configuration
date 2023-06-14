using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Mafin.Configuration.Providers.DirectoryProbing;

/// <summary>
/// Extension methods for <see cref="IConfigurationBuilder"/> to add <see cref="DirectoryProbingConfigurationProvider"/>.
/// </summary>
public static class DirectoryProbingConfigurationBuilderExtension
{
    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(this IConfigurationBuilder builder)
    {
        return AddDirectoryProbing(builder, DirectoryProbingConfigurationSource.AllFilesPattern);
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="filePathPattern">The <see cref="string"/> with pattern of the name of the file to be probed.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(
        this IConfigurationBuilder builder,
        string filePathPattern)
    {
        return AddDirectoryProbing(builder, new[] { filePathPattern });
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="filePathPatterns">The <see cref="IEnumerable{String}"/> with patterns of the name of the file to be probed.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(
        this IConfigurationBuilder builder,
        IEnumerable<string> filePathPatterns)
    {
        return AddDirectoryProbing(builder, filePathPatterns, Environment.CurrentDirectory);
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="filePathPattern">The <see cref="string"/> with pattern of the name of the file to be probed.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(
        this IConfigurationBuilder builder,
        string filePathPattern,
        string baseDirectory)
    {
        return AddDirectoryProbing(builder, new[] { filePathPattern }, baseDirectory);
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="filePathPatterns">The <see cref="IEnumerable{String}"/> with patterns of the name of the file to be probed.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(
        this IConfigurationBuilder builder,
        IEnumerable<string> filePathPatterns,
        string baseDirectory)
    {
        return AddDirectoryProbing(builder, filePathPatterns, baseDirectory, formatSourceMap: null);
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="filePathPatterns">The <see cref="IEnumerable{String}"/> with patterns of the name of the file to be probed.</param>
    /// <param name="formatSourceMap">The dictionary that contains the relation of the file format to the function that creating the corresponding <see cref="FileConfigurationSource"/>.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(
        this IConfigurationBuilder builder,
        IEnumerable<string> filePathPatterns,
        Dictionary<string, Func<FileConfigurationSource>>? formatSourceMap)
    {
        return AddDirectoryProbing(builder, filePathPatterns, Environment.CurrentDirectory, formatSourceMap);
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="filePathPatterns">The <see cref="IEnumerable{String}"/> with patterns of the name of the file to be probed.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="formatSourceMap">The dictionarythat contains the relation of the file format to the function that creating the corresponding <see cref="FileConfigurationSource"/>.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(
        this IConfigurationBuilder builder,
        IEnumerable<string> filePathPatterns,
        string baseDirectory,
        Dictionary<string, Func<FileConfigurationSource>>? formatSourceMap)
    {
        return AddDirectoryProbing(builder, filePathPatterns, baseDirectory, formatSourceMap, SearchOption.TopDirectoryOnly, provider: null, optional: false, reloadOnChange: false);
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="filePathPatterns">The <see cref="IEnumerable{String}"/> with patterns of the name of the file to be probed.</param>
    /// <param name="baseDirectory">The <see cref="string"/> with base directory.</param>
    /// <param name="formatSourceMap">The dictionary that contains the relation of the file format to the function that creating the corresponding <see cref="FileConfigurationSource"/>.</param>
    /// <param name="searchOption">The <see cref="SearchOption"/> which shows the depth of the directory probe.</param>
    /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(
        this IConfigurationBuilder builder,
        IEnumerable<string> filePathPatterns,
        string baseDirectory,
        Dictionary<string, Func<FileConfigurationSource>>? formatSourceMap,
        SearchOption searchOption,
        IFileProvider? provider,
        bool optional,
        bool reloadOnChange)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder), $"'{nameof(builder)}' cannot be null");
        }

        if (filePathPatterns == null)
        {
            throw new ArgumentNullException(nameof(filePathPatterns), $"'{nameof(filePathPatterns)}' cannot be null.");
        }

        filePathPatterns = filePathPatterns.Where(x => !string.IsNullOrWhiteSpace(x));

        if (!filePathPatterns.Any())
        {
            throw new ArgumentException($"'{nameof(filePathPatterns)}' cannot be empty.", nameof(filePathPatterns));
        }

        return builder.AddDirectoryProbing(s =>
        {
            s.FileProvider = provider;
            s.BaseDirectory = baseDirectory;
            s.FilePathPatterns = filePathPatterns.ToList();
            s.SearchOption = searchOption;

            if (formatSourceMap is not null)
            {
                foreach (var keyValuePair in formatSourceMap)
                {
                    s.FormatSourceMap[keyValuePair.Key] = keyValuePair.Value;
                }
            }

            s.Optional = optional;
            s.ReloadOnChange = reloadOnChange;
            s.ResolveFileProvider();
        });
    }

    /// <summary>
    /// Adds a directory probing configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configureSource">Action to Configure the source.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddDirectoryProbing(this IConfigurationBuilder builder, Action<DirectoryProbingConfigurationSource> configureSource)
        => builder.Add(configureSource);
}
