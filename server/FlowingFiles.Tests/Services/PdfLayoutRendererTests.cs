using FlowingFiles.Core.Services;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;

namespace FlowingFiles.Tests.Services;

[TestClass]
public class PdfLayoutRendererTests
{
    // Two table rows sharing the same three X positions (20 / 120 / 220pt), 20pt apart vertically,
    // plus an isolated line far below (100pt gap) to exercise the blank-line-on-large-gap rule.
    private static PdfDocument BuildFixture()
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddStandard14Font(Standard14Font.Helvetica);
        var page = builder.AddPage(300, 200);

        page.AddText("Col1", 12, new PdfPoint(20, 160), font);
        page.AddText("Col2", 12, new PdfPoint(120, 160), font);
        page.AddText("Col3", 12, new PdfPoint(220, 160), font);

        page.AddText("RowB1", 12, new PdfPoint(20, 140), font);
        page.AddText("RowB2", 12, new PdfPoint(120, 140), font);
        page.AddText("RowB3", 12, new PdfPoint(220, 140), font);

        page.AddText("Footer", 12, new PdfPoint(20, 40), font);

        return PdfDocument.Open(builder.Build());
    }

    [TestMethod]
    public void Render_WordsOnSameLine_KeepLeftToRightOrder()
    {
        using var document = BuildFixture();

        var text = PdfLayoutRenderer.Render(document);
        var lines = text.Split(Environment.NewLine);
        var headerLine = lines.Single(l => l.Contains("Col1"));

        Assert.IsTrue(headerLine.IndexOf("Col1", StringComparison.Ordinal) <
                      headerLine.IndexOf("Col2", StringComparison.Ordinal));
        Assert.IsTrue(headerLine.IndexOf("Col2", StringComparison.Ordinal) <
                      headerLine.IndexOf("Col3", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Render_WordsAtSameX_AlignToTheSameColumnAcrossLines()
    {
        using var document = BuildFixture();

        var text = PdfLayoutRenderer.Render(document);
        var lines = text.Split(Environment.NewLine);
        var headerLine = lines.Single(l => l.Contains("Col1"));
        var secondRowLine = lines.Single(l => l.Contains("RowB1"));

        Assert.AreEqual(
            headerLine.IndexOf("Col2", StringComparison.Ordinal),
            secondRowLine.IndexOf("RowB2", StringComparison.Ordinal));
        Assert.AreEqual(
            headerLine.IndexOf("Col3", StringComparison.Ordinal),
            secondRowLine.IndexOf("RowB3", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Render_LargeVerticalGap_EmitsBlankLineBeforeNextBlock()
    {
        using var document = BuildFixture();

        var text = PdfLayoutRenderer.Render(document);
        var lines = text.Split(Environment.NewLine);
        var secondRowIndex = Array.FindIndex(lines, l => l.Contains("RowB1"));
        var footerIndex = Array.FindIndex(lines, l => l.Contains("Footer"));

        Assert.AreEqual(secondRowIndex + 2, footerIndex, "A blank line should separate the table from the footer.");
        Assert.AreEqual(string.Empty, lines[secondRowIndex + 1]);
    }
}
