using System;
using System.IO;
using System.Windows.Forms;

namespace Employee
{
    public partial class Form3 : Form
    {
        string[] data = new string[2];  //Array to save a password hint and type of password hint from a text file
        Password password = new Password();

        public Form3()
        {
            InitializeComponent();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Enabled = false;
            textBox4.Enabled = false;

            if (File.Exists("jvfd.txt"))
            {
                using (StreamReader reader = new StreamReader("jvfd.txt"))
                {
                    data[0] = reader.ReadLine();  //Tooltip type
                    data[1] = reader.ReadLine();  //Tooltip
                }
            }
            else
            {
                MessageBox.Show("Помилка! Пароль не знайдено");
            }
            
            label1.Text = data[0];   //Changes the caption to the type of password hint
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)   //The user wants to change the tooltip to a new one
            {
                comboBox1.Enabled = true;
                textBox4.Enabled = true;
                comboBox1.Items.AddRange(new string[] { "Ім'я домашньої таврини", "Дівоче прізвище мами", "Улюблене число" });
            }
            else if (radioButton1.Checked == false)   //The user wants to keep the old tooltip
            {
                comboBox1.Enabled = false;
                textBox4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != data[1])
            {
                MessageBox.Show("Відповідь на підказку неправильна!");
            }
            else if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Паролі не співпадають!");
            }
            else if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Всі поля повинні бути заповнені!");
            }
            else if (comboBox1.Enabled == true && textBox4.Text == "")
            {
                MessageBox.Show("Всі поля повинні бути заповнені!");
            }
            else if (comboBox1.Text == "" && comboBox1.Enabled == true)
            {
                MessageBox.Show("Оберіть тип підказки");
            }
            else
            {
                password.SavePassword(comboBox1, textBox4, textBox2);   //Saves the new password

                Application.OpenForms["Form3"].Close();   //Closes the password change form (this one)
            }
        }
    }
}