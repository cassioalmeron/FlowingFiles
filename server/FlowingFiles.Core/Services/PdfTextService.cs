using PDFtoImage;
using SkiaSharp;
using System.Diagnostics;
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
