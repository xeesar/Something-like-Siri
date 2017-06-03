using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Voice3
{
    public partial class ADD : Form
    {
        string path = "E:/Dialog.xml";
        public ADD()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XDocument xDoc = XDocument.Load(path);
            //Сохранение
            xDoc.Root.Add(new XElement("Dialogs", new XAttribute("id", textBox1.Text),
                new XElement("d1", textBox2.Text),
                new XElement("d2", textBox3.Text),
                new XElement("d3", textBox4.Text)));
            xDoc.Save(path);
            MessageBox.Show("Добавлено в словарь", "Спасибо", MessageBoxButtons.OK);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
