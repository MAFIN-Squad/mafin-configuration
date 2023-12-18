using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Mafin.Configuration.Providers.DirectoryProbing;

/// <summary>
/// Directory Probing <see cref="ConfigurationProvider"/>.
/// </summary>
public class DirectoryProbingConfigurationProvider : IConfigurationProvider, IDisposable
{
    private readonly DirectoryProbingConfigurationSource _source;
    private IConfiguration? _config;
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryProbingConfigurationProvider"/> class.
    /// </summary>
    /// <param name="source">The source settings.</param>
    public DirectoryProbingConfigurationProvider(DirectoryProbingConfigurationSource source)
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
    /// Tries to get a configuration value for the specified <paramref name="key"/>.
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
    /// Sets a configuration value for the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Set(string key, string? value) => Configuration[key] = value;

    /// <summary>
    /// Returns a change token if this provider supports change tracking, <see langword="null"/> otherwise.
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
        var section = parentPath == null
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
        ConfigurationBuilder builder = new();
        foreach (var source in _source.Sources)
        {
            builder.Add(source);
        }

        _config = builder.Build();
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
}
