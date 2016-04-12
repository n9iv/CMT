using System;
using System.Collections.Generic;
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
using Microsoft.Office.Interop.Word;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Threading;

namespace CMT.ClockConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCinstruction.xaml
    /// </summary>
    public partial class UCinstruction : UserControl
    {
        private Int16 _val;

        public UCinstruction(Int16 val)
        {
            InitializeComponent();
            _val = val;
        }

        /// <summary>
        /// This method takes a Word document full path and new XPS document full path and name
        /// and returns the new XpsDocument
        /// </summary>
        /// <param name="wordDocName"></param>
        /// <param name="xpsDocName"></param>
        /// <returns></returns>
        private XpsDocument ConvertWordDocToXPSDoc(string wordDocName, string xpsDocName)
        {
            // Create a WordApplication and add Document to it
            Microsoft.Office.Interop.Word.Application
                wordApplication = new Microsoft.Office.Interop.Word.Application();
            wordApplication.Documents.Add(wordDocName);
            XpsDocument xpsDoc = null;

            Document doc = wordApplication.ActiveDocument;
            // You must ensure you have Microsoft.Office.Interop.Word.Dll version 12.
            // Version 11 or previous versions do not have WdSaveFormat.wdFormatXPS option
            try
            {
                doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS);
                wordApplication.Quit();

                xpsDoc = new XpsDocument(xpsDocName, System.IO.FileAccess.Read);
                return xpsDoc;
            }
            catch (Exception exp)
            {
                
            }
            return xpsDoc;
        }

        public void ReadInstructions()
        {
            var process = Process.GetCurrentProcess(); // Or whatever method you are using
            string fullPath = process.MainModule.FileName;
            string fileName = "temp.docx";
            fullPath = fullPath.Replace("CMT.exe", fileName);
            if(!File.Exists(fullPath))
            {
                MessageBox.Show("There is no file instruction.");
                UCstruct.isNxtEnabled = false;
                return;
            }
            XpsDocument xpsDoc = null;

            string newXPSDocumentName = String.Concat(System.IO.Path.GetDirectoryName(fullPath), "\\",
                           System.IO.Path.GetFileNameWithoutExtension(fullPath), ".xps");
            xpsDoc = ConvertWordDocToXPSDoc(fullPath, newXPSDocumentName);
            _dcDoc.Document = xpsDoc.GetFixedDocumentSequence();
            xpsDoc.Close();
            UCstruct.usNext = (UserControl)new FinalWin(_val);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReadInstructions();
        }
    }
}
