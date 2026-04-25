using System.Xml;

namespace FlowingFiles.Core.Services;

public class FileClassifierService(OcrService ocrService)
{
    public async Task<string[]> ClassifyAllAsync(IEnumerable<string> filePaths)
    {
        var tasks = filePaths.Select(ClassifyAsync);
        return await Task.WhenAll(tasks);
    }

    public async Task<string> ClassifyAsync(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();

        try
        {
            return extension switch
            {
                ".pdf" => await ClassifyPdfAsync(filePath),
                ".jpg" or ".jpeg" or ".png" => await ClassifyImageAsync(filePath),
                ".xml" => ClassifyXml(filePath),
                _ => "Unknown"
            };
        }
        catch
        {
            return "Unknown";
        }
    }

    private async Task<string> ClassifyPdfAsync(string filePath)
    {
        var text = await PdfTextService.ExtractTextAsync(filePath);

        if (text.Contains("Sacador/Avalista: E-CONT CONTABILIDADE LTDA (11.462.634/0001-82)"))
            return "E-Cont - Boleto";
        if (text.Contains("E-CONT CONTABILIDADE LTDA"))
            return "E-Cont - PDF";
        if (text.StartsWith("CASSIO ALMERON SOFTWARE ENGINEERING LTDA"))
            return "Relatorio";
        if (text.Contains("CASSIO ALMERON SOFTWARE ENGINEERING") && (text.Contains("Instituição: Banco Inter") || text.Contains("Instituicao: Banco Inter")))
            return "Extrato Inter";
        if (text.Contains("CASSIO ALMERON SOFTWARE ENGINEERING LTDA") && text.Contains("Conta Digital XP"))
            return "Extrato - XP";
        if (text.Contains("Demonstrativo de Pagamento"))
            return "Folha de Pagamento - Contra Cheque";
        if (text.Contains("CSLL - LUCRO PRESUMIDO OU ARBITRADO"))
            return "CSLL - Boleto";
        if (text.Contains("IRPJ - LUCRO PRESUMIDO"))
            return "IRPJ - Boleto";
        if (text.Contains("Documento de Arrecadacao de Receitas Federais") || text.Contains("Documento de Arrecadação de Receitas Federais"))
            return "DARF_INSS - Boleto";
        if (text.Contains("qrcode.sicredi.com.br"))
            return "New Office - Boleto";
        if (text.Contains("emitida por CASSIO ALMERON SOFTWARE ENGINEERING LTDA"))
            return "NFSE - PDF";
        if (text.Contains("emitida por E-CONT CONTABILIDADE LTDA"))
            return "E-Cont - PDF";
        if (text.Contains("emitida por NEW OFFICE CENTRO APOIO ADMINISTRATIVO ESCRITORIO LTDA"))
            return "New Office - PDF";
        if (text.Contains("ISS E TAXAS: TAXA DE FISCALIZAÇÃO"))
            return "Alvará - Boleto";

        return "Unknown";
    }

    private async Task<string> ClassifyImageAsync(string filePath)
    {
        var text = await ocrService.ExtractTextAsync(filePath);

        if (text.Contains("R$ 400,00"))
            return "E-Cont - Comprovante";
        if (text.Contains("R$ 100,00"))
            return "New Office - Comprovante";
        if (text.Contains("Folha de pagamento"))
            return "PIX - Folha de Pagamento";
        if (text.Contains("Divisao de lucros") || text.Contains("Divisão de lucros"))
            return "PIX - Divisao de Lucros";
        if (text.Contains("PREFEITURA MUNICIPAL DE SAO JOSE"))
            return "Alvará - Comprovante";
        if (text.Contains("3336662"))
            return "IRPJ - Comprovante";
        if (text.Contains("810100"))
            return "CSLL - Comprovante";
        if (text.Contains("Nome Receita Federal"))
            return "DARF_INSS - Comprovante";

        return "Unknown";
    }

    private static string ClassifyXml(string filePath)
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

        return "Unknown";
    }

    public static async Task<string> Classify(string filePath)
    {
        var service = new FileClassifierService(new OcrService());
        return await service.ClassifyAsync(filePath);
    }
}
