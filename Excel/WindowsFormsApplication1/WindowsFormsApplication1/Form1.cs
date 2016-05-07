using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string html = WindowsFormsApplication1.Properties.Resources.HTMLPage1;

           // webBrowser1.DocumentText = html;

            DataSet dataSet = new DataSet();
            StringWriter sw = new StringWriter();

            dataSet.WriteXml(sw);
            webBrowser1.DocumentText = "<!DOCTYPE html><html><body><p>Click the button to create a Hidden Input Field.</p><button onclick='myFunction()'>Try it</button><p id='demo'></p><script>function myFunction() {  var x = document.createElement('INPUT');    x.setAttribute('type', 'hidden');  document.body.appendChild(x); document.getElementById('demo').innerHTML = 'The Hidden Input Field was created. However, you are not able to see it because it is hidden (not visible).';}</script></body></html>";

            
        }
    }
}
