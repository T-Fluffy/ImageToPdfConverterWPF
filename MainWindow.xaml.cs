using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ImageToPdfConverterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? inputFolder;
        private string? outputPdfFile;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectImagesFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
            if (dialog.ShowDialog() == true)
            {
                inputFolder = Path.GetDirectoryName(dialog.FileName);
                ImagesFolderPathTextBox.Text = inputFolder;
            }

        }

        private void SelectOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "PDF files (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                outputPdfFile = dialog.FileName;
                OutputPdfPathTextBox.Text = outputPdfFile;
            }
        }
        private void ConvertToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (inputFolder == null || outputPdfFile == null)
            {
                MessageBox.Show("Please select input folder and output PDF file first.");
                return;
            }

            // ConvertToPdf logic
            string[] imageFiles = Directory.GetFiles(inputFolder, "*.png");

            using (PdfDocument pdf = new PdfDocument())
            {
                foreach (string imageFile in imageFiles)
                {
                    using (Bitmap bitmap = new Bitmap(imageFile))
                    {
                        PdfPage page = pdf.AddPage();
                        page.Width = bitmap.Width * 72 / bitmap.HorizontalResolution;
                        page.Height = bitmap.Height * 72 / bitmap.VerticalResolution;
                        XGraphics gfx = XGraphics.FromPdfPage(page);

                        XImage xImage = XImage.FromFile(imageFile);
                        gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);
                    }
                }
                pdf.Save(outputPdfFile);
            }

            // Implement logic for converting images to PDF using inputFolder and outputPdfFile
            MessageBox.Show("PDF conversion done successfully !.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Background = System.Windows.Media.Brushes.Blue; // Change the background color when hovered over
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Background = System.Windows.Media.Brushes.LightBlue; // Change the background color back to the original color when mouse leaves
        }


    }
}
