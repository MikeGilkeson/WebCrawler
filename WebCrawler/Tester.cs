using System;
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
            webCrawler.CrawlInternet(internet.FirstPage);

            outputTextBox.Text = string.Format("Success:\r\n{0}\r\n\r\nSkipped:\r\n{1}\r\n\r\nError:\r\n{2}", webCrawler.GetSuccessfullyVisitsJson(), webCrawler.GetSkippedJson(), webCrawler.GetErrorsJson());
        }
    }
}
