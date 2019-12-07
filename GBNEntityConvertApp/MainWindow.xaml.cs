using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GNBSophieEntityConverter;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

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
            gridSelectedItemInfo.IsEnabled = false;

            comboboxEncodings.Items.Clear();
            foreach (var en in Encoding.GetEncodings())
            {
                comboboxEncodings.Items.Add(en.Name.ToLower());
            }
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
                    var arg = new ConvertArgs(name);
                    if( !listboxSelectedFile.Items.Contains(arg))
                        listboxSelectedFile.Items.Add(arg);
                }
            }

            if (!listboxSelectedFile.Items.IsEmpty)
            {
                gridSelectedItemInfo.IsEnabled = true;
                listboxSelectedFile.SelectedItem = listboxSelectedFile.Items[0];

            }


        }

        private void BtnSelectOutPath_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbSaveTo.Text = fbd.SelectedPath;
                }
            }

        }

        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            List<ConvertArgs> doneArgs = new List<ConvertArgs>();
            StringBuilder builder = new StringBuilder("Progress:");
            StringBuilder errorBuilder = new StringBuilder("Errors");
            int countFail = 0;
            int countGood = 0;

            foreach (ConvertArgs arg in listboxSelectedFile.Items)
            {
                Converter converter = new Converter();
                try
                {
                    builder.AppendLine("");
                    builder.Append("Converting [");
                    builder.Append(System.IO.Path.GetFileName(arg.Input));
                    builder.AppendLine("] To xml file...");
                    converter.Convert(arg.Input, arg.SaveTo, arg.OutputFileName, arg.Encoding);
                    countGood++;
                    builder.AppendLine("Done.");
                    doneArgs.Add(arg);

                }
                catch (Exception ex)
                {
                    countFail++;
                    builder.AppendLine("Aborted");
                    errorBuilder.AppendLine("");
                    errorBuilder.AppendLine("Error with file:\t" + System.IO.Path.GetFileName(arg.Input));
                    errorBuilder.AppendLine("\t\tError MSG:" + Environment.NewLine + "\t\t"+ex.Message);

                }
                finally
                {
                    labelProgress.Text = builder.ToString();
                    labelErrors.Text = errorBuilder.ToString();
                }
            }
           
            foreach(var done in doneArgs)
            {
                if(listboxSelectedFile.Items!= null)
                {
                    listboxSelectedFile.Items.Remove(done);
                    if(listboxSelectedFile.Items.IsEmpty)
                    {
                        gridSelectedItemInfo.IsEnabled = false;
                    }
                }
            }

            string amountConvertedMsg = "Done: " + countGood + "/" + (countGood + countFail) + " converted";
            System.Windows.MessageBox.Show(amountConvertedMsg, "App");


        }

        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            listboxSelectedFile.Items.Remove(listboxSelectedFile.SelectedItem);
            if (listboxSelectedFile.Items.IsEmpty)
            {
                gridSelectedItemInfo.IsEnabled = false;
            }
        }

        private void BtnApplyToAll_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            foreach(ConvertArgs arg in listboxSelectedFile.Items)
            {
                arg.Encoding = comboboxEncodings.SelectedItem.ToString();
                arg.SaveTo = tbSaveTo.Text;
                if (cbOutFileNameUseDefault.IsChecked == false)
                {
                    arg.OutputFileName = tbOutFileName.Text + "_" + i++;
                    arg.IsDefaultOutputFileName = false;
                }

                if (cbSaveToUseDefault.IsChecked == false)
                {
                    arg.SaveTo = tbSaveTo.Text;
                    arg.IsDefaultSaveTo = false;
                }

                if (cbEncodingUseDefault.IsChecked == false)
                {
                    arg.Encoding = comboboxEncodings.SelectedItem.ToString();
                    arg.IsDefaultEncoding = false;
                }
            }
        }

        private void ListBoxSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            labelSelectedItemName.Content = arg.ToString();
            comboboxEncodings.SelectedItem = arg.Encoding.ToLower();
            tbOutFileName.Text = arg.OutputFileName;
            tbSaveTo.Text = arg.SaveTo;

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
            comboboxEncodings.SelectedItem = arg.Encoding.ToLower(); ;
            tbOutFileName.Text = arg.OutputFileName;
            tbSaveTo.Text = arg.SaveTo;

            cbEncodingUseDefault.IsChecked = arg.IsDefaultEncoding;
            cbOutFileNameUseDefault.IsChecked = arg.IsDefaultOutputFileName;
            cbSaveToUseDefault.IsChecked = arg.IsDefaultSaveTo;
        }

        private void CheckboxOutFileNameUseDefault_Checked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.DefaultOutputFileName();
            tbOutFileName.IsEnabled = false;
            tbOutFileName.Text = arg.OutputFileName;
        }

        private void CheckboxOutFileNameUseDefault_Unchecked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.IsDefaultOutputFileName = false;
            tbOutFileName.IsEnabled = true;
        }

        private void CheckboxSaveToUseDefault_Unchecked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.IsDefaultSaveTo = false;
            tbSaveTo.IsEnabled = true;
            btnSelectOutPath.IsEnabled = true;

        }

        private void CheckboxSaveToUseDefault_Checked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.DefaultSaveTo();
            tbSaveTo.IsEnabled = false;
            btnSelectOutPath.IsEnabled = false;
            tbSaveTo.Text = arg.SaveTo;

        }

        private void CheckboxEncodingUseDefault_Unchecked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.IsDefaultEncoding = false;
            comboboxEncodings.IsEnabled = true;
        }

        private void CheckboxEncodingUseDefault_Checked(object sender, RoutedEventArgs e)
        {
            ConvertArgs arg = listboxSelectedFile.SelectedItem as ConvertArgs;
            arg.DefaultEncoding();
            comboboxEncodings.IsEnabled = false;
            comboboxEncodings.SelectedItem = arg.Encoding.ToLower();
        }

        private void TbSaveTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = listboxSelectedFile.SelectedItem as ConvertArgs;
            if (item == null) return;
            item.SaveTo = tbSaveTo.Text;
        }
       
        private void TbOutFileName_TextChanged(object sender, TextChangedEventArgs e)
        {

            var item = listboxSelectedFile.SelectedItem as ConvertArgs;
            if (item == null) return;
            item.OutputFileName = tbOutFileName.Text;
        }

        private void ComboboxEncodings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = listboxSelectedFile.SelectedItem as ConvertArgs;
            if (item == null) return;
            item.Encoding = comboboxEncodings.SelectedItem.ToString();
        }
    }

    public class ConvertArgs
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
            if (obj is ConvertArgs)
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
}
