using Microsoft.Extensions.Configuration;

namespace Mafin.Configuration.Providers.SettingsFile;

/// <summary>
/// A settings file based <see cref="FileConfigurationSource"/>.
/// </summary>
public class SettingsFileConfigurationSource : FileConfigurationSource
{
    /// <inheritdoc/>
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new SettingsFileConfigurationProvider(this);
    }
}
