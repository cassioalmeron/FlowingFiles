using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace FlowingFiles.Core.Services;

public class PdfLayoutRenderer
{
    private const double DefaultCharWidthFactor = 0.5;
    private const double LineToleranceFactor = 0.3;
    private const double BlankLineGapFactor = 1.8;
    private const double SingleSpacingFactor = 1.2;

    public static string Render(PdfDocument document, double charWidthFactor = DefaultCharWidthFactor)
    {
        var pages = document.GetPages().Select(page => RenderPage(page, charWidthFactor));
        return string.Join(Environment.NewLine, pages);
    }

    private static string RenderPage(Page page, double charWidthFactor)
    {
        var words = page.GetWords().Where(w => !string.IsNullOrWhiteSpace(w.Text)).ToList();
        if (words.Count == 0)
            return string.Empty;

        var lines = GroupIntoLines(words);
        var averageFontSize = AverageFontSize(words);
        var characterWidth = charWidthFactor * averageFontSize;

        // Normal line height comes from the page's font size, not from statistics of the observed
        // line gaps: with only a couple of lines the one large gap we want to flag would itself drag
        // up a gap-based average, masking the very break it should detect.
        var normalLineHeight = averageFontSize * SingleSpacingFactor;

        var text = new StringBuilder();
        double? previousY = null;

        foreach (var line in lines)
        {
            if (previousY is double prevY && prevY - line.Y > normalLineHeight * BlankLineGapFactor)
                text.AppendLine();

            text.AppendLine(RenderLine(line.Words, characterWidth));
            previousY = line.Y;
        }

        return text.ToString().TrimEnd('\r', '\n');
    }

    // Words are clustered top-to-bottom by baseline proximity rather than by an exact Y match,
    // since two words on the same visual line rarely share the exact same bounding-box coordinate.
    private static List<(double Y, List<Word> Words)> GroupIntoLines(List<Word> words)
    {
        var lines = new List<(double Y, List<Word> Words)>();

        foreach (var word in words.OrderByDescending(w => w.BoundingBox.Bottom))
        {
            var tolerance = AverageFontSize([word]) * LineToleranceFactor;
            var index = lines.FindIndex(l => Math.Abs(l.Y - word.BoundingBox.Bottom) <= tolerance);

            if (index < 0)
                lines.Add((word.BoundingBox.Bottom, [word]));
            else
                lines[index].Words.Add(word);
        }

        foreach (var line in lines)
            line.Words.Sort((a, b) => a.BoundingBox.Left.CompareTo(b.BoundingBox.Left));

        return lines;
    }

    private static double AverageFontSize(IReadOnlyList<Word> words)
    {
        var fontSizes = words.SelectMany(w => w.Letters).Select(l => (double)l.FontSize).ToList();
        if (fontSizes.Count > 0)
            return fontSizes.Average();

        return words.Select(w => w.BoundingBox.Height).DefaultIfEmpty(1).Average();
    }

    private static string RenderLine(List<Word> words, double characterWidth)
    {
        if (characterWidth <= 0)
            return string.Join(" ", words.Select(w => w.Text));

        var line = new StringBuilder();

        foreach (var word in words)
        {
            var column = Math.Max((int)Math.Round(word.BoundingBox.Left / characterWidth), line.Length);
            line.Append(' ', column - line.Length);
            line.Append(word.Text);
        }

        return line.ToString();
    }
}
