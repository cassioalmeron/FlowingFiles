using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FlowingFiles.MVVM
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            var options = new List<OptionViewModel>
            {
                // Contabil
                new() { Description = "Folha de Pagamento - Contra Cheque", Path = @"Contabil\Folha de Pagamento\Contra Cheque", Required = true },
                new() { Description = "PIX - Divisao de Lucros", Path = @"Contabil\Folha de Pagamento\PIX - Divisao de Lucros", Required = true },
                new() { Description = "PIX - Folha de Pagamento", Path = @"Contabil\Folha de Pagamento\PIX - Folha de Pagamento", Required = true },
                new() { Description = "DARF_INSS - Boleto", Path = @"Contabil\Pagamentos\DARF_INSS - Boleto", Required = true },
                new() { Description = "DARF_INSS - Comprovante", Path = @"Contabil\Pagamentos\DARF_INSS - Comprovante", Required = true },
                new() { Description = "E-Cont - Boleto", Path = @"Contabil\Pagamentos\ECont - Boleto", Required = true },
                new() { Description = "E-Cont: Comprovante", Path = @"Contabil\Pagamentos\ECont - Comprovante", Required = true },
                new() { Description = "New Office: Boleto", Path = @"Contabil\Pagamentos\NewOffice - Boleto", Required = true },
                new() { Description = "New Office: Comprovante", Path = @"Contabil\Pagamentos\NewOffice - Comprovante", Required = true },
                new() { Description = "CSLL - Boleto", Path = @"Contabil\Pagamentos\CSLL - Boleto" },
                new() { Description = "CSLL - Comprovante", Path = @"Contabil\Pagamentos\CSLL - Comprovante" },
                new() { Description = "IRPJ - Boleto", Path = @"Contabil\Pagamentos\IRPJ - Boleto" },
                new() { Description = "IRPJ - Comprovante", Path = @"Contabil\Pagamentos\IRPJ - Comprovante" },

                //new() { Description = "Alvará Funcionamento - Boleto", Path = @"Contabil\Pagamentos\Alvará Funcionamento - Boleto", Required = true },
                //new() { Description = "Alvará Funcionamento - Comprovante", Path = @"Contabil\Pagamentos\Alvará Funcionamento - Comprovante", Required = true },

                // Fiscal
                new()  { Description = "NFSE - XML", Path = @"Fiscal\Notas Fiscais Emitidas\NFSE", Required = true },
                new()  { Description = "NFSE - PDF", Path = @"Fiscal\Notas Fiscais Emitidas\NFSE", Required = true },
                new()  { Description = "E-Cont - XML", Path = @"Fiscal\Notas Fiscais Serviços Tomados\ECont", Required = true },
                new()  { Description = "E-Cont - PDF", Path = @"Fiscal\Notas Fiscais Serviços Tomados\ECont", Required = true },
                new()  { Description = "New Office - XML", Path = @"Fiscal\Notas Fiscais Serviços Tomados\NewOffice", Required = true },
                new()  { Description = "New Office - PDF", Path = @"Fiscal\Notas Fiscais Serviços Tomados\NewOffice", Required = true },

                // Root
                new()  { Description = "Extrato", Path = @"Extrato", Required = true },
                new()  { Description = "Movimentações (OFX)", Path = @"Movimentacoes", Required = true },
                new()  { Description = "Relatório", Path = @"Relatorio", Required = true }
            };

            ExportFolderCommand = new RelayCommand(ExportFolder);
            ExportZipFileCommand = new RelayCommand(ExportZipFile);

            foreach (var option in options)
                Files.Add(new()
                {
                    Option = option,
                });
        }

        private OptionViewModel? _currentFileOption;
        public OptionViewModel? CurrentFileOption 
        {
            get { return _currentFileOption; }
            set { SetProperty(ref _currentFileOption, value, () => CurrentFileOption); }
        }

        public ObservableCollection<FileViewModel> Files { get; private set; } = new ObservableCollection<FileViewModel>();

        private FileViewModel? _currentFile = null;
        public FileViewModel? CurrentFile
        {
            get { return _currentFile; }
            set { SetProperty(ref _currentFile, value, () => CurrentFile); }
        }

        private int _selectedMonth = DateTime.Now.Month;
        public int SelectedMonth {
            get { return _selectedMonth; }
            set { SetProperty(ref _selectedMonth, value, () => SelectedMonth); }
        }

        public ICommand ExportFolderCommand { get; }

        private void ExportFolder()
        {
            try
            {
                GenerateFolder();

                Process.Start("explorer.exe", DestinationFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Flowing Files", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private static string _destinationFolder = @"C:\Temp\Flowing\";

        private string DestinationFolder
        {
            get
            {
                return $@"{_destinationFolder}{SelectedMonthAsExtension()}";
            }
        }

        public ICommand ExportZipFileCommand { get; }

        private void ExportZipFile()
        {
            try
            {
                GenerateFolder();

                var zipFilePath = $@"C:\Temp\Flowing\{SelectedMonthAsExtension()}.zip";
                if (File.Exists(zipFilePath))
                    File.Delete(zipFilePath);

                System.IO.Compression.ZipFile.CreateFromDirectory(DestinationFolder, zipFilePath);
                
                Process.Start("explorer.exe", zipFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Flowing Files", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateFolder()
        {
            var files = Files.Where(x => !string.IsNullOrEmpty(x.FileName)).ToList();
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);
                var destinationFile = $@"{DestinationFolder}\{file.Option.Path}{extension}";
                var folder = Path.GetDirectoryName(destinationFile);

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                File.Copy(file.FileName, destinationFile, true);
            }
        }

        private static string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        private string SelectedMonthAsExtension()
        {
            return months[SelectedMonth - 1].Substring(0, 3);
        }
    }
}