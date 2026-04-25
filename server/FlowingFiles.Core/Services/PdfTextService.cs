using PDFtoImage;
using SkiaSharp;
using Tesseract;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Core;

namespace FlowingFiles.Core.Services;

public class PdfTextService
{
    public static Task<string> ExtractTextAsync(string filePath)
    {
        try
        {
            return Task.FromResult(ExtractText(filePath));
        }
        catch (PdfDocumentFormatException)
        {
            return Task.FromResult(ExtractViaOcr(filePath));
        }
    }

    private static string ExtractText(string filePath)
    {
        using var document = PdfDocument.Open(filePath, new ParsingOptions { UseLenientParsing = true });
        return string.Join(Environment.NewLine,
            document.GetPages().Select(page =>
                string.Join(" ", page.GetWords().Select(w => w.Text))));
    }

    private static string ExtractViaOcr(string filePath)
    {
        var pages = new List<string>();
        var tessdataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");

        using var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default);
        using var pdfStream = File.OpenRead(filePath);

        foreach (var image in Conversion.ToImages(pdfStream, options: new RenderOptions(Dpi: 200)))
        using (image)
        {
            using var skImage = SKImage.FromBitmap(image);
            using var jpgData = skImage.Encode(SKEncodedImageFormat.Jpeg, 95);
            using var pix = Pix.LoadFromMemory(jpgData.ToArray());
            using var page = engine.Process(pix);
            pages.Add(page.GetText());
        }

        return string.Join(Environment.NewLine, pages);
    }
}