using Mafin.Configuration.Extensions;
using Mafin.Configuration.Models;
using Mafin.Configuration.Providers.DirectoryProbing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Mafin.Configuration.Providers.SettingsFile;

/// <summary>
/// A settings file based <see cref="ConfigurationProvider"/>.
/// </summary>
public class SettingsFileConfigurationProvider : IConfigurationProvider, IDisposable
{
    private readonly SettingsFileConfigurationSource _source;
    private IConfiguration? _config;
    private ModuleConfig? _providerConfig;
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsFileConfigurationProvider"/> class.
    /// </summary>
    /// <param name="source">The source settings.</param>
    public SettingsFileConfigurationProvider(SettingsFileConfigurationSource source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source), $"'{nameof(source)}' cannot be null");
    }

    /// <summary>
    /// Gets the inner configuration.
    /// </summary>
    public IConfiguration Configuration
    {
        get
        {
            if (_config is null)
            {
                Load();
            }

            return _config!;
        }
    }

    /// <summary>
    /// Tries to get a configuration value for the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The result.</param>
    /// <returns><see langword="true"/> if a value for the specified key was found, otherwise <see langword="false"/>.</returns>
    public bool TryGet(string key, out string? value)
    {
        value = Configuration[key];
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Sets a configuration value for the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Set(string key, string? value) => Configuration[key] = value;

    /// <summary>
    /// Returns a change token if this provider supports change tracking, null otherwise.
    /// </summary>
    /// <returns>The change token.</returns>
    public IChangeToken GetReloadToken() => Configuration.GetReloadToken();

    /// <summary>
    /// Returns the immediate descendant configuration keys for a given parent path based on this
    /// <see cref="IConfigurationProvider"/>s data and the set of keys returned by all the preceding
    /// <see cref="IConfigurationProvider"/>s.
    /// </summary>
    /// <param name="earlierKeys">The child keys returned by the preceding providers for the same parent path.</param>
    /// <param name="parentPath">The parent path.</param>
    /// <returns>The child keys.</returns>
    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        IConfiguration section = parentPath == null
            ? Configuration
            : Configuration.GetSection(parentPath);

        List<string> keys = [];
        keys.AddRange(section.GetChildren().Select(child => child.Key));
        keys.AddRange(earlierKeys);

        return keys;
    }

    /// <summary>
    /// Loads configuration values from the source represented by this <see cref="IConfigurationProvider"/>.
    /// </summary>
    public void Load()
    {
        try
        {
            _providerConfig = LoadModuleConfig();
            _config = LoadConfig();
        }
        catch (Exception e)
        {
            throw new FormatException("Unable to load configuration from module settings file.", e);
        }
    }

    /// <summary>
    /// Disposes <see cref="DirectoryProbingConfigurationProvider"/> instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes <see cref="DirectoryProbingConfigurationProvider"/> instance.
    /// </summary>
    /// <param name="disposing">
    ///     The disposing parameter is a <see langword="bool"/> that indicates whether
    ///     the method call comes from a <see cref="Dispose()"/> method (its value is <see langword="true"/>)
    ///     or from a finalizer (its value is <see langword="false"/>).
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                (_config as IDisposable)?.Dispose();
            }

            _disposedValue = true;
        }
    }

    private ModuleConfig LoadModuleConfig()
    {
        var configuration = new ConfigurationBuilder()
            .AddDirectoryProbing(new[] { _source.Path! })
            .Build();

        var moduleConfig = configuration.GetSection<ModuleConfig>();

        return moduleConfig is null
            ? throw new FileLoadException($"Unable to load configuration from {_source.Path}")
            : moduleConfig;
    }

    private IConfiguration LoadConfig()
    {
        var basePath = _providerConfig!.Directory ?? Environment.CurrentDirectory;
        List<string> filter = [];

        if (_providerConfig.FileExtensions?.Count > 0)
        {
            var extensions = _providerConfig.FileExtensions.Select(x => Path.ChangeExtension("*", x));
            filter.AddRange(extensions);
        }

        if (_providerConfig.Files?.Count > 0)
        {
            filter.AddRange(_providerConfig.Files);
        }

        if (filter.Count is 0)
        {
            filter.Add("*.*");
        }

        var configuration = new ConfigurationBuilder()
            .AddDirectoryProbing(filter, basePath, formatSourceMap: null, SearchOption.TopDirectoryOnly, provider: _source.FileProvider, _source.Optional, _source.ReloadOnChange)
            .Build();

        return configuration;
    }
}
