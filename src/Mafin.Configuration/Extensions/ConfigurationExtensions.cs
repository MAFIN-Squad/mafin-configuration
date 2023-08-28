using Mafin.Configuration.Attributes;
using Microsoft.Extensions.Configuration;

namespace Mafin.Configuration.Extensions;

/// <summary>
/// Provides extensions to <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets a configuration sub-section with the specified key.
    /// </summary>
    /// <typeparam name="T">Configuration sub-section type.</typeparam>
    /// <param name="configuration">The <see cref="IConfiguration"/> with section.</param>
    /// <returns>The new instance of <typeparamref name="T"/> if successful, default(<typeparamref name="T"/>) otherwise.</returns>
    /// <exception cref="ArgumentNullException">Throw when <paramref name="configuration"/> is null.</exception>
    public static T? GetSection<T>(this IConfiguration configuration)
    {
        var pathAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(ConfigurationSectionAttribute)) as ConfigurationSectionAttribute;
        var path = pathAttribute?.SectionPath ?? string.Empty;
        return configuration.GetSection<T>(path);
    }

    /// <summary>
    /// Gets a configuration sub-section with the specified key.
    /// </summary>
    /// <typeparam name="T">Configuration sub-section type.</typeparam>
    /// <param name="configuration">The <see cref="IConfiguration"/> with section.</param>
    /// <param name="key">>The key of the configuration section.</param>
    /// <returns>The new instance of <typeparamref name="T"/> if successful, default(<typeparamref name="T"/>) otherwise.</returns>
    /// <exception cref="ArgumentNullException">Throw when <paramref name="configuration"/> is null.</exception>
    /// <exception cref="ArgumentException">Throws when <paramref name="key"/> is null or empty.</exception>
    public static T? GetSection<T>(this IConfiguration configuration, string key)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
        }

        return configuration.GetSection(key).Get<T>();
    }
}
