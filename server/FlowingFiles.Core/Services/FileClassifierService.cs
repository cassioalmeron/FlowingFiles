using FlowingFiles.Core.Dtos;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace FlowingFiles.Core.Services;

public class FileClassifierService(
    OcrService ocrService,
    SimilarityClassifierService similarityClassifierService,
    ILogger<FileClassifierService> logger)
{
    public async Task<FileClassification[]> ClassifyAllAsync(IEnumerable<string> filePaths)
    {
        var tasks = filePaths.Select(ClassifyAsync);
        return await Task.WhenAll(tasks);
    }

    public async Task<FileClassification> ClassifyAsync(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();

        try
        {
            return extension switch
            {
                ".pdf" => await ClassifyPdfAsync(filePath),
                ".jpg" or ".jpeg" or ".png" => await ClassifyImageAsync(filePath),
                ".xml" => new FileClassification(ClassifyXml(filePath), ClassificationMethod.Rule, null),
                _ => UnsupportedExtension(filePath, extension)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Classification failed for {FileName} ({Extension})", Path.GetFileName(filePath), extension);
            return new FileClassification("Unknown", ClassificationMethod.Rule, null);
        }
    }

    private async Task<FileClassification> ClassifyPdfAsync(string filePath)
    {
        var text = await PdfTextService.ExtractTextAsync(filePath);

        var result = await similarityClassifierService.ClassifyAsync(text);
        LogSimilarityResult(filePath, result);
        return new FileClassification(result.Label, ClassificationMethod.Similarity, result.Neighbours);
    }

    private async Task<FileClassification> ClassifyImageAsync(string filePath)
    {
        var text = await ocrService.ExtractTextAsync(filePath);

        var result = await similarityClassifierService.ClassifyAsync(text);
        LogSimilarityResult(filePath, result);
        return new FileClassification(result.Label, ClassificationMethod.Similarity, result.Neighbours);
    }

    private void LogSimilarityResult(string filePath, ClassificationResult result)
    {
        var neighbours = result.Neighbours.Count == 0
            ? "(no samples in corpus)"
            : string.Join(", ", result.Neighbours.Select(n => $"{n.Label}={n.Similarity:F3}"));

        logger.LogInformation(
            "Similarity classification for {FileName}: label={Label} confidence={Confidence:F3} neighbours=[{Neighbours}]",
            Path.GetFileName(filePath), result.Label, result.Confidence, neighbours);
    }

    private FileClassification UnsupportedExtension(string filePath, string extension)
    {
        logger.LogWarning("Unsupported file extension: {FileName} ({Extension})", Path.GetFileName(filePath), extension);
        return new FileClassification("Unknown", ClassificationMethod.Rule, null);
    }

    private string ClassifyXml(string filePath)
    {
        var xml = new XmlDocument();
        xml.Load(filePath);
        var cnpj = xml["nfse"]?["prestador"]?["cpfcnpj"]?.InnerText;

        if (cnpj == "11462634000182")
            return "E-Cont - XML";
        if (cnpj == "28380119000156")
            return "New Office - XML";
        if (cnpj == "46539874000112")
            return "NFSE - XML";

        logger.LogWarning("XML not classified: {FileName}. CNPJ: {Cnpj}", Path.GetFileName(filePath), cnpj ?? "null");
        return "Unknown";
    }
}
