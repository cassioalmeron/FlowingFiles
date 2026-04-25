// Console project retained for future prototyping. Classification logic moved to FlowingFiles.Core.

using FlowingFiles.Core.Services;

//var result = await FileClassifierService.Classify(@"C:\Temp\Flowing\Mar\Relatorio.pdf");
//Console.WriteLine(result);

//var str = PdfTextService.ExtractText(@"C:\Temp\Flowing\Mar\xxx\DARF_INSS - Boleto.pdf");
//var str = PdfTextService.ExtractText(@"C:\Temp\Flowing\CSLL_1º_TRIMESTRE_2026_CASSIO_ALMERON.pdf");
//var str = await PdfTextService.ExtractTextAsync(@"C:\Users\cassi\Downloads\Boleto_46539874000112_20260505.pdf");
var str = await PdfTextService.ExtractTextAsync(@"C:\Temp\Flowing\Mar\xxx\ECont-Boleto.pdf");
Console.WriteLine(str);

//var ocr = new OcrService("http://localhost:11434", "glm-ocr:latest");
//var result = await ocr.ExtractTextAsync(@"C:\Temp\Flowing\Mar\Contabil\Folha de Pagamento\PIX - Folha de Pagamento.jpeg");
//var result = await ocr.ExtractTextAsync(@"C:\Temp\Flowing\Mar\xxx\PIX - Divisao de Lucros.jpeg");
//var str = await ocr.ExtractTextAsync(@"C:\Temp\Flowing\WhatsApp Image 2026-04-25 at 2.33.59 PM.jpeg");
//var str = await ocr.ExtractTextAsync(@"C:\Temp\Flowing\WhatsApp Image 2026-04-25 at 2.33.31 PM.jpeg");
//Console.WriteLine(str);

//var result = await FileClassifierService.Classify(@"C:\Temp\Flowing\Mar\xxx\ExtratoXP.pdf");
//var result = await FileClassifierService.Classify(@"C:\Temp\Flowing\Mar\xxx\DARF_INSS - Boleto.pdf");
//var result = await FileClassifierService.Classify(@"C:\Temp\Flowing\CSLL_1º_TRIMESTRE_2026_CASSIO_ALMERON.pdf");
//var result = await FileClassifierService.Classify(@"C:\Temp\Flowing\WhatsApp Image 2026-04-25 at 2.33.59 PM.jpeg");
//var result = await FileClassifierService.Classify(@"C:\Users\cassi\Downloads\Boleto_46539874000112_20260505.pdf");
var result = await FileClassifierService.Classify(@"C:\Temp\Flowing\Mar\xxx\ECont-Boleto.pdf");
Console.WriteLine(result);

return;
