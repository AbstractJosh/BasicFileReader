using System;
using System.Windows;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfiumViewer;
using System.Windows.Forms.Integration;

namespace PdfEditorApp
{
    public partial class MainWindow : Window
    {
        private PdfDocument pdfSharpDoc;
        private string currentFilePath;
        private PdfiumViewer.PdfViewer pdfiumViewer;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize PdfiumViewer inside WindowsFormsHost
            pdfiumViewer = new PdfiumViewer.PdfViewer
            {
                Dock = System.Windows.Forms.DockStyle.Fill
            };
            pdfHost.Child = pdfiumViewer;
        }

        private void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "PDF files (*.pdf)|*.pdf" };
            if (dialog.ShowDialog() == true)
            {
                currentFilePath = dialog.FileName;

                // Load for viewing
                pdfiumViewer.Document?.Dispose(); // clean up old one
                pdfiumViewer.Document = PdfiumViewer.PdfDocument.Load(currentFilePath);

                // Load for editing
                pdfSharpDoc = PdfReader.Open(currentFilePath, PdfDocumentOpenMode.Modify);
            }
        }

        private void AddText_Click(object sender, RoutedEventArgs e)
        {
            if (pdfSharpDoc == null)
            {
                MessageBox.Show("Please open a PDF file first.");
                return;
            }

            var page = pdfSharpDoc.Pages[0];
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 20, XFontStyle.Bold);

            gfx.DrawString("Hello from PdfSharp!", font, XBrushes.Red,
                new XRect(50, 50, page.Width, page.Height),
                XStringFormats.TopLeft);

            MessageBox.Show("Text added to page 1.");
        }

        private void SavePdf_Click(object sender, RoutedEventArgs e)
        {
            if (pdfSharpDoc == null)
            {
                MessageBox.Show("No PDF loaded.");
                return;
            }

            var dialog = new SaveFileDialog { Filter = "PDF files (*.pdf)|*.pdf" };
            if (dialog.ShowDialog() == true)
            {
                pdfSharpDoc.Save(dialog.FileName);
                MessageBox.Show("PDF saved successfully.");
            }
        }
    }
}
