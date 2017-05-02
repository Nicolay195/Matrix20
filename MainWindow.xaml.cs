using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Matrix20
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool loadSuccess = false;
        bool readSuccess = true;
        bool workInTextbox = false;

        int rows, collum;
        
        public void WorkInTextbox()
        {
            string lines = textBox.Text;

            int[] massive = lines.Split(new char[] { ' ', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            int collum = 0;
            int rows = textBox.LineCount;

            for (int i = 0; lines[i] != '\n'; i++)
            {
                if (lines[i] == ' ')
                    collum++;
            }
            collum++;

            int[,] newArr = new int[rows, collum];

            int rowNumber = Convert.ToInt32(textBox1.Text) - 1;
            int collumNumber = Convert.ToInt32(textBox2.Text) - 1;
            textBox.Clear();
            int p = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < collum; j++)
                {
                    newArr[i, j] = massive[p];
                    p++;
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < collum; j++)
                {
                    if ((i == rowNumber) && (j == collumNumber))
                    {
                        textBox.Text += newArr[i, j].ToString() + "*" + " ";
                    }

                    else
                    {
                        textBox.Text += newArr[i, j].ToString() + " " ;
                    }
                }
                textBox.Text += Environment.NewLine;
            }


        }

        public void WorkInFile()
        {

            int rowNumber = Convert.ToInt32(textBox1.Text) - 1;
            int collumNumber = Convert.ToInt32(textBox2.Text) - 1;

            textBox.Clear();
            textBox.IsEnabled = false;
            int[,] newArr = new int[rows, collum];
            string[] lines = File.ReadAllLines("1.txt");
            int[] first = lines[0].Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            int sizeRow = lines.Count();
            int sizeCollum = first.Count();

            //    rows = sizeRow;
            //   collum = sizeCollum;

            int[,] arr = new int[sizeRow, sizeCollum];

            for (int i = 0; i < sizeRow; i++)
            {
                int[] row = lines[i].Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

                for (int j = 0; j < sizeCollum; j++)
                {
                    arr[i, j] = row[j];
                }
            }
            // Записываем в textbox
            for (int i = 0; i < sizeRow; i++)
            {
                for (int j = 0; j < sizeCollum; j++)
                {
                    if ((i == rowNumber) && (j == collumNumber))
                    {
                        textBox.Text += arr[i, j].ToString() + "*" + " ";
                    }

                    else
                    {
                        textBox.Text += arr[i, j].ToString() +" ";
                    }
                }
                textBox.Text += Environment.NewLine;
            }
        }

        public void MenuItem_Click_Open(object sender, RoutedEventArgs e)
        {
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = true;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Выберите файл";
            openFileDialog1.Filter = "Текстовые файлы|*.txt";
            openFileDialog1.ShowDialog();
            string[] lines = File.ReadAllLines(openFileDialog1.FileName);

            int[] first = lines[0].Split(new char[] { ' ' }, 
                    StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            int sizeRow = lines.Count();
            int sizeCollum = first.Count();

            rows = sizeRow;
            collum = sizeCollum; // коммент
                    
             int[,] arr = new int[sizeRow, sizeCollum];

            for (int i = 0; i < sizeRow; i++)
            {
                int[] row = lines[i].Split(new char[] { ' ' }, 
                    StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

                if (sizeCollum != row.Count())
                {
                    MessageBox.Show("Неправильный формат матрицы, сорян");
                    readSuccess = false;
                    break;
                }
                    
                for (int j = 0; j < sizeCollum; j++)
                {
                    arr[i, j] = row[j];
                }
            }
            // вывести в текстбокс
            if (readSuccess == true)
            {
                for (int i = 0; i < sizeRow; i++)
                {
                    for (int j = 0; j < sizeCollum; j++)
                    {
                        textBox.Text += arr[i, j].ToString() + " ";
                    }
                    textBox.Text += Environment.NewLine;
                }
                loadSuccess = true;
            }
                button.IsEnabled = true;

        }

        private void MenuItem_Click_Save_As(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ShowDialog();

            string[] lines = File.ReadAllLines("1.txt");
            int[,] arr = new int[rows, collum];

            FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            int rowNumber = Convert.ToInt32(textBox1.Text) - 1;
            int collumNumber = Convert.ToInt32(textBox2.Text) - 1;

            for (int i = 0; i < rows; i++)
            {
                int[] row = lines[i].Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
                for (int j = 0; j < collum; j++)
                {
                    arr[i, j] = row[j];
                }
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < collum; j++)
                {
                    if ((i == rowNumber) && (j == collumNumber))
                    {
                        sw.Write(arr[i, j] + "*" + "\t");
                    }

                    else
                    {
                        sw.Write(arr[i, j] + "\t");
                    }
                }
                textBox.Text += Environment.NewLine;
                sw.Write("\r\n");
            }
            sw.Close();

        }

        private void MenuItem_Click_Create(object sender, RoutedEventArgs e)
        {
            textBox.IsEnabled = true;
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = true;
            workInTextbox = true;
            button.IsEnabled = true;

        }


        public void button_Click_Remove(object sender, RoutedEventArgs e)
        {
           

            if (workInTextbox == true)
                WorkInTextbox();
            else
                WorkInFile();
        }
    }
}
