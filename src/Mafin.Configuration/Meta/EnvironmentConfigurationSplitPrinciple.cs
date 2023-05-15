namespace Mafin.Configuration
{
    /// <summary>
    /// Represents the principle of separation of environment dependent configurations.
    /// </summary>
    public enum EnvironmentConfigurationSplitPrinciple
    {
        /// <summary>
        /// Split configuration by folder.
        /// </summary>
        ByFolder,

        /// <summary>
        /// Split configuration by file name.
        /// </summary>
        ByFileName,
    }
}
