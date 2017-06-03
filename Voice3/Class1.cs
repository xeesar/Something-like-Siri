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
    class Class1
    {
        public string[] pogodaa = new string[4];
        int u = 0;
        int ui = 0;
        public string pogod;
        bool flag = false;
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();
        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        public void Pogoda(bool flag)
        {
            string data = GetHtmlPageText("http://pogoda.tut.by/");
            string tag1 = @"<div class=""fcurrent-h"">(.+)</div></div>";
            Regex regex = new Regex(tag1, RegexOptions.ExplicitCapture);
            MatchCollection matches = regex.Matches(data);
            int i = 0;
            u = 0;
            ui = 0;
            foreach (Match matche in matches)
            {
                if (i < 4)
                {
                    flag = false;
                    Pogoda1(matche.Value);
                    Pogoda2();
                    Pogoda3();
                    i++;
                    if (flag)
                    {
                        Speak();
                    }
                    else
                    {
                        int count = 0;
                        for (int o = 0; o < 1; o++)
                        {
                            pogodaa[count] = pogod;
                        }
                        count++;
                    }
                    pogod = "";
                }
            }
        }
        public void Speak()
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(pogod);
            sSynth.Speak(pBuilder);
        }
        public void Pogoda1(string t)
        {
            string tag2 = @"[\w*\s*]*";
            string tagg = @"[-\w*]*";
            if (!flag)
            {
                Regex regex = new Regex(tag2, RegexOptions.ExplicitCapture);
                MatchCollection matches = regex.Matches(t);
                int i = 0;
                foreach (Match matche in matches)
                {
                    if (i > 8)
                    {
                        if (matche.Value != "div")
                        {
                            if (matche.Value != "")
                            {
                                pogod += matche.Value + " ";
                            }
                        }
                    }
                    else i++;
                }
            }
            else
            {
                Regex regex = new Regex(tagg, RegexOptions.ExplicitCapture);
                MatchCollection matches = regex.Matches(t);
                int i = 0;
                foreach (Match matche in matches)
                {
                    if (i > 8)
                    {
                        if (matche.Value != "div")
                        {
                            if (matche.Value != "")
                            {
                                pogod += matche.Value + " ";
                            }
                        }
                    }
                    else i++;
                }
            }
        }
        public void Pogoda3()
        {
            string tag = @"<div class=""fcurrent-descr"">(.+)*";
            int j = 0;
            string data = GetHtmlPageText("http://pogoda.tut.by/");
            Regex regex = new Regex(tag, RegexOptions.ExplicitCapture);
            MatchCollection matches = regex.Matches(data);
            foreach (Match matche in matches)
            {
                if (ui < 4)
                {
                    if (j == ui)
                    {
                        flag = false;
                        Pogoda1(matche.Value);
                        j++;
                        break;
                    }
                    else j++;
                }
            }
        }
        public void Pogoda2()
        {
            int j = 0;
            string data = GetHtmlPageText("http://pogoda.tut.by/");
            string tag3 = @"<span class=""temp-i"">[\W*\d*]*";
            Regex regex = new Regex(tag3, RegexOptions.ExplicitCapture);
            MatchCollection matches = regex.Matches(data);
            foreach (Match matche in matches)
            {
                if (u < 4)
                {
                    if (j == u)
                    {
                        flag = true;
                        Pogoda1(matche.Value);
                        u++;
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
            }
        }
        public static string GetHtmlPageText(string url)
        {
            WebClient client = new WebClient();
            using (Stream data = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
