using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;
using System.Data.SQLite;
using System.Data.Common;
using System.Data.SqlClient;

namespace ChildrenClassDB
{
    public partial class Form1 : Form
    {
        string filename;
        bool chooseDB = false;
        public Form1()
        {
            InitializeComponent();
        }
        private DBJob db;
        string textTable;



        private void button1_Click(object sender, EventArgs e)
        {

            if (chooseDB)
            {
                MessageBox.Show($"Соединение с базой {filename}");
                filename = openFileDialog1.FileName;
                db.Conn.Open();
                
            }
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                db = new SQLiteDBJob(filename);
                chooseDB = true;
            }
            string sql = "Select * from sqlite_master";
           // string sql = "SELECT name FROM sqlite_master WHERE type='tableName'";
            DataTable DtProduct = new DataTable();
            DbDataAdapter adProduct = db.GetDataAdapter(sql);
            adProduct.Fill(DtProduct);
            dataGridView1.DataSource=DtProduct;
        


        }

        private void button3_Click(object sender, EventArgs e)
        {
            db = new SQLDBJob("");

            int numberOfPopularBooks = 3; // 3 самые популярные книги

            //  SQL-запрос 
            string sql = $"SELECT TOP {numberOfPopularBooks}  Title, TotalCopies - CopiesInLibrary As MostPopular\r\nFROM Books\r\nORDER BY MostPopular DESC;";
          
            DataTable DtProduct = new DataTable();
            DbDataAdapter adProduct = db.GetDataAdapter(sql);
            adProduct.Fill(DtProduct);
            dataGridView1.DataSource = DtProduct;
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!button1.Enabled)
            {
                textTable = textBox1.Text;
                bool answer = db.HasTable(textTable);
                MessageBox.Show($"Указанная таблица = {answer}");
            }
            else
            {
                MessageBox.Show("Сначала нажмите кнопку 'Коннект SQLite'");
            }
          

            }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!button6.Enabled)
            {
                textTable = textBox1.Text;
                bool answer = db.HasTable(textTable);
                MessageBox.Show($"Указанная таблица = {answer}");
            }
            else
            {
                MessageBox.Show("Сначала нажмите кнопку 'Коннект SQL'");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string ConnString = @"Data Source=DESKTOP-EKD1ADK;Initial Catalog=Library;Integrated Security=True";
            db = new SQLDBJob(ConnString);
            db.Conn.Open();
            MessageBox.Show("Соединение с базой установлено");

        }
    }
    }

