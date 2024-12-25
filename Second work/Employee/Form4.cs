using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Employee
{
    public partial class Form4 : Form
    {
        List<Employee> employees = new List<Employee>(); //List for storing employees 
        Control[] addOrDelete = new Control[] { };   //Windows Forms elements for a worm with removing and adding an employee
        Control[] profile = new Control[] { };   //Windows Forms elements for an individual employee profile form
        Control[] stat = new Control[] { };   //Windows Forms elements for a form with employee statistics
        int whichMenu = 0;   //Shows which form is open to understand which elements to turn off when leaving the form

        public Form4()
        {
            InitializeComponent();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker3.Format = DateTimePickerFormat.Short;
            dateTimePicker4.Format = DateTimePickerFormat.Short;
            dateTimePicker5.Format = DateTimePickerFormat.Short;
            dateTimePicker6.Format = DateTimePickerFormat.Short;
            button6.Visible = false;
            comboBox3.Enabled = false;
            textBox7.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox9.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox12.ReadOnly = true;
            textBox13.ReadOnly = true;

            addOrDelete = new Control[] { label2, label3, label4, label5, label6, textBox2, textBox3, textBox4, textBox5, 
                dateTimePicker1, button8, button9, button10, groupBox1,
                comboBox2, comboBox3, panel1};

            profile = new Control[] { label7, label8, label9, label10, label11, label12, label13, label14, textBox6, textBox7, 
                textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, button11, button12,
            button13, comboBox4, dateTimePicker2, dateTimePicker3, dateTimePicker4, panel2};

            stat = new Control[] { panel3, dataGridView1, comboBox5, dateTimePicker5, dateTimePicker6, button14, label15, label16 };

            foreach (Control control in addOrDelete)
            {
                control.Visible = false;
            }

            foreach (Control control in profile)
            {
                control.Visible = false;
            }

            foreach (Control control in stat)
            {
                control.Visible = false;
            }

            if (!File.Exists("Employees.txt"))
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }
            else
            {
                using (StreamReader reader = new StreamReader("Employees.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        string[] array = line.Split(new char[] { '*' });

                        DateTime temp = DateTime.ParseExact(array[1], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        if (array.Length == 4)
                        {
                            employees.Add(new FullTimeEmployee(array[0], temp, array[2], array[3]));
                        }
                        else if (array.Length == 5)
                        {
                            employees.Add(new PartTimeEmployee(array[0], temp, array[2], array[3], double.Parse(array[4])));
                        }
                    }

                    UpdateTextBoxes();
                }

                foreach (var employee in employees)
                {
                    employee.LoadFromFile();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)   //Search for an employee
        {
            if (employees.Count == 0)
            {
                MessageBox.Show("Ви не додали жодного співробітника!");
            }
            else
            {
                foreach (Employee employee in employees)
                {
                    if (textBox1.Text.ToLower() == employee.GetName().ToLower())
                    {
                        comboBox1.SelectedItem = employee.GetName();
                        break;
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)     //Adding a new employee to the program
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заповніть всі поля!");
            }
            else if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Оберіть тип зайнятості!");
            }
            else if (radioButton2.Checked && comboBox3.Text == "")
            {
                MessageBox.Show("Оберіть робочі години на добу!");
            }
            else
            {
                if (radioButton1.Checked)
                {
                    using (StreamWriter writer = new StreamWriter("Employees.txt", true))
                    {
                        writer.WriteLine($"{textBox2.Text}*{dateTimePicker1.Text}*{textBox3.Text}*{textBox4.Text}");
                    }

                    employees.Add(new FullTimeEmployee(textBox2.Text, dateTimePicker1.Value, textBox3.Text, textBox4.Text));
                }
                else if (radioButton2.Checked)
                {
                    using (StreamWriter writer = new StreamWriter("Employees.txt", true))
                    {
                        writer.WriteLine($"{textBox2.Text}*{dateTimePicker1.Text}*{textBox3.Text}*{textBox4.Text}*{comboBox3.Text}");
                    }

                    employees.Add(new PartTimeEmployee(textBox2.Text, dateTimePicker1.Value, textBox3.Text, textBox4.Text, double.Parse(comboBox3.Text)));
                }

                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                MessageBox.Show("Ви додали співробітника");

                if (comboBox1.Visible == false && comboBox2.Visible == false)
                {
                    comboBox1.Visible = true;
                    comboBox2.Visible = true;
                }

                UpdateTextBoxes();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox3.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboBox3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button6.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;

            foreach (Control control in addOrDelete)
            {
                control.Visible = true;
            }

            UpdateTextBoxes();

            whichMenu = 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            switch (whichMenu)
            {
                case 1:
                    foreach (Control control in addOrDelete)
                    {
                        control.Visible = false;
                    }

                    UpdateTextBoxes();

                    if (employees.Count != 0)
                    {
                        comboBox1.Enabled = true;
                    }
                    else
                    {
                        comboBox1.Enabled = false;
                    }

                    break;
                case 2:
                    foreach (Control control in profile)
                    {
                        control.Visible = false;
                    }

                    break;
                case 3:
                    foreach (Control control in stat)
                    {
                        control.Visible = false;
                    }

                    break;
            }

            button6.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)   //Removing an employee from a file and from the List
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Ви не обрали співробітника!");
            }
            else
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    if (comboBox2.Text == employees[i].GetName())
                    {
                        if (File.Exists($"{employees[i].GetName()}.txt"))    //Deletes an employee's personal file with work data by date
                        {
                            File.Delete($"{employees[i].GetName()}.txt");
                        }

                        employees.RemoveAt(i);

                        MessageBox.Show("Ви видалили співробітника");

                        string[] lines = File.ReadAllLines("Employees.txt");

                        if (lines.Length >= i)   
                        {
                            var updatedLines = new List<string>();  //Create a list to save the lines except for the one that corresponds to the deleted employee

                            for (int j = 0; j < lines.Length; j++)
                            {
                                if (j != i)   //Missing a line with the deleted employee's index
                                {
                                    updatedLines.Add(lines[j]);
                                }
                            }

                            File.WriteAllLines("Employees.txt", updatedLines);     //Overwrite the file without this employee
                        }

                        break;
                    }
                }

                UpdateTextBoxes();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (employees.Count == 0)
            {
                MessageBox.Show("Ви не додали жодного співробітника!");
            }
            else
            {
                foreach (Employee employee in employees)
                {
                    if (textBox5.Text.ToLower() == employee.GetName().ToLower())
                    {
                        comboBox2.SelectedItem = employee.GetName();

                        break;
                    }
                }
            }
        }

        public void UpdateTextBoxes()  //Updates comboboxes with new information
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox4.Items.Clear();

            foreach (Employee employee in employees)
            {
                comboBox1.Items.Add(employee.GetName());
                comboBox2.Items.Add(employee.GetName());
                comboBox4.Items.Add(employee.GetName());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button6.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            comboBox4.Visible = true;
            textBox6.Visible = true;
            label7.Visible = true;
            comboBox4.Visible = true;
            panel2.Visible = true;
            button11.Visible = true;

            UpdateTextBoxes();

            whichMenu = 2;
        }

        private void button11_Click(object sender, EventArgs e)    //Search for an employee
        {
            if (employees.Count == 0)
            {
                MessageBox.Show("Ви не додали жодного співробітника!");
            }
            else
            {
                foreach (Employee employee in employees)
                {
                    if (textBox6.Text.ToLower() == employee.GetName().ToLower())
                    {
                        comboBox4.SelectedItem = employee.GetName();

                        break;
                    }
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text != "")
            {
                foreach (Control control in profile)
                {
                    control.Visible = true;
                }

                foreach (Employee employee in employees)
                {
                    if (comboBox4.Text.ToLower() == employee.GetName().ToLower())
                    {
                        textBox7.Text = employee.GetName();
                        textBox9.Text = employee.GetBirth();
                        textBox8.Text = employee.GetPosition();
                        textBox10.Text = employee.GetEmail();

                        if (employee is PartTimeEmployee)
                        {
                            PartTimeEmployee partTimeEmp = (PartTimeEmployee)employee;
                            textBox11.Text = partTimeEmp.GetWorkTime();
                        }

                        break;
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            foreach (Employee employee in employees)
            {
                if (comboBox4.Text.ToLower() == employee.GetName().ToLower())
                {
                    textBox12.Text = employee.GetWorkHoursByDate(dateTimePicker2.Value);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)     //Starts the working day of the selected employee
        {
            foreach (Employee employee in employees)
            {
                if (comboBox1.Text.ToLower() == employee.GetName().ToLower())
                {
                    if (employee.GetIsDayStart() == false)
                    {
                        employee.SetIsDayStart(true);
                        employee.SetStartTime(DateTime.Now);

                        MessageBox.Show("Робочий день почато!");
                    }
                    else if (employee.GetIsDayStart() == true)
                    {
                        MessageBox.Show("Робочий день цього співробітника ВЖЕ йде!");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)    //Ends the working day of the selected employee
        {
            foreach (Employee employee in employees)
            {
                if (comboBox1.Text.ToLower() == employee.GetName().ToLower() && employee.GetStartTime() != null)
                {
                    if (employee.GetIsDayStart() == true)
                    {
                        employee.SetIsDayStart(false);
                        employee.AddWorkLog(DateTime.Now);

                        MessageBox.Show("Робочий день завершено!");

                        employee.SaveToFile();
                        employee.LoadFromFile();
                    }
                    else if (employee.GetIsDayStart() == false)
                    {
                        MessageBox.Show("Ви ще не почали робочий день цього співробітника!");
                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)    //Visualization of statistics
        {
            foreach (Employee employee in employees)
            {
                if (comboBox4.Text.ToLower() == employee.GetName().ToLower())
                {
                    textBox13.Text = employee.GetWorkedHoursForPeriod(dateTimePicker3.Value, dateTimePicker4.Value);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button6.Visible = true;
            dateTimePicker5.Enabled = false;
            dateTimePicker6.Enabled = false;

            foreach (var atatik in stat)
            {
                atatik.Visible = true;
            }

            UpdateTextBoxes();

            whichMenu = 3;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox5.Text)
            {
                case "Відпрацьовіні години за оброну дату":
                    dateTimePicker5.Enabled = true;
                    dateTimePicker6.Enabled = false;
                    break;
                case "Відпрацьовіні години за оброий період":
                    dateTimePicker5.Enabled = true;
                    dateTimePicker6.Enabled = true;
                    break;
                case "Середній час роботи співробітників":
                    dateTimePicker5.Enabled = false;
                    dateTimePicker6.Enabled = false;
                    break;
            }
        }

        private void button14_Click(object sender, EventArgs e)   //Visualization of statistics
        {
            switch (comboBox5.Text)
            {
                case "Відпрацьовіні години за оброну дату":

                    dataGridView1.Rows.Clear();

                    foreach (var employee in employees)
                    {
                        string EmployeeType = "";

                        if (employee is PartTimeEmployee)
                        {
                            EmployeeType = "Неповний";
                        }
                        else if (employee is FullTimeEmployee)
                        {
                            EmployeeType = "Повний";
                        }

                        dataGridView1.Rows.Add(employee.GetName(), employee.GetPosition(), EmployeeType, employee.GetWorkHoursByDate(dateTimePicker5.Value.Date));
                    }

                    break;
                case "Відпрацьовіні години за оброий період":

                    dataGridView1.Rows.Clear();

                    foreach (var employee in employees)
                    {
                        string EmployeeType = "";

                        if (employee is PartTimeEmployee)
                        {
                            EmployeeType = "Неповний";
                        }
                        else if (employee is FullTimeEmployee)
                        {
                            EmployeeType = "Повний";
                        }

                        dataGridView1.Rows.Add(employee.GetName(), employee.GetPosition(), EmployeeType, 
                            employee.GetWorkedHoursForPeriod(dateTimePicker5.Value.Date, dateTimePicker6.Value.Date));
                    }

                    break;
                case "Середній час роботи співробітників":

                    dataGridView1.Rows.Clear();

                    foreach (var employee in employees)
                    {
                        string EmployeeType = "";

                        if (employee is PartTimeEmployee)
                        {
                            EmployeeType = "Неповний";
                        }
                        else if (employee is FullTimeEmployee)
                        {
                            EmployeeType = "Повний";
                        }

                        dataGridView1.Rows.Add(employee.GetName(), employee.GetPosition(), EmployeeType, employee.GetAverageWorkHoursPerDay() + " год. на день");
                    }

                    break;
            }
        }
    }

    class Employee
    {
        private string _name;
        private DateTime _dateOfBirth;
        private string _email;
        private string _position;
        private TimeTracking _workSchedule;
        private bool _isDayStart;   //To check whether an employee's working day has already started

        public Employee() { }

        public Employee(string name, DateTime dateOfBirth, string email, string position)
        {
            _name = name;
            _dateOfBirth = dateOfBirth;
            _email = email;
            _position = position;
            _isDayStart = false;
            _workSchedule = new TimeTracking();
        }

        public void SaveToFile()
        {
            _workSchedule.SaveToFile(_name);
        }

        public void LoadFromFile()
        {
            _workSchedule.LoadFromFile(_name);
        }

        public string GetWorkedHoursForPeriod(DateTime startDate, DateTime endDate)
        {
            TimeSpan time = _workSchedule.GetWorkedHoursForPeriod(startDate, endDate);

            return time.ToString();
        }

        public double GetAverageWorkHoursPerDay()
        {
            return _workSchedule.GetAverageWorkHoursPerDay();
        }

        public string GetName()
        {
            return _name;
        }

        public string GetPosition()
        {
            return _position;
        }

        public string GetEmail()
        {
            return _email;
        }

        public string GetBirth()
        {
            string birth = _dateOfBirth.ToString("dd-MM-yyyy");

            return birth;
        }

        public DateTime GetStartTime()
        {
            return _workSchedule.GetStartTime();
        }

        public string GetWorkHoursByDate(DateTime date)  //Returns hours worked for a specific date
        {
            TimeSpan? workedHours = _workSchedule.GetWorkedHoursByDate(date);

            if (workedHours == null)
            {
                return "Немає даних за обрану дату";
            }
            else
            {
                return workedHours.Value.ToString();
            }
        }

        public void AddWorkLog(DateTime time)
        {
            _workSchedule.AddWorkLog(time);
        }

        public void SetStartTime(DateTime startTime)
        {
            _workSchedule.SetStartTime(startTime);
        }

        public void SetEndTime(DateTime endTime)
        {
            _workSchedule.SetEndTime(endTime);
        }

        public bool GetIsDayStart()
        {
            return _isDayStart;
        }

        public void SetIsDayStart(bool _isDay)
        {
            _isDayStart = _isDay;
        }
    }

    class FullTimeEmployee : Employee   //Full-time employee
    {
        public FullTimeEmployee(string name, DateTime dateOfBirth, string email, string position)
            : base(name, dateOfBirth, email, position)
        {
        }
    }

    class PartTimeEmployee : Employee   //Part-time employee
    {
        private double _workTime;   //Hours of work specified for a part-time employee (individually)

        public PartTimeEmployee(string name, DateTime dateOfBirth, string email, string position, double worktime)
            : base(name, dateOfBirth, email, position)
        {
            _workTime = worktime;
        }

        public string GetWorkTime()
        {
            string workTime = Convert.ToString(_workTime);

            return workTime;
        }
    }

    class TimeTracking
    {
        private DateTime _startTime;     //Start time of the working day
        private DateTime _endTime;       //Time of the end of the working day
        private Dictionary<DateTime, TimeSpan> _workLog;   //Hours worked by date

        public TimeTracking()
        {
            _workLog = new Dictionary<DateTime, TimeSpan>();
        }

        public void SaveToFile(string nameOfEmployee)  //Saves information about hours worked for a certain date to a text file of an individual employee
        {
            string nameOfFile = nameOfEmployee + ".txt";

            using (StreamWriter writer = new StreamWriter(nameOfFile))
            {
                foreach (var entry in _workLog)
                {
                    writer.WriteLine($"{entry.Key.ToString("dd.MM.yyyy")}*{entry.Value}");
                }
            }
        }

        public void LoadFromFile(string nameOfEmployee)   //Adds information about hours worked for a certain date to the _workLog field, which is taken from a text file
        {
            string nameOfFile = nameOfEmployee + ".txt";

            if (File.Exists(nameOfFile))
            {
                _workLog.Clear();

                using (StreamReader reader = new StreamReader(nameOfFile))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('*');

                        if (parts.Length == 2)
                        {
                            DateTime date;

                            if (DateTime.TryParse(parts[0], out date))
                            {
                                TimeSpan time;

                                if (TimeSpan.TryParse(parts[1], out time))
                                {
                                    TimeSpan roundedTime = TimeSpan.FromSeconds(Math.Round(time.TotalSeconds));   //Rounding to whole seconds

                                    _workLog[date] = roundedTime;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AddWorkLog(DateTime date)  //Adds work hours for a specific date to the _workLog field
        {
            _endTime = date;

            TimeSpan workedHours = _endTime - _startTime;

            if (_workLog.ContainsKey(date))
            {
                _workLog[date] += workedHours;    //If there is already a record for this date, add it to the existing time
            }
            else
            {
                _workLog[date] = workedHours;     //If not, create a new record
            }
        }

        public TimeSpan GetWorkedHoursByDate(DateTime date)   //Returns the hours worked for the selected date
        {
            if (_workLog.TryGetValue(date.Date, out TimeSpan workedHours))
            {
                return workedHours;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public TimeSpan GetWorkedHoursForPeriod(DateTime startDate, DateTime endDate)   //Returns the total operating time for the period
        {
            TimeSpan totalWorkedHours = TimeSpan.Zero;

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))   //Iterate by dates in the specified range (inclusive)
            {
                totalWorkedHours += GetWorkedHoursByDate(date);
            }

            return totalWorkedHours;
        }

        public double GetAverageWorkHoursPerDay()  //Returns the average working time per day in hours
        {
            if (_workLog.Count == 0)
            {
                return 0;
            }

            TimeSpan totalWorkedHours = TimeSpan.Zero;

            foreach (var entry in _workLog)
            {
                totalWorkedHours += entry.Value;
            }

            double averageHours = totalWorkedHours.TotalHours / _workLog.Count;

            return Math.Round(averageHours, 1);
        }

        public DateTime GetStartTime()
        {
            return _startTime;
        }

        public DateTime GetEndTime()
        {
            return _endTime;
        }

        public void SetStartTime(DateTime startTime)
        {
            _startTime = startTime;
        }

        public void SetEndTime(DateTime endTime)
        {
            _endTime = endTime;
        }

        public TimeSpan GetWorkedHours()
        {
            return _endTime - _startTime;
        }
    }
}