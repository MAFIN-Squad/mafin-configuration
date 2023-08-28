using System.Globalization;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Mafin.Configuration.Parsers;

/// <summary>
/// Represents a parser for the YAML format.
/// </summary>
internal class YamlParser
{
    private static readonly string[] NullValues = new[] { "null", "~" };

    private readonly IDictionary<string, string?> _data = new SortedDictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Parses <paramref name="unparsedStream"/> from YAML format.
    /// </summary>
    /// <param name="unparsedStream"> <see cref="Stream"/> in YAML format. </param>
    /// <returns> Parsed YAML <paramref name="unparsedStream"/>. </returns>
    public IDictionary<string, string?> Parse(Stream unparsedStream)
    {
        if (unparsedStream is null)
        {
            throw new ArgumentNullException(nameof(unparsedStream), "Unable to parse an empty stream");
        }

        _data.Clear();

        using var streamReader = new StreamReader(unparsedStream);
        var stream = new YamlStream();
        stream.Load(streamReader);

        if (stream.Documents.Any())
        {
            var rootNode = stream.Documents.Single().RootNode;
            VisitNode(rootNode, new Stack<string>());
        }

        return _data;
    }

    private static bool IsNullValue(YamlScalarNode node)
    {
        var isPlain = node.Style == ScalarStyle.Plain;
        var isNull = Array.Exists(NullValues, value => value.Equals(node.Value, StringComparison.OrdinalIgnoreCase));

        return isPlain && isNull;
    }

    private void VisitNode(YamlNode node, Stack<string> path)
    {
        switch (node)
        {
            case YamlScalarNode scalarNode:
                VisitScalarNode(scalarNode, path);
                break;
            case YamlMappingNode mappingNode:
                VisitMappingNode(mappingNode, path);
                break;
            case YamlSequenceNode sequenceNode:
                VisitSequenceNode(sequenceNode, path);
                break;
        }
    }

    private void VisitScalarNode(YamlScalarNode node, Stack<string> path)
    {
        var formattedPath = ConfigurationPath.Combine(path.Reverse());
        if (_data.ContainsKey(formattedPath))
        {
            throw new FormatException();
        }

        _data[formattedPath] = IsNullValue(node) ? null : node.Value;
    }

    private void VisitMappingNode(YamlMappingNode node, Stack<string> path)
    {
        foreach (var yamlNodePair in node.Children)
        {
            VisitNodePair(yamlNodePair, path);
        }
    }

    private void VisitSequenceNode(YamlSequenceNode node, Stack<string> path)
    {
        for (int i = 0; i < node.Children.Count; i++)
        {
            path.Push(i.ToString(CultureInfo.CurrentCulture));
            VisitNode(node.Children[i], path);
            path.Pop();
        }
    }

    private void VisitNodePair(KeyValuePair<YamlNode, YamlNode> yamlNodePair, Stack<string> path)
    {
        var nodePath = ((YamlScalarNode)yamlNodePair.Key).Value;

        path.Push(nodePath ?? string.Empty);
        VisitNode(yamlNodePair.Value, path);
        path.Pop();
    }
}
