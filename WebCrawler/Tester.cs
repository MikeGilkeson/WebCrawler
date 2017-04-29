using System;
using System.Diagnostics;
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
            try
            {
                outputLabel.Text = "Output";
                var internet = Internet.Parse(inputTextBox.Text);
                var visitedPages = new WebAddressCollection();
                var skippedPages = new WebAddressCollection();
                var badPages = new WebAddressCollection();
                var webCrawler = new WebCrawler(internet, visitedPages, skippedPages, badPages);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                webCrawler.CrawlInternet(internet.FirstPage);
                stopwatch.Stop();
                outputLabel.Text = string.Format("Output - time to CrawlInternet: {0}", stopwatch.Elapsed);
                outputTextBox.Text = string.Format("Success:\r\n{0}\r\n\r\nSkipped:\r\n{1}\r\n\r\nError:\r\n{2}", visitedPages.GetJson(), skippedPages.GetJson(), badPages.GetJson());
            }
            catch (InternetParseException internetParseException)
            {
                outputTextBox.Text = internetParseException.Message + "\r\n" + internetParseException.InnerException.ToString();
            }
            catch (CrawlException crawlException)
            {
                outputTextBox.Text = crawlException.Message + "\r\n" + crawlException.InnerException.ToString();
            }
            catch (Exception exception)
            {
                outputTextBox.Text = "An unexpected error happened while trying to crawl.\r\n" + exception.ToString();
            }
        }
    }
}
