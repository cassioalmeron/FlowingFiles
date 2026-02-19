using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowingFiles.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedDocumentOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DocumentOption",
                columns: ["Description", "Path", "Required", "Position"],
                values: new object[,]
                {
                    // Contabil - Folha de Pagamento
                    { "Folha de Pagamento - Contra Cheque", "Contabil/Folha de Pagamento/Contra Cheque", true, 1 },
                    { "PIX - Divisao de Lucros",            "Contabil/Folha de Pagamento/PIX - Divisao de Lucros", true, 2 },
                    { "PIX - Folha de Pagamento",           "Contabil/Folha de Pagamento/PIX - Folha de Pagamento", true, 3 },

                    // Contabil - Pagamentos (required)
                    { "DARF_INSS - Boleto",       "Contabil/Pagamentos/DARF_INSS - Boleto", true, 4 },
                    { "DARF_INSS - Comprovante",  "Contabil/Pagamentos/DARF_INSS - Comprovante", true, 5 },
                    { "E-Cont - Boleto",          "Contabil/Pagamentos/ECont - Boleto", true, 6 },
                    { "E-Cont - Comprovante",     "Contabil/Pagamentos/ECont - Comprovante", true, 7 },
                    { "New Office - Boleto",      "Contabil/Pagamentos/NewOffice - Boleto", true, 8 },
                    { "New Office - Comprovante", "Contabil/Pagamentos/NewOffice - Comprovante", true, 9 },

                    // Contabil - Pagamentos (optional)
                    { "CSLL - Boleto",      "Contabil/Pagamentos/CSLL - Boleto", false, 10 },
                    { "CSLL - Comprovante", "Contabil/Pagamentos/CSLL - Comprovante", false, 11 },
                    { "IRPJ - Boleto",      "Contabil/Pagamentos/IRPJ - Boleto", false, 12 },
                    { "IRPJ - Comprovante", "Contabil/Pagamentos/IRPJ - Comprovante", false, 13 },

                    // Fiscal - Notas Fiscais Emitidas
                    { "NFSE - XML", "Fiscal/Notas Fiscais Emitidas/NFSE", true, 14 },
                    { "NFSE - PDF", "Fiscal/Notas Fiscais Emitidas/NFSE", true, 15 },

                    // Fiscal - Notas Fiscais Serviços Tomados
                    { "E-Cont - XML",     "Fiscal/Notas Fiscais Servicos Tomados/ECont", true, 16 },
                    { "E-Cont - PDF",     "Fiscal/Notas Fiscais Servicos Tomados/ECont", true, 17 },
                    { "New Office - XML", "Fiscal/Notas Fiscais Servicos Tomados/NewOffice", true, 18 },
                    { "New Office - PDF", "Fiscal/Notas Fiscais Servicos Tomados/NewOffice", true, 19 },

                    // Root
                    { "Extrato",             "Extrato",        true, 20 },
                    { "Movimentacoes (OFX)", "Movimentacoes",  true, 21 },
                    { "Relatorio",           "Relatorio",      true, 22 },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentOption",
                keyColumn: "Description",
                keyValues:
                [
                    "Folha de Pagamento - Contra Cheque",
                    "PIX - Divisao de Lucros",
                    "PIX - Folha de Pagamento",
                    "DARF_INSS - Boleto",
                    "DARF_INSS - Comprovante",
                    "E-Cont - Boleto",
                    "E-Cont - Comprovante",
                    "New Office - Boleto",
                    "New Office - Comprovante",
                    "CSLL - Boleto",
                    "CSLL - Comprovante",
                    "IRPJ - Boleto",
                    "IRPJ - Comprovante",
                    "NFSE - XML",
                    "NFSE - PDF",
                    "E-Cont - XML",
                    "E-Cont - PDF",
                    "New Office - XML",
                    "New Office - PDF",
                    "Extrato",
                    "Movimentacoes (OFX)",
                    "Relatorio",
                ]);
        }
    }
}
