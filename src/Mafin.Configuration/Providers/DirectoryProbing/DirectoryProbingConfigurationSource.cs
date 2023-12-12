using Mafin.Configuration.Extensions;
using Mafin.Configuration.Providers.Toml;
using Mafin.Configuration.Providers.Yaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Xml;

namespace Mafin.Configuration.Providers.DirectoryProbing;

/// <summary>
/// Directory Probing <see cref="FileConfigurationSource"/>.
/// </summary>
public class DirectoryProbingConfigurationSource : FileConfigurationSource
{
    /// <summary>
    /// A constant containing a pattern corresponding to all the files.
    /// </summary>
    public const string AllFilesPattern = "*.*";

    private static readonly Dictionary<string, Func<FileConfigurationSource>> DefaultFormatSourceMap = new()
    {
        ["json"] = () => new JsonConfigurationSource(),
        ["xml"] = () => new XmlConfigurationSource(),
        ["ini"] = () => new IniConfigurationSource(),
        ["yaml"] = () => new YamlConfigurationSource(),
        ["yml"] = () => new YamlConfigurationSource(),
        ["toml"] = () => new TomlConfigurationSource(),
    };

    private string _baseDirectory = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryProbingConfigurationSource"/> class.
    /// </summary>
    public DirectoryProbingConfigurationSource()
    {
        SearchOption = SearchOption.TopDirectoryOnly;
        FormatSourceMap = DefaultFormatSourceMap;
        BaseDirectory = Environment.CurrentDirectory;
        FilePathPatterns = new[] { AllFilesPattern };
    }

    /// <summary>
    /// Gets or sets the value which shows the depth of the directory probe.
    /// </summary>
    public SearchOption SearchOption { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="List{String}"/> with patterns of the name of the file to be probed.
    /// </summary>
    public IReadOnlyList<string> FilePathPatterns { get; set; }

    /// <summary>
    /// Gets the value with dictionary that contains
    /// the relation of the file format to the function that creating the corresponding <see cref="FileConfigurationSource"/>.
    /// </summary>
    public Dictionary<string, Func<FileConfigurationSource>> FormatSourceMap { get; }

    /// <summary>
    /// Gets list of <see cref="FileConfigurationSource"/>.
    /// </summary>
    public IList<FileConfigurationSource> Sources => CreateConfigurationSources(GetConfigurationFiles());

    /// <summary>
    /// Gets or sets base directory for probing.
    /// </summary>
    public string BaseDirectory
    {
        get => _baseDirectory;
        set
        {
            value = value.AddDirectorySeparatorChar();

            if (value.IsPathFullyQualified())
            {
                _baseDirectory = value;
            }
            else
            {
                _baseDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, value));
            }
        }
    }

    /// <inheritdoc/>
    public override IConfigurationProvider Build(IConfigurationBuilder builder) => new DirectoryProbingConfigurationProvider(this);

    private List<string> GetConfigurationFiles()
    {
        List<string> result = [];

        foreach (var pattern in FilePathPatterns)
        {
            if (pattern.IsPathFullyQualified())
            {
                result.Add(pattern);
            }
            else
            {
                result.AddRange(Directory.GetFiles(BaseDirectory, pattern, SearchOption));
            }
        }

        return result;
    }

    private List<FileConfigurationSource> CreateConfigurationSources(IEnumerable<string> files)
    {
        List<FileConfigurationSource> result = [];

        foreach (var filePath in files)
        {
            var extension = System.IO.Path.GetExtension(filePath).TrimStart('.');

            if (FormatSourceMap.TryGetValue(extension, out var sourceFactory))
            {
                var source = sourceFactory.Invoke();
                source.Path = filePath;
                source.ReloadOnChange = ReloadOnChange;
                source.Optional = Optional;
                source.ResolveFileProvider();

                result.Add(source);
            }
        }

        return result;
    }
}
