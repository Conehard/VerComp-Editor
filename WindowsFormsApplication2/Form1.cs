using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
   
    public partial class Form1 : Form
    {
        public static string diretorio = "";
        public Form1()
        {
            InitializeComponent();  
        }

        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Arquivo dat | *.dat";
            sfd.FileName = "VerComp.dat";
            if (File.Exists(diretorio + "\\VerComp.dat")|| File.Exists(diretorio + "\\ver"))
            {
                try
                {
                    File.Delete(diretorio + "\\VerComp.dat");
                    File.Delete(diretorio + "\\ver");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            sfd.ShowDialog();
            
            if (string.IsNullOrEmpty(sfd.FileName)==false)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        writer.WriteLine(diretorio);
                        writer.WriteLine(textBox3.Text);
                        writer.Flush();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Não foi possível salvar o arquivo. Erro: {0}", ex.Message),"Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox3.Text = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                listBox1.HorizontalScrollbar = true;
                StringBuilder sb = new StringBuilder();
                String[] allfiles = Directory.GetFiles(fbd.SelectedPath, "*.*", SearchOption.AllDirectories);

                foreach (string file in allfiles)
                {
                    long info = new FileInfo(file).Length;
                    using (FileStream arquivo = new FileStream(file, FileMode.Open)) {
                        HashAlgorithm hash;
                        hash = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        if (hash != null)
                        {
                            byte[] retVal = hash.ComputeHash(arquivo);
                            for (int i=0; i< retVal.Length; i++)
                            {
                                int k=0;
                                k++;
                                if (k < 32)
                                {
                                    sb.Append(retVal[i].ToString("x2"));
                                }                                
                            }
                            diretorio = fbd.SelectedPath+"\\";
                            textBox3.AppendText(Path.GetFullPath(file) + "," + sb.ToString() + "," + info + "\n");
                            textBox3.Text = textBox3.Text.Replace(diretorio, "");
                            textBox3.Text = textBox3.Text.Replace("\\", "/");
                            textBox3.Text = textBox3.Text.Replace(diretorio + "/", "");
                            sb.Clear();
                        }
                        hash.Dispose();
                    }  
                        listBox1.Items.Add(file);
                        int numfile = listBox1.Items.Count;
                        label1.Text = "Número de arquivos: " + numfile;
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arquivo dat | *.dat";
            ofd.ShowDialog();
            listBox1.Enabled = false;
            if (string.IsNullOrEmpty(ofd.FileName) == false)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(ofd.FileName, Encoding.GetEncoding(CultureInfo.GetCultureInfo("pt-BR").TextInfo.ANSICodePage)))
                    {
                        textBox3.Text = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Não foi possível abrir o arquivo. Erro: {0}", ex.Message), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Selecione o arquivo";
                ofd.ShowDialog();
                StringBuilder sb = new StringBuilder();
                using (FileStream arquivo = new FileStream(ofd.FileName, FileMode.Open))
                {
                    HashAlgorithm hash;
                    hash = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    if (hash != null)
                    {
                        byte[] retVal = hash.ComputeHash(arquivo);
                        for (int i = 0; i < retVal.Length; i++)
                        {
                                sb.Append(retVal[i].ToString("x2"));
                        }
                    }
                    hash.Dispose();
                    string verhash = sb.ToString();
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = "ver";
                    sfd.ShowDialog();

                    if (string.IsNullOrEmpty(sfd.FileName) == false)
                    {
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                            {
                                writer.WriteLine(verhash);
                                writer.Flush();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format("Não foi possível salvar o arquivo. Erro: {0}", ex.Message), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Não foi possível abrir o arquivo. Erro: {0}", ex.Message), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void comoUsarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(" Criação do VerComp \n NÃO DEIXE NENHUM arquivo como somente leitura \n 1 - Selecione a pasta com os arquivos de atualização.\n 2 - Salve o arquivo (Arquivo -> Salvar Como ou Ctrl+S).\n -------------------------------------------------------------------\n Criação do Ver \n 1 - Clique em Gerar Ver. \n 2 - Selecione o VerComp.dat criado anteriormente. \n 3 - Salve seu Ver."), "Como Usar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void eGENDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("A EGEND é uma empresa de tecnologia, especializada no desenvolvimento e distribuição de produtos digitais, buscamos unir eficiência e simplicidade em todos nossos projetos para que todos os produtos sejam acessíveis a qualquer tipo de pessoa."), "Sobre a EGEND", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void verCompEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("Versão 1.4"), "Sobre o VerComp Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Process.Start("http://egendnetwork.com/");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
