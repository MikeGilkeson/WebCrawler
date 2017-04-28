using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WebCrawler
{
    public partial class Tester : Form
    {
        public Tester()
        {
            InitializeComponent();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            var internet = Internet.Parse(inputTextBox.Text);
            var webCrawler = new WebCrawler(internet);
            webCrawler.CrawlInternet(internet.Pages[0]);

            outputTextBox.Text = string.Format("Success:\r\n{0}\r\n\r\nSkipped:\r\n{1}\r\n\r\nError:\r\n{2}", webCrawler.GetSuccessfullyVisitsJson(), webCrawler.GetSkippedJson(), webCrawler.GetErrorsJson());
        }
    }
}
