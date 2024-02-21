using System;
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
        private string? inputFolder; // Stores the path of the selected input folder
        private string? outputPdfFile; // Stores the path of the selected output PDF file

        // Constructor
        public MainWindow()
        {
            // Set the application icon
            this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("./convert.ico", UriKind.RelativeOrAbsolute));
            InitializeComponent(); // Initialize the window components
        }

        // Event handler for selecting the input images folder
        private void SelectImagesFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog(); // Open file dialog
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*"; // Filter for image files
            if (dialog.ShowDialog() == true) // If user selects a file
            {
                inputFolder = Path.GetDirectoryName(dialog.FileName); // Get the directory of the selected file
                ImagesFolderPathTextBox.Text = inputFolder; // Display the selected folder path
            }
        }

        // Event handler for selecting the output PDF file
        private void SelectOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog(); // Save file dialog
            dialog.Filter = "PDF files (*.pdf)|*.pdf"; // Filter for PDF files
            if (dialog.ShowDialog() == true) // If user selects a file
            {
                outputPdfFile = dialog.FileName; // Get the selected file path
                OutputPdfPathTextBox.Text = outputPdfFile; // Display the selected file path
            }
        }

        // Event handler for converting images to PDF
        private void ConvertToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (inputFolder == null || outputPdfFile == null) // If input folder or output PDF file is not selected
            {
                MessageBox.Show("Please select input folder and output PDF file first."); // Display error message
                return;
            }

            // Get all PNG files in the selected input folder
            string[] imageFiles = Directory.GetFiles(inputFolder, "*.png");

            using (PdfDocument pdf = new PdfDocument()) // Create a new PDF document
            {
                foreach (string imageFile in imageFiles) // Iterate through each image file
                {
                    using (Bitmap bitmap = new Bitmap(imageFile)) // Open the image file
                    {
                        PdfPage page = pdf.AddPage(); // Add a new page to the PDF document
                        // Set page dimensions based on image dimensions and resolution
                        page.Width = bitmap.Width * 72 / bitmap.HorizontalResolution;
                        page.Height = bitmap.Height * 72 / bitmap.VerticalResolution;
                        XGraphics gfx = XGraphics.FromPdfPage(page); // Create graphics context for the page

                        XImage xImage = XImage.FromFile(imageFile); // Load the image file
                        // Draw the image on the PDF page
                        gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);
                    }
                }
                pdf.Save(outputPdfFile); // Save the PDF document
            }

            MessageBox.Show("PDF conversion done successfully !.", "Information", MessageBoxButton.OK, MessageBoxImage.Information); // Display success message
        }

        // Event handler for mouse entering button
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Background = System.Windows.Media.Brushes.Blue; // Change the background color when hovered over
        }

        // Event handler for mouse leaving button
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Background = System.Windows.Media.Brushes.LightBlue; // Change the background color back to the original color when mouse leaves
        }
    }
}
