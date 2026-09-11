using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FlowingFiles.Core.Services;

public class OcrService(ILogger<OcrService> logger)
{
    public async Task<string> ExtractTextAsync(string filePath)
    {
        var outputBase = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var outputFile = $"{outputBase}.txt";

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "tesseract",
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            psi.ArgumentList.Add(filePath);
            psi.ArgumentList.Add(outputBase);
            psi.ArgumentList.Add("-l");
            psi.ArgumentList.Add("eng");

            using var process = Process.Start(psi)!;
            await process.WaitForExitAsync();

            if (!File.Exists(outputFile))
            {
                var stderr = await process.StandardError.ReadToEndAsync();
                logger.LogError("Tesseract exited {ExitCode} for {FileName}. Stderr: {Stderr}",
                    process.ExitCode, Path.GetFileName(filePath), stderr);
                return string.Empty;
            }

            // Postgres `text` columns reject embedded NUL characters outright; strip any that OCR
            // produces from noisy input (same reasoning as PdfTextService.Sanitize).
            var text = await File.ReadAllTextAsync(outputFile);
            return text.Replace("\0", string.Empty);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "OCR failed for {FileName}", Path.GetFileName(filePath));
            throw;
        }
        finally
        {
            if (File.Exists(outputFile))
                File.Delete(outputFile);
        }
    }
}
