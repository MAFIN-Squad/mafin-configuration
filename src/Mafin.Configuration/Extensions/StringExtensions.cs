namespace Mafin.Configuration.Extensions;

/// <summary>
/// Provides extensions to <see cref="string"/>.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Adds <see cref="Path.DirectorySeparatorChar"/> to <paramref name="path"/>.
    /// </summary>
    /// <param name="path">Directory path.</param>
    /// <returns><paramref name="path"/> with <see cref="Path.DirectorySeparatorChar"/>.</returns>
    /// <exception cref="ArgumentException">Throws when <paramref name="path"/> null or whitespace.</exception>
    public static string AddDirectorySeparatorChar(this string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
        }

        if (!path.EndsWith(Path.DirectorySeparatorChar))
        {
            path += Path.DirectorySeparatorChar;
        }

        return path;
    }
}
