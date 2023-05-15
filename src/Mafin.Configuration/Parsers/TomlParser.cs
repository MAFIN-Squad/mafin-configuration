using System.Globalization;
using Microsoft.Extensions.Configuration;
using Tomlyn;
using Tomlyn.Syntax;

namespace Mafin.Configuration.Parsers;

/// <summary>
/// Represents a parser for the TOML format.
/// </summary>
internal class TomlParser
{
    private readonly IDictionary<string, string?> _data = new SortedDictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Parses <paramref name="value"/> from TOML format.
    /// </summary>
    /// <param name="value"> <see cref="string"/> in TOML format. </param>
    /// <returns> Parsed TOML <paramref name="value"/>. </returns>
    public IDictionary<string, string?> Parse(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value), "Unable to parse an empty string");
        }

        var document = Toml.Parse(value);
        VisitDocument(document);

        return _data;
    }

    private void VisitDocument(DocumentSyntax document)
    {
        VisitSyntaxNode(document.KeyValues, new Stack<string>());
        VisitSyntaxNode(document.Tables, new Stack<string>());
    }

    private void VisitSyntaxNode(SyntaxNode node, Stack<string> path)
    {
        if (node is TableSyntax tableNode)
        {
            ParseTableNode(tableNode, path);
        }
        else if (node is ArraySyntax arrayNode)
        {
            ParseArrayNode(arrayNode, path);
        }
        else if (node is ArrayItemSyntax arrayItemSyntax)
        {
            ParseArrayItemNode(arrayItemSyntax, path);
        }
        else if (node is SyntaxList nodes)
        {
            ParseListNode(nodes, path);
        }
        else if (node is KeyValueSyntax keyValueNode)
        {
            ParseKeyValueNode(keyValueNode, path);
        }
        else if (node is ValueSyntax valueNode)
        {
            ParseValueNode(valueNode, path);
        }
    }

    private void ParseKeyValueNode(KeyValueSyntax node, Stack<string> path)
    {
        path.Push(node.Key?.ToString().Trim() ?? string.Empty);

        if (node.Value is not null)
        {
            VisitSyntaxNode(node.Value, path);
        }

        path.Pop();
    }

    private void ParseValueNode(ValueSyntax node, Stack<string> path)
    {
        var nodePath = ConfigurationPath.Combine(path.Reverse());
        _data[nodePath] = node.ToString().Trim('\"');
    }

    private void ParseListNode(SyntaxList node, Stack<string> path)
    {
        dynamic nodes = node;
        foreach (var keyValue in nodes)
        {
            VisitSyntaxNode(keyValue, path);
        }
    }

    private void ParseArrayNode(ArraySyntax node, Stack<string> path)
    {
        using var enumerator = node.Items.GetEnumerator();
        int i = 0;
        while (enumerator.MoveNext())
        {
            var value = enumerator.Current;

            path.Push(i.ToString(CultureInfo.CurrentCulture));
            VisitSyntaxNode(value, path);
            path.Pop();
            i++;
        }
    }

    private void ParseArrayItemNode(ArrayItemSyntax node, Stack<string> path)
    {
        if (node.Value is not null)
        {
            VisitSyntaxNode(node.Value, path);
        }
    }

    private void ParseTableNode(TableSyntax node, Stack<string> path)
    {
        path.Push(node.Name?.ToString() ?? string.Empty);
        VisitSyntaxNode(node.Items, path);
        path.Pop();
    }
}
