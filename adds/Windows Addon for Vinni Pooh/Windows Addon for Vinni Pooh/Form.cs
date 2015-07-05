using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        TreeNode[] Nodes;
        System.Text.Encoding Encode = System.Text.Encoding.GetEncoding(1251);
        System.IO.StreamReader LoadFile;
        System.IO.StreamWriter UpdateFile;
        int Maxlevel = 0;
        int a, b;
        public MainForm()
        {
            InitializeComponent();
        }


        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadFile = new System.IO.StreamReader(Application.StartupPath + "\\SavedGame.txt", Encode);
            a = Convert.ToInt32(LoadFile.ReadLine());
            b = Convert.ToInt32(LoadFile.ReadLine());
            Maxlevel = Convert.ToInt32(LoadFile.ReadLine());
            LoadFile.Close();
            treeView1.Enabled = true;
            Nodes = new TreeNode[100];
            for (int i = 1; i <= Maxlevel; i++)
            {
                Nodes[i] = new TreeNode();
                treeView1.Nodes.Add(Nodes[i]);
                Nodes[i].Text = "level" + Convert.ToString(i);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            groupBox1.Enabled = true;
            LoadFile = new System.IO.StreamReader(Application.StartupPath + "\\levels\\" + treeView1.SelectedNode.Text + ".txt", Encode);
            textBox1.Text = LoadFile.ReadLine();
            textBox2.Text = LoadFile.ReadLine();
            textBox3.Text = LoadFile.ReadLine();
            LoadFile.Close();
            if(textBox3.Text.Length > 1) textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 2);
        }


        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nodes[Maxlevel].Remove();
            Maxlevel--;
            UpdateFile = new System.IO.StreamWriter(Application.StartupPath + "\\SavedGame.txt", false, Encode);
            UpdateFile.WriteLine(a);
            UpdateFile.WriteLine(b);
            UpdateFile.WriteLine(Maxlevel);
            UpdateFile.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateFile = new System.IO.StreamWriter(Application.StartupPath + "\\levels\\" + treeView1.SelectedNode.Text + ".txt", false, Encode);
            UpdateFile.WriteLine(textBox1.Text);
            UpdateFile.WriteLine(textBox2.Text);
            UpdateFile.WriteLine(textBox3.Text + "+`");
            UpdateFile.Close();
        }

        private void додатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maxlevel++;
            Nodes[Maxlevel] = new TreeNode();
            treeView1.Nodes.Add(Nodes[Maxlevel]);
            Nodes[Maxlevel].Text = "level" + Convert.ToString(Maxlevel);
            UpdateFile = new System.IO.StreamWriter(Application.StartupPath + "\\SavedGame.txt", false, Encode);
            UpdateFile.WriteLine(a);
            UpdateFile.WriteLine(b);
            UpdateFile.WriteLine(Maxlevel);
            UpdateFile.Close();
            UpdateFile = new System.IO.StreamWriter(Application.StartupPath + "\\levels\\level" + Convert.ToString(Maxlevel) + ".txt", false, Encode);
            UpdateFile.Close();
        }

    }
}
