using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GNBSophieEntityConverter;
using Microsoft.Win32;
namespace GBNEntityConvertApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Text files|*.txt|Log Files|*.log|All Files|*.*";
            fileDialog.DefaultExt = ".txt";
            var dialogOk = fileDialog.ShowDialog();
            if (dialogOk == true)
            {
                StringBuilder allFiles = new StringBuilder(String.Empty);

                foreach (String name in fileDialog.FileNames)
                {
                    /* var converter = new Converter();

                     converter.Convert(name);
                     allFiles.Append(name.Replace(" ", String.Empty));
                     allFiles.Append(Environment.NewLine)
                     ;*/

                    var arg = new ConvertArgs(name);
                    if( !listboxSelectedFile.Items.Contains(arg))
                        listboxSelectedFile.Items.Add(arg);
                }
            }


        }

        private void BtnSelectOutPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Text files|*.txt|Log Files|*.log|All Files|*.*";
            fileDialog.DefaultExt = ".txt";
            var dialogOk = fileDialog.ShowDialog();
            if (dialogOk == true)
            {
                StringBuilder allFiles = new StringBuilder(String.Empty);

                foreach (String name in fileDialog.FileNames)
                {
                    /* var converter = new Converter();

                     converter.Convert(name);
                     allFiles.Append(name.Replace(" ", String.Empty));
                     allFiles.Append(Environment.NewLine);*/
                    listboxSelectedFile.Items.Add(name);
                }
            }


        }

        private void ListBoxSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            labelSelectedItemName.Content = arg.ToString();
            tbEncoding.Text = arg.Encoding;
            tbOutFileName.Text = arg.OutputFileName;
            tbSaveTo.Text = arg.SaveTo;

        }

        private class ConvertArgs
        {
            public string Input { get; }
            public string SaveTo { get; set; }
            public string OutputFileName { get; set; }
            public string Encoding { get; set; }
            public bool IsDefaultEncoding { get; set; }
            public bool IsDefaultSaveTo { get; set; }
            public bool IsDefaultOutputFileName { get; set; }

            public ConvertArgs(string input)
            {
                Input = input;
                ResetDefault();
            }

            public void DefaultSaveTo()
            {
                IsDefaultSaveTo = true;
                SaveTo = Input.Replace(System.IO.Path.GetFileName(Input), String.Empty);
            }

            public void DefaultOutputFileName()
            {
                IsDefaultOutputFileName = true;
                OutputFileName = System.IO.Path.GetFileNameWithoutExtension(Input);
            }

            public void DefaultEncoding()
            {
                IsDefaultEncoding = true;
                Encoding = "Windows-1257";
            }

            public void ResetDefault()
            {
                DefaultSaveTo();
                DefaultOutputFileName();
                DefaultEncoding();
            }

            public override string ToString()
            {
                return System.IO.Path.GetFileName(Input);
            }

            public override bool Equals(object obj)
            {
                if(obj is ConvertArgs)
                {
                    var arg = obj as ConvertArgs;
                    return arg.Input == this.Input && arg.OutputFileName == this.OutputFileName && arg.Encoding == this.Encoding && arg.SaveTo == this.SaveTo;
                }
                else
                {
                    return false;
                }
            }
        }

        private void ListboxSelectedFiels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            if (arg == null)
            {
                if(listboxSelectedFile.Items.Count > 0)
                    listboxSelectedFile.SelectedItem = listboxSelectedFile.Items[0];
                return;
            }
            labelSelectedItemName.Content = arg.ToString();
            tbEncoding.Text = arg.Encoding;
            tbOutFileName.Text = arg.OutputFileName;
            tbSaveTo.Text = arg.SaveTo;

            cbEncoding.IsChecked = arg.IsDefaultEncoding;
            cbOutFileNameUseDefault.IsChecked = arg.IsDefaultOutputFileName;
            cbSaveTo.IsChecked = arg.IsDefaultSaveTo;
        }

        private void CbOutFileNameUseDefault_Checked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.DefaultOutputFileName();
            tbOutFileName.IsEnabled = false;
            tbOutFileName.Text = arg.OutputFileName;
        }

        private void CbOutFileNameUseDefault_Unchecked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.IsDefaultOutputFileName = false;
            tbOutFileName.IsEnabled = true;
        }

        private void CbSaveTo_Unchecked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.IsDefaultSaveTo = false;
            tbSaveTo.IsEnabled = true;
        }

        private void CbSaveTo_Checked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.DefaultSaveTo();
            tbSaveTo.IsEnabled = false;
            tbSaveTo.Text = arg.SaveTo;
        }

        private void CbEncoding_Unchecked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.IsDefaultEncoding = false;
            tbEncoding.IsEnabled = true;
        }

        private void CbEncoding_Checked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.DefaultEncoding();
            tbEncoding.IsEnabled = false;
            tbEncoding.Text = arg.SaveTo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<ConvertArgs> doneArgs = new List<ConvertArgs>();

            foreach (ConvertArgs arg in listboxSelectedFile.Items)
            {
                StringBuilder builder = new StringBuilder(labelProgress.Text);
                Converter converter = new Converter();
                try
                {
                    builder.AppendLine("");
                    builder.Append("Converting [");
                    builder.Append(System.IO.Path.GetFileName(arg.Input));
                    builder.AppendLine("] To xml file...");
                    converter.Convert(arg.Input, arg.SaveTo, arg.OutputFileName, arg.Encoding);
                    builder.AppendLine("Done.");
                    doneArgs.Add(arg);
                }
                catch (Exception ex)
                {
                    builder.AppendLine("Error" + ex.Message);
                }
                finally
                {
                    labelProgress.Text = builder.ToString();
                }
            }

            //listboxSelectedFile.SelectionMode = ;
            foreach(var done in doneArgs)
            {
                if(listboxSelectedFile.SelectedItems.Count>0)
                {
                    listboxSelectedFile.SelectedItems.Remove(done);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listboxSelectedFile.Items.Remove(listboxSelectedFile.SelectedItem);
        }
    }
}
