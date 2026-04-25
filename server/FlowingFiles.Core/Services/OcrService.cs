using Tesseract;

namespace FlowingFiles.Core.Services;

public class OcrService
{
    public Task<string> ExtractTextAsync(string filePath)
    {
        var tessdataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");

        using var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default);
        using var pix = Pix.LoadFromFile(filePath);
        using var page = engine.Process(pix);
        return Task.FromResult(page.GetText());
    }
}