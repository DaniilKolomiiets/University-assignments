using System;
using System.IO;
using System.Windows.Forms;

namespace Employee
{
    public partial class Form1 : Form
    {
        Password password = new Password();

        public Form1()
        {
            InitializeComponent();

            label2.Visible = false;

            password.CheckPassword();
            password.GetDate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введіть пароль");
            }
            else if (textBox1.Text != password.GetPassword())  //Opens the form for changing the password
            {
                label2.Visible = true;
            }
            else   //If the password is correct, it opens the form of the main program
            {
                textBox1.Text = "";
                new Form4().ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ви точно хочете змінити пароль?", "Зміна пароля", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                new Form3().ShowDialog();   //Opens the form for changing the password
            }
        }
    }

    class Password
    {
        private string _password;

        public void CheckPassword()
        {
            if (!File.Exists("jvfd.txt"))   //If there is no password file, opens the registration form
            {
                new Form2().ShowDialog();
            }
            else                            //If the file exists but is empty, opens the registration form
            {
                using (StreamReader reader = new StreamReader("jvfd.txt"))
                {
                    string line = reader.ReadLine();

                    if (line == null) 
                    {
                        new Form2().ShowDialog();
                    }
                }
            }
        }

        public void SavePassword(Control comboboxOne, Control textboxThree, Control textboxOne)
        {
            using (StreamWriter writer = new StreamWriter("jvfd.txt"))
            {
                writer.WriteLine(comboboxOne.Text);   //Tooltip type
                writer.WriteLine(textboxThree.Text);  //Tooltip
                writer.WriteLine(textboxOne.Text);    //Password
            }
        }

        public void GetDate()    //Adds a password from a text file to the _password field
        {
            if (File.Exists("jvfd.txt"))
            {
                using (StreamReader reader = new StreamReader("jvfd.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        _password = reader.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("Помилка! Пароль не знайдено");
            }
        }

        public string GetPassword()
        {
            return _password;
        }
    }
}