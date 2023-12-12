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

        if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            path += Path.DirectorySeparatorChar;
        }

        return path;
    }

    /// <summary>
    /// Checks whether <paramref name="path"/> is fully qualified.
    /// </summary>
    /// <param name="path"><see cref="string"/> to be checked.</param>
    /// <returns><see langword="true"/> if the <paramref name="path"/> is fully qualified.</returns>
    public static bool IsPathFullyQualified(this string path)
    {
        return Environment.OSVersion.Platform is not PlatformID.Win32NT
            ? Path.IsPathRooted(path)
            : path.Length >= 3
                && path[1] == Path.VolumeSeparatorChar
                && path[2] == Path.DirectorySeparatorChar
                && IsValidDriveChar(path[0]);
    }

    /// <summary>
    /// Checks whether <paramref name="value"/> is a valid drive letter.
    /// </summary>
    /// <param name="value">character to be checked.</param>
    /// <returns><see langword="true"/> if the given character is a valid drive letter.</returns>
    internal static bool IsValidDriveChar(char value) =>
        value is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');
}
