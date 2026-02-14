import type { DocumentOption } from '../types';

export const documentOptions: DocumentOption[] = [
  // Contabil - Folha de Pagamento
  { description: 'Folha de Pagamento - Contra Cheque', path: 'Contabil/Folha de Pagamento/Contra Cheque', required: true },
  { description: 'PIX - Divisao de Lucros', path: 'Contabil/Folha de Pagamento/PIX - Divisao de Lucros', required: true },
  { description: 'PIX - Folha de Pagamento', path: 'Contabil/Folha de Pagamento/PIX - Folha de Pagamento', required: true },

  // Contabil - Pagamentos (required)
  { description: 'DARF_INSS - Boleto', path: 'Contabil/Pagamentos/DARF_INSS - Boleto', required: true },
  { description: 'DARF_INSS - Comprovante', path: 'Contabil/Pagamentos/DARF_INSS - Comprovante', required: true },
  { description: 'E-Cont - Boleto', path: 'Contabil/Pagamentos/ECont - Boleto', required: true },
  { description: 'E-Cont - Comprovante', path: 'Contabil/Pagamentos/ECont - Comprovante', required: true },
  { description: 'New Office - Boleto', path: 'Contabil/Pagamentos/NewOffice - Boleto', required: true },
  { description: 'New Office - Comprovante', path: 'Contabil/Pagamentos/NewOffice - Comprovante', required: true },

  // Contabil - Pagamentos (optional)
  { description: 'CSLL - Boleto', path: 'Contabil/Pagamentos/CSLL - Boleto', required: false },
  { description: 'CSLL - Comprovante', path: 'Contabil/Pagamentos/CSLL - Comprovante', required: false },
  { description: 'IRPJ - Boleto', path: 'Contabil/Pagamentos/IRPJ - Boleto', required: false },
  { description: 'IRPJ - Comprovante', path: 'Contabil/Pagamentos/IRPJ - Comprovante', required: false },

  // Fiscal - Notas Fiscais Emitidas
  { description: 'NFSE - XML', path: 'Fiscal/Notas Fiscais Emitidas/NFSE', required: true },
  { description: 'NFSE - PDF', path: 'Fiscal/Notas Fiscais Emitidas/NFSE', required: true },

  // Fiscal - Notas Fiscais Serviços Tomados
  { description: 'E-Cont - XML', path: 'Fiscal/Notas Fiscais Servicos Tomados/ECont', required: true },
  { description: 'E-Cont - PDF', path: 'Fiscal/Notas Fiscais Servicos Tomados/ECont', required: true },
  { description: 'New Office - XML', path: 'Fiscal/Notas Fiscais Servicos Tomados/NewOffice', required: true },
  { description: 'New Office - PDF', path: 'Fiscal/Notas Fiscais Servicos Tomados/NewOffice', required: true },

  // Root
  { description: 'Extrato', path: 'Extrato', required: true },
  { description: 'Movimentacoes (OFX)', path: 'Movimentacoes', required: true },
  { description: 'Relatorio', path: 'Relatorio', required: true },
];
