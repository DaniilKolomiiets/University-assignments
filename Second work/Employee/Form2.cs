using System;
using System.Windows.Forms;

namespace Employee
{
    public partial class Form2 : Form
    {
        bool isOk = false;    //Are the fields filled in correctly
        Password password = new Password();

        public Form2()
        {
            InitializeComponent();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.AddRange(new string[] { "Ім'я домашньої таврини", "Дівоче прізвище мами", "Улюблене число" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckThisForm();

            if (isOk)
            {
                password.SavePassword(comboBox1, textBox3, textBox1);   //Saves a new password to a text file (the Password class is defined in Form1)

                Application.OpenForms["Form2"].Close();   //Closes the password change form (this form)
            }
        }

        public void CheckThisForm()  //Checking the correctness of filling out forms
        {
            if (textBox1.Text != textBox2.Text)
                MessageBox.Show("Паролі не співпадають!");
            else if (comboBox1.Text == "")
                MessageBox.Show("Оберіть тип підказки");
            else if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                MessageBox.Show("Всі поля повинні бути заповнені!");
            else
                isOk = true;
        }
    }
}