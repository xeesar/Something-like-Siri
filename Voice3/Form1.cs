using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Microsoft.Speech.Recognition;
using System.Xml.Linq;
using System.Threading;
using System.Net;
using System.IO;
namespace Voice3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Class1 class1 = new Class1();
        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();
        Choices sList = new Choices();
        public bool job = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            sSynth.Volume = 100;
            sList.Add(new string[] { "Сири что ты думаешь о политике", "Сколько время", "Выход", "Я на паре","Какая сейчас погода"});
            //XDocument xDoc = XDocument.Load(@"http://net2ftp.ru/node0/evtuhv@mail.ru/Dialog.xml");
            //var Dl = from Dialog in xDoc.Descendants("Dialogs")
            //           select new 
            //           {
            //               Question = Dialog.Attribute("id").Value
            //           };
            //foreach (var bk in Dl)
            //{
            //    sList.Add(bk.Question);
            //}
            //Start();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            END();
            string pass = @"E:\Dialog.xml";
            File.Delete(pass);
            Application.Exit();
        }
        public void Speak()
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(class1.pogod);
            sSynth.Speak(pBuilder);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(textBox1.Text);
            sSynth.Speak(pBuilder);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = true;
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            //try
            //{
            sRecognize.RequestRecognizerUpdate();
            sRecognize.LoadGrammar(gr);
            sRecognize.SpeechRecognized += sReconize_SpeechRecognized;
            sRecognize.SetInputToDefaultAudioDevice();
            sRecognize.RecognizeAsync(RecognizeMode.Multiple);
            //}
            /*catch
            {
                return;
            }*/
        }

       

        private void sReconize_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "Выход")
            {
                Application.Exit();
            }
            else if (e.Result.Text == "Сколько время")
            {

                pBuilder.ClearContent();
                pBuilder.AppendText("Правильнее говорить который час!" + DateTime.Now.ToLongTimeString());
                sSynth.Speak(pBuilder);
            }
            else if (e.Result.Text == "Сири что ты думаешь о политике")
            {
                pBuilder.ClearContent();
                pBuilder.AppendText("Кто такая сири урод?");
                sSynth.Speak(pBuilder);
            }
            else if (e.Result.Text == "Я на паре")
            {
                pBuilder.ClearContent();
                pBuilder.AppendText("Хорошо, извини, сделаю громкость поменьше.");
                sSynth.Speak(pBuilder);
                sSynth.Volume = 10;
            }
            else if (e.Result.Text == "Кто ты")
            {
                pBuilder.ClearContent();
                pBuilder.AppendText("Я искусственный интелект Лера");
                sSynth.Speak(pBuilder);
            }
            else if(e.Result.Text == "Какая сейчас погода")
            {
                job = true;
                class1.Pogoda(job);
            }
            else
            {
                Random n = new Random();
                int i = n.Next(1, 4);
                XDocument xDoc = XDocument.Load(@"http://net2ftp.ru/node0/evtuhv@mail.ru/Dialog.xml");
                var Dl = from Dialog in xDoc.Descendants("Dialogs")
                         where Dialog.Attribute("id").Value == e.Result.Text
                         select new
                         {

                             Answer = Dialog.Element("d" + i).Value
                         };
                foreach (var bk in Dl)
                {
                    pBuilder.ClearContent();
                    pBuilder.AppendText(bk.Answer);
                    sSynth.Speak(pBuilder);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = false;
            sRecognize.RecognizeAsyncStop();
        }

        private void добавитьДиалогиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ADD ad = new ADD();
            ad.Show();
        }

        public void Start()
        {
            string ftpUserID = "evtuhv@mail.ru";
            string ftpPassword = "f171ed7d";
            string path = "ftp://93.189.45.35/public_http/Dialog.xml"; // путь к файлу на хостинге
            string to = "E:/Dialog.xml"; // путь куда сохранять
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                FileStream file = File.Create(to);
                byte[] buffer = new byte[512 * 1024];
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, read);
                }
                file.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки файла!");
                return;
            }

        }

        public void END()
        {
            string ftpUserID = "evtuhv@mail.ru";
            string ftpPassword = "f171ed7d";
            string path = "ftp://93.189.45.35/public_http/Dialog.xml"; // путь к файлу на хостинге
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                //Load the file
                FileStream stream = File.OpenRead(@"E:\Dialog.xml");
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                //Upload file
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки файла!");
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            job = false;
            class1.Pogoda(job);
            for(int i =0;i<class1.pogodaa.Length;i++)
            {
                richTextBox1.Text += class1.pogodaa[i] + "\n";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            job = true;
            class1.Pogoda(job);
        }
    }
}
