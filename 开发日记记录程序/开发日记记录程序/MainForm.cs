using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSWord = Microsoft.Office.Interop.Word;
using System.Reflection;
namespace 开发日记记录程序
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
            Timer t = new Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(Timer_Tick);
            t.Start();
            label3.Text = DateTime.Now.ToLongDateString().ToString();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = Path.GetFullPath(openFileDialog1.FileName);
            Console.Write(textBox1.Text);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = Path.GetFullPath(openFileDialog1.FileName);
            Console.Write(textBox1.Text);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Word.Document wordDoc = null;                  //Word文档变量
            string strContent;                        //文本内容变量
            Microsoft.Office.Interop.Word.Application wapp = new Microsoft.Office.Interop.Word.Application();
            Object Nothing = Missing.Value;  //由于使用的是COM库，因此有许多变量需要用Missing.Value代替
            object filename = null;
            object isread = false;
            object isvisible = true;
            object miss = System.Reflection.Missing.Value;
            wapp.Visible = false;
            if (textBox1.Text == "")
            {
                DialogResult r = MessageBox.Show("请输入日记文件路径！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (r == DialogResult.OK)
                {
                    return;
                }
            }
            else if (System.IO.File.Exists(textBox1.Text))
            {
                filename = textBox1.Text;
            }
            try
            {
                wordDoc = wapp.Documents.Open(ref filename, ref miss, ref isread, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref isvisible, ref miss, ref miss, ref miss, ref miss);
                wapp.Selection.ParagraphFormat.LineSpacing = 16f;//设置文档的行间距
                wapp.Selection.ParagraphFormat.FirstLineIndent = 30;//首行缩进的长度

                //写入普通文本
                strContent = DateTime.Now.ToLocalTime().ToString();
                wordDoc.Paragraphs.Last.Range.Text = "时间:" + strContent + "\n";
                wordDoc.Paragraphs.Last.Range.Text = "遇到问题：\n";
                wordDoc.Paragraphs.Last.Range.Text = textBox2.Text + "\n";
                wordDoc.Paragraphs.Last.Range.Text = "解决方案：\n";
                wordDoc.Paragraphs.Last.Range.Text = textBox3.Text + "\n";
                wordDoc.Paragraphs.Last.Range.Text = "备注：\n";
                wordDoc.Paragraphs.Last.Range.Text = textBox4.Text + "\n";

                //直接添加段，不是覆盖( += )
                //wordDoc.Paragraphs.Last.Range.Text += "不会覆盖的,";
                //添加在此段的文字后面，不是新段落
                //wordDoc.Paragraphs.Last.Range.InsertAfter("这是后面的内容\n");
            }
            finally
            {
                if (wordDoc != null)
                {
                    wordDoc.Close(ref miss, ref miss, ref miss);
                    wordDoc = null;
                }
                if (wapp != null)
                {
                    wapp.Quit(ref miss, ref miss, ref miss);
                    wapp = null;
                }
            }
            wapp = null;
            DialogResult result = MessageBox.Show("提交成功！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            if (result == DialogResult.OK)
            {
                Process.GetCurrentProcess().Kill();
            }          
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                e.Cancel = false;  //点击OK   
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)//关于窗体的在这
        {
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }
    }

}
