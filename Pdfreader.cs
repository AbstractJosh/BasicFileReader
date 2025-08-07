// Required NuGet Packages: // - PdfSharp // - PdfiumViewer

using System; using System.IO; using System.Windows; using System.Windows.Forms.Integration; using PdfiumViewer; using PdfSharp.Pdf; using PdfSharp.Drawing; using Microsoft.Win32;

namespace WpfPdfEditor { public partial class MainWindow : Window { private PdfViewer _pdfViewer; private string _currentFilePath;

public MainWindow()
    {
        InitializeComponent();

        // Setup PdfiumViewer in WindowsFormsHost
        _pdfViewer = new PdfViewer();
        WindowsFormsHost host = new WindowsFormsHost();
        host.Child = _pdfViewer;
        PdfContainer.Children.Add(host); // PdfContainer is a Grid or Panel in XAML
    }

    private void LoadPdf_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "PDF Files (*.pdf)|*.pdf";
        if (ofd.ShowDialog() == true)
        {
            _currentFilePath = ofd.FileName;
            _pdfViewer.Document?.Dispose();
            _pdfViewer.Document = PdfiumViewer.PdfDocument.Load(_currentFilePath);
        }
    }

    private void AddText_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_currentFilePath)) return;

        string newPath = Path.Combine(Path.GetDirectoryName(_currentFilePath), "edited.pdf");
        var document = PdfSharp.Pdf.IO.PdfReader.Open(_currentFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify);
        var gfx = XGraphics.FromPdfPage(document.Pages[0]);
        gfx.DrawString("Edited by WPF App", new XFont("Arial", 14), XBrushes.Black, new XRect(100, 100, 300, 50), XStringFormats.TopLeft);
        document.Save(newPath);
        MessageBox.Show($"PDF edited and saved as: {newPath}");
    }
}

}

