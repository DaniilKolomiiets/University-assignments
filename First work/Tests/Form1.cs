using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Tests
{
    public partial class Form1 : Form
    {
        //Arrays containing the objects needed for the menu and the objects needed for testing
        Control[] _mainMenu;
        Control[] _test;
        Test test = new Test();
        string[,] answers;
        Dictionary<string, char> questionAndCorrectAnswer = new Dictionary<string, char>();

        public Form1()
        {
            InitializeComponent();

            //Adding Windows Forms objects to the appropriate arrays
            _test = new Control[] { button1, button2, button3, textBox1, groupBox1, comboBox1, label1, label3 };
            _mainMenu = new Control[] { button4, button5, button6, button7, label2 };

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            textBox1.ReadOnly = true;
            button8.Visible = false;

            foreach (Control obj in _test)
            {
                obj.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            foreach (Control obj in _mainMenu)
            {
                obj.Visible = false;
            }

            foreach (Control obj in _test)
            {
                obj.Visible = true;
            }

            switch (button.Text)
            {
                case "Математика":
                    test.TestRequired("mathAnswers.txt", "mathRightAnswersAndQuestions.txt");
                    break;
                case "Фізика":
                    test.TestRequired("physicsAnswers.txt", "physicsRightAnswersAndQuestions.txt");
                    break;
                case "Хімія":
                    test.TestRequired("chemistryAnswers.txt", "chemistryRightAnswersAndQuestions.txt");
                    break;
                case "Історія України":
                    test.TestRequired("historyAnswers.txt", "historyRightAnswersAndQuestions.txt");
                    break;
            }

            answers = test.GetAnswers();
            questionAndCorrectAnswer = test.GetQuestionsAndAnswers();

            for (int i = 0; i < test.GetQuestionsAndAnswers().Count; i++)
            {
                int item = i + 1;
                comboBox1.Items.Add(item.ToString());
            }

            comboBox1.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button8.Visible = true;
            textBox1.Text = "";

            textBox1.Text = $"Ви відповіли правильно у {test.GetScore()} завданнях";
        }

        public void ChangeText()
        {
            label1.Text = $"Завдання {comboBox1.SelectedIndex + 1} з {comboBox1.Items.Count}";
            radioButton1.Text = "A";
            radioButton2.Text = "Б";
            radioButton3.Text = "В";
            radioButton4.Text = "Г";
            textBox1.Text = "";

            radioButton1.Text += answers[comboBox1.SelectedIndex, 0];
            radioButton2.Text += answers[comboBox1.SelectedIndex, 1];
            radioButton3.Text += answers[comboBox1.SelectedIndex, 2];
            radioButton4.Text += answers[comboBox1.SelectedIndex, 3];

            textBox1.Text = questionAndCorrectAnswer.Keys.ElementAt(comboBox1.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp = "";

            if (radioButton1.Checked)
            {
                temp = radioButton1.Text;
                AddScoreAndChangeComboboxIndex(temp);
            }
            else if (radioButton2.Checked)
            {
                temp = radioButton2.Text;
                AddScoreAndChangeComboboxIndex(temp);
            }
            else if (radioButton3.Checked)
            {
                temp = radioButton3.Text;
                AddScoreAndChangeComboboxIndex(temp);
            }
            else if (radioButton4.Checked)
            {
                temp = radioButton4.Text;
                AddScoreAndChangeComboboxIndex(temp);
            }
            else
            {
                MessageBox.Show("Ви не позначили відповідь");
            }
        }

        public void AddScoreAndChangeComboboxIndex(string temp)
        {
            test.Score(comboBox1.SelectedIndex, temp[0]);

            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                button3.PerformClick();
            else
                comboBox1.SelectedIndex++;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeText();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Visible = false;

            foreach (Control obj in _test)
            {
                obj.Visible = false;
            }

            foreach (Control obj in _mainMenu)
            {
                obj.Visible = true;
            }

            test.Clear();
            comboBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            char temp = '0';

            test.Score(comboBox1.SelectedIndex, temp);

            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                button3.PerformClick();
            else
                comboBox1.SelectedIndex++;
        }
    }


    class Test
    {
        //Task number and whether the user answered correctly
        private Dictionary<int, bool> _isAnswertrue = new Dictionary<int, bool>();
        //Task and correct answer a b c or d
        private Dictionary<string, char> _questionAndCorrectAnswer = new Dictionary<string, char>();
        //An array containing all 4 answers to each of the 30 questions in the test
        private string[,] _answers = new string[30, 4];

        public void TestRequired(string answers, string rightAnswersAndQuestions)
        {
            //Add all the answers
            using (StreamReader reader = new StreamReader(answers))
            {
                while (!reader.EndOfStream)
                {
                    for (int i = 0; i < _answers.GetLength(0); i++)
                    {
                        for (int j = 0; j < _answers.GetLength(1); j++)
                        {
                            string line = reader.ReadLine();
                            _answers[i, j] = $") {line}";
                        }
                    }
                }
            }

            //Add a question and the correct answer that is linked to it 
            using (StreamReader reader = new StreamReader(rightAnswersAndQuestions))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split('$');
                    string question = parts[0].Trim();
                    char answer = Convert.ToChar(parts[1].Trim());

                    _questionAndCorrectAnswer.Add(question, answer);
                }
            }
        }

        public void Score(int index, char answer)
        {
            if (_isAnswertrue.ContainsKey(index))
            {
                _isAnswertrue.Remove(index);
            }

            if (answer == _questionAndCorrectAnswer.Values.ElementAt(index))
            {
                _isAnswertrue.Add(index, true);
            }
            else
            {
                _isAnswertrue.Add(index, false);
            }
        }

        public string[,] GetAnswers()
        {
            return _answers;
        }

        public Dictionary<string, char> GetQuestionsAndAnswers()
        {
            return _questionAndCorrectAnswer;
        }

        public int GetScore()
        {
            int score = 0;

            foreach (var a in _isAnswertrue.Values)
            {
                if (a == true)
                {
                    score++;
                }
            }

            return score;
        }

        public void Clear()
        {
            _isAnswertrue.Clear();
            _questionAndCorrectAnswer.Clear();

            for (int i = 0; i < _answers.GetLength(0); i++)
            {
                for (int j = 0; j < _answers.GetLength(1); j++)
                {
                    _answers[i, j] = "";
                }
            }
        }
    }
}