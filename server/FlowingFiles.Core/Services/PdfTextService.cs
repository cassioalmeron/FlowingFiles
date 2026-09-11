using PDFtoImage;
using SkiaSharp;
using System.Diagnostics;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Core;

namespace FlowingFiles.Core.Services;

public enum PdfExtractionMode
{
    Raw,
    LayoutPreserving
}

public class PdfTextService
{
    public static Task<string> ExtractTextAsync(string filePath, PdfExtractionMode mode = PdfExtractionMode.Raw)
    {
        try
        {
            return Task.FromResult(Sanitize(ExtractText(filePath, mode)));
        }
        catch (PdfDocumentFormatException)
        {
            return Task.FromResult(Sanitize(ExtractViaOcr(filePath)));
        }
    }

    // Certain PDF font encodings make PdfPig yield embedded NUL characters; Postgres `text` columns
    // reject them outright ("invalid byte sequence for encoding UTF8: 0x00"), so strip them at the
    // one point every extraction path converges rather than at every caller.
    private static string Sanitize(string text) => text.Replace("\0", string.Empty);

    private static string ExtractText(string filePath, PdfExtractionMode mode)
    {
        using var document = PdfDocument.Open(filePath, new ParsingOptions { UseLenientParsing = true });

        if (mode == PdfExtractionMode.LayoutPreserving)
            return PdfLayoutRenderer.Render(document);

        return string.Join(Environment.NewLine,
            document.GetPages().Select(page =>
                string.Join(" ", page.GetWords().Select(w => w.Text))));
    }

    private static string ExtractViaOcr(string filePath)
    {
        var pages = new List<string>();

        using var pdfStream = File.OpenRead(filePath);
        foreach (var image in Conversion.ToImages(pdfStream, options: new RenderOptions(Dpi: 200)))
        using (image)
        {
            var tempImagePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");
            var outputBase = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var outputFile = $"{outputBase}.txt";

            try
            {
                using var skImage = SKImage.FromBitmap(image);
                using var jpgData = skImage.Encode(SKEncodedImageFormat.Jpeg, 95);
                File.WriteAllBytes(tempImagePath, jpgData.ToArray());

                var psi = new ProcessStartInfo
                {
                    FileName = "tesseract",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                psi.ArgumentList.Add(tempImagePath);
                psi.ArgumentList.Add(outputBase);
                psi.ArgumentList.Add("-l");
                psi.ArgumentList.Add("eng");

                using var process = Process.Start(psi)!;
                process.WaitForExit();

                if (File.Exists(outputFile))
                    pages.Add(File.ReadAllText(outputFile));
            }
            finally
            {
                if (File.Exists(tempImagePath)) File.Delete(tempImagePath);
                if (File.Exists(outputFile)) File.Delete(outputFile);
            }
        }

        return string.Join(Environment.NewLine, pages);
    }
}
