using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        int[,] matrixDup = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader file = File.OpenText("C:\\Users\\Eda\\Desktop\\NDPyuklenenodevler");

            string all_text = file.ReadToEnd();
             //Console.WriteLine(all_text);
            int line_count = all_text.Split('\n').Count();//\n sayısı bize olması gereken dizi eleman saysını verir
            string[] arr = new string[line_count];

            int i = 0;
            foreach (var line in all_text.Split('\n'))
            {
                arr[i] = line;
                i++;
            }//arr dizisine lineların ataması yapıldı.
            int column_count = 3;//daha sonra  buraya foreach içinde //column_count = line.Split(';').Count(); olarak tanım yapıcaz

            int[,] matrix = new int[line_count, column_count];
           
            for ( int k = 0; k < arr.Length; k++)
            {
                for (int p = 0;  p < arr[k].Split(';').Length; p++)
                {
                    string value = arr[k].Split(';')[p].Replace("X:", "").Replace("Y:", "").Replace("Z:", "");
                    matrix[k, p] = Convert.ToInt32(value);
                }
            }
            // Matris hazır, bunu class değişkenine kopyaladım.
            matrixDup = matrix;
            int current_column = 0;
            richTextBox1.Font = new Font("Times New Roman", 20);

            for (int current_line = 0; current_line < line_count; current_line++)
            {
                AppendText(matrix[current_line, current_column] + " ", Color.Green, false);
                current_column += 1;
                AppendText(matrix[current_line, current_column] + " ", Color.Blue, false);
                current_column += 1;
                AppendText(matrix[current_line, current_column] + " ", Color.Red, true); // Bu satır için yazma işlemimiz bittiği için alt satıra geçiyoruz.
                current_column = 0; // Bir alt satıra geçtiğimiz için şu anki sütun 0. sütun olmuş oluyor.
            }
        // Form1Load metodunun sonu
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x = 0; int y = 0;

            if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("Bu kısımlar boş bırakılamaz");
            }
            if (!(textBox1.Text == "" || textBox2.Text == ""))
            {
                x = Convert.ToInt32(textBox1.Text);
                y = Convert.ToInt32(textBox2.Text);
            }

            if (x > 999 || x < 500)
            {
                MessageBox.Show("Lütfen X için 500-999 Arası Bir Sayı Giriniz:");
                x = 0;
                textBox1.Text = "";
                richTextBox2.Visible = false;
            }
            if (y < 1000 || y > 1499)
            {
                MessageBox.Show("Lütfen Y için 1000-1499 Arasında Bir Sayı Giriniz:");
                y = 0;
                textBox2.Text = "";
                richTextBox2.Visible = false;
            }
            if (!(y == 0 || x == 0))
            {
                richTextBox2.Visible = true;
          
            }
            richTextBox2.Clear();
            double[,] result = new double[matrixDup.GetLength(0), 2];
            for (int i = 0; i < matrixDup.GetLength(0); i++)
            {
                result[i, 1] = Math.Sqrt(Math.Pow((matrixDup[i, 0] - x),2) +Math.Pow( (matrixDup[i, 1] - y) , 2));
                result[i, 0] = matrixDup[i, 2]; //  Z değeri
            }
            // Bubble Sort
            double temp1, temp2;
            for (int i = 0; i < matrixDup.GetLength(0); i++)
            {
                for (int j = 0; j < matrixDup.GetLength(0) - (i+1); j++)
                {
                    if (result[j, 1] > result[j + 1, 1]) // 1. sütunla karşılaştır
                    {
                        temp1 = result[j, 0];              // 0. sütunla 1. sütunu değiştir.
                        temp2 = result[j, 1];

                        result[j, 0] = result[j + 1, 0];
                        result[j, 1] = result[j + 1, 1];

                        result[j + 1, 0] = temp1;
                        result[j + 1, 1] = temp2;
                    }
                }
            }
            for (int i = 0; i < result.GetLength(0); i++)
            {
                AppendTextTo2(result[i, 0]+" ");

                AppendTextTo2(result[i, 1] + " ", true);
            }
            // Bubble Sort Sıralama Algoritmasıyla sıralandı.
            // ilk 5 indexteki z sayılarını sayıyoruz.
            int n = 5; // Dikkate almak istediğimiz z sayısı (ilk n tanesini almak için)
            int iBest = -1;  // en çok geçen alt kümedeki ilk sayının indexi
            int nBest = -1;  // en çok geçen sayının kaç kez geçtiği
                             // for -> her alt küme için
            for (int i = 0; i < n;)
            {
                int ii = i; // ii = alt kümedeki ilk sayı dizini
                int nn = 0; // nn = alt kümedeki sayıların sayısı
                            // alt küme içindeki her sayı için, say!
                for (; i < n && result[i,0] == result[ii,0]; i++, nn++) { }
                // alt kümenin şu ana kadar best'ten daha fazla numarası varsa
                // best = newBest // besti yenisiyle değiştir.
                if (nBest < nn) { nBest = nn; iBest = ii; }
            }
            double mostFreq = result[iBest,0];
            sonuc.Text = mostFreq + " " ;
        }
       
        private void richTextBox1_TextChanged(object sender, EventArgs e){}

        public void AppendText(string text, Color color, bool addNewLine = false)//AppendText metodunu tanımladık.
        {
            richTextBox1.SuspendLayout();
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(addNewLine
                ? $"{text}{Environment.NewLine}"
                : text);
            richTextBox1.ResumeLayout();
        }

        public void AppendTextTo2(string text, bool addNewLine = false)//Color parametresi olmadan tanımladık.
        {
            richTextBox2.SuspendLayout();
            richTextBox2.SelectionColor = Color.Red;
            richTextBox2.AppendText(addNewLine
                ? $"{text}{Environment.NewLine}"
                : text);
            richTextBox2.ResumeLayout();
        }

        private void label1_Click(object sender, EventArgs e){}

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
