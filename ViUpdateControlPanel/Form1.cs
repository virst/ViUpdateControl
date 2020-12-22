using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SngMediaApp.Utils;
using UpdService.Objects;
using File = System.IO.File;

namespace ViUpdateControlPanel
{
    public partial class Form1 : Form
    {
        public string Server = "http://localhost:32625";

        private WebClient wc = new WebClient() {Encoding = Encoding.UTF8};

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var a = wc.DownloadString(Server + "/AppInfo");
            var ll = JsonUtil<List<App>>.ObjFromStr(a);
            if(ll.Count==0) return;
            listBox1.DataSource = ll;
            listBox1.SelectedIndex = 0;
            /*
            byte[] bb = new byte[27];
            wc.UploadData(Server + "/Files/Load/12", bb);
            */
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            App a = listBox1.SelectedItem as App;
            listBox2.DataSource = a?.Files;
        }

        private void label1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            var a = listBox1.SelectedItem as UpdService.Objects.App;
            if (a == null) return;
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            foreach (string file in files)
            {
                if(!File.Exists(file)) continue;
                var f = Path.GetFileName(file);
                if(MessageBox.Show($"Загрузить {f} ?","",MessageBoxButtons.YesNo)!= DialogResult.Yes) continue;
                var bb = File.ReadAllBytes(file);
                wc.UploadData(Server + $"/Files/Load/{a.Id}/{f}", bb);
            }

            Form1_Load(null,null);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var a = listBox2.SelectedItem as UpdService.Objects.File;
            e.Cancel = a == null;
            if (a == null) return;
            tsmiRun.Text = a.Run ? "Назначить не запускаемым" : "Назначить запускаемым";

        }

        private void tsmiRun_Click(object sender, EventArgs e)
        {
            var a = listBox2.SelectedItem as UpdService.Objects.File;
            if (a == null) return;
            var rm = a.Run ? 0 : 1;
            wc.DownloadString(Server + $"/Files/SetRun/{a.Id}/{rm}");
            a.Run = !a.Run;
            var tmp = listBox2.DataSource;
            listBox2.DataSource = null;
            listBox2.DataSource = tmp;
        }

        private void tsmiNew_Click(object sender, EventArgs e)
        {
            var NewName = InputMessageBox.Show("Наименование:", "Создать", "Application1");
            if (string.IsNullOrWhiteSpace(NewName)) return;
            var a = wc.DownloadString(Server + "/AppInfo/Add/" + NewName.Replace(' ','_'));
            Form1_Load(null,null);
        }
    }
}
