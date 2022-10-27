using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MSSQLForCSProgs
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString);
            sqlConnection.Open();
            
            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }
        }

        private void btnINSERT_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [Students] (Name, Surname, Birthday, Birthplace, Phone, Email) VALUES (@Name, @Surname, @Birthday, @Birthplace, @Phone, @Email)", sqlConnection);

            DateTime date = DateTime.Parse(tbBirthday.Text);
            command.Parameters.AddWithValue("Name", tbName.Text);
            command.Parameters.AddWithValue("Surname", tbSurname.Text);
            command.Parameters.AddWithValue("Birthday", $"{date.Month}/{date.Day}/{date.Year}");
            command.Parameters.AddWithValue("Birthplace", tbBirthplace.Text);
            command.Parameters.AddWithValue("Phone", tbPhone.Text);
            command.Parameters.AddWithValue("Email", tbEmail.Text);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(tbSelect.Text, sqlConnection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dgvView.DataSource = dataSet.Tables[0];
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Некорректный запрос, проверьте синтаксис");
            }
            
            catch
            {
                MessageBox.Show("Проверьте запрос");
            }

        }

        private void btnSelect2_Click(object sender, EventArgs e)
        {
            lv2.Items.Clear();
            SqlDataReader dataReader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products", sqlConnection); 
                dataReader = sqlCommand.ExecuteReader();
                ListViewItem item = null;
                while (dataReader.Read())
                {
                    item = new ListViewItem(new String[] { Convert.ToString(dataReader["ProductName"]), 
                        Convert.ToString(dataReader["QuantityPerUnit"]), 
                        Convert.ToString(dataReader["UnitPrice"]) });
                    lv2.Items.Add(item);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }
    }
}
