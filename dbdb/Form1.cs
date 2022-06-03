using System.IO;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace dbdb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            using (db_contex db = new db_contex())
            {
                dataGridView1.DataSource = db.Hostels.ToList();
                if (db.Hostels.Any())
                {
                    numericUpDown2.Maximum = (decimal)db.Hostels.OrderByDescending(e => e.Id).FirstOrDefault().Id;
                    numericUpDown8.Maximum = numericUpDown2.Maximum;
                    numericUpDown4.Maximum = numericUpDown2.Maximum;
                    numericUpDown5.Maximum = numericUpDown2.Maximum;

                    identity_change();
                    comboBox1.DataSource = db.FirstName.ToList();
                    comboBox1.DisplayMember = "First";
                    comboBox2.DataSource = db.SecondName.ToList();
                    comboBox2.DisplayMember = "Second";
                    comboBox4.DataSource = db.FirstName.ToList();
                    comboBox4.DisplayMember = "First";
                    comboBox3.DataSource = db.SecondName.ToList();
                    comboBox3.DisplayMember = "Second";
                }
            }

        }
        private void Table_Restart(object sender, EventArgs e)
        {
            using (db_contex db = new db_contex())
            {
                db.Database.ExecuteSqlRaw("truncate table Hostel");
                db.SaveChanges();

                if (db.FirstName.OrderByDescending(x => x.Id).FirstOrDefault()?.Id == null)
                    AddFirstName();
                if (db.SecondName.OrderByDescending(x => x.Id).FirstOrDefault()?.Id == null)
                    AddSecondName();

                FillLibraryT();

                dataGridView1.DataSource = db.Hostels.ToList();
                numericUpDown2.Maximum = (decimal)db.Hostels.OrderByDescending(e => e.Id).FirstOrDefault().Id;
                numericUpDown8.Maximum = numericUpDown2.Maximum;
                numericUpDown4.Maximum = numericUpDown2.Maximum;
                numericUpDown5.Maximum = numericUpDown2.Maximum;

                identity_change();
                comboBox1.DataSource = db.FirstName.ToList();
                comboBox1.DisplayMember = "FirstName";
                comboBox2.DataSource = db.SecondName.ToList();
                comboBox2.DisplayMember = "SecondName";
                comboBox4.DataSource = db.FirstName.ToList();
                comboBox4.DisplayMember = "FirstName";
                comboBox3.DataSource = db.SecondName.ToList();
                comboBox3.DisplayMember = "SecondName";
            }
            MessageBox.Show("Новая таблица сгенерирована");
        }
        private void Add_Click(object sender, EventArgs e)
        {
            using (db_contex db = new db_contex())
            {
                Hostel HOST = new Hostel
                {
                    FirstName = comboBox1.Text,
                    SecondName = comboBox2.Text,
                    RoomNumber = (int?)numericUpDown1.Value,
                    RoomType = textBox1.Text,
                    DateRoomOccupied = dateTimePicker2.Value,
                    DateRoomFree = dateTimePicker3.Value,
                };
                db.Add<Hostel>(HOST);
                db.SaveChanges();
                dataGridView1.DataSource = db.Hostels.ToList();
                numericUpDown2.Maximum = (decimal)db.Hostels.OrderByDescending(e => e.Id).FirstOrDefault().Id;
                numericUpDown8.Maximum = numericUpDown2.Maximum;
                numericUpDown4.Maximum = numericUpDown2.Maximum;
                numericUpDown5.Maximum = numericUpDown2.Maximum;
            }
            MessageBox.Show("Запись добавлена");
            dataGridView1.Update();
        }
        private void Change_Click(object sender, EventArgs e)
        {
            using (db_contex db = new db_contex())
            {
                Hostel HOST = db.Hostels.Where(e => e.Id == numericUpDown2.Value).FirstOrDefault();
                if (HOST != null)
                {
                    HOST.FirstName = textBox2.Text;
                    HOST.SecondName = textBox3.Text;
                    HOST.RoomNumber = (int)numericUpDown3.Value;
                    HOST.RoomType = textBox4.Text;
                    HOST.DateRoomOccupied = dateTimePicker1.Value;
                    HOST.DateRoomFree = dateTimePicker4.Value;

                    db.SaveChanges();
                    dataGridView1.DataSource = db.Hostels.ToList();
                }
                else
                {
                    MessageBox.Show("Запись не удалось изменить");
                    return;
                }
            }
            MessageBox.Show("Запись изменена");
            dataGridView1.Update();
        }
        private void identity_change(object sender = null, EventArgs e = null)
        {
            using (db_contex db = new db_contex())
            {
                Hostel HOST = db.Hostels.Where(e => e.Id == numericUpDown2.Value).FirstOrDefault();
                if (HOST != null)
                {
                    textBox2.Text = HOST.FirstName;
                    textBox3.Text = HOST.SecondName;
                    numericUpDown3.Value = (int)HOST.RoomNumber;
                    textBox4.Text = HOST.RoomType;
                    dateTimePicker1.Value = (DateTime)HOST.DateRoomOccupied;
                    dateTimePicker4.Value = (DateTime)HOST.DateRoomFree;
                }
            }
        }
        private void Deleting_Click(object sender, EventArgs e)
        {
            using (db_contex db = new db_contex())
            {
                var HOST = db.Hostels.Find((int)numericUpDown8.Value);

                if (HOST != null)
                {
                    db.Hostels.Remove(HOST);
                    db.SaveChanges();
                    dataGridView1.DataSource = db.Hostels.ToList();
                }
                else
                {
                    MessageBox.Show("Данной записи не существует");
                    return;
                }
            }
            MessageBox.Show("Запись удалена");
            dataGridView1.Update();
        }
        private void Filter_Click(object sender, EventArgs e)
        {
            using (db_contex db = new db_contex())
            {
                System.Linq.IQueryable<dbdb.Hostel> HOST = db.Hostels;

                if (numericUpDown4.Value != 0)
                    if (numericUpDown5.Value != 0 && numericUpDown5.Value >= numericUpDown4.Value)
                        HOST = HOST.Where(e => e.Id >= numericUpDown4.Value && e.Id <= numericUpDown5.Value);
                    else
                    {
                        MessageBox.Show("Максимальное значение id задано неверно");
                        return;
                    }
                if (comboBox4.Text != "")
                    HOST = HOST.Where(e => e.FirstName.Contains(comboBox4.Text));
                if (comboBox3.Text != "")
                    HOST = HOST.Where(e => e.SecondName.Contains(comboBox3.Text));
                if (textBox5.Text != "")
                    HOST = HOST.Where(e => e.RoomType.Contains(textBox5.Text));
                if (numericUpDown6.Value != 0)
                    if (numericUpDown7.Value != 0 && numericUpDown7.Value >= numericUpDown6.Value)
                        HOST = HOST.Where(e => e.RoomNumber >= numericUpDown6.Value && e.RoomNumber <= numericUpDown7.Value);
                    else
                    {
                        MessageBox.Show("Максимальное значение страниц задано неверно");
                        return;
                    }
                if (HOST != null)
                    dataGridView1.DataSource = HOST.ToList();
                else
                {
                    MessageBox.Show("Параметры фильтрации не заданы");
                    return;
                }
            }
            dataGridView1.Update();
            MessageBox.Show("Записи отфильтрованы");
        }
        internal void AddFirstName()
        {
            using (db_contex db = new db_contex())
            {
                using (StreamReader reader = File.OpenText(@"..\Names.txt"))
                {
                    string line = reader.ReadLine();
                    string[] split;

                    while ((line = reader.ReadLine()) != null)
                    {
                        split = line.Split(';');

                        FirstName fn = new FirstName { First = split[1], Gender = CheckGender(split[2]) };

                        db.Add<FirstName>(fn);
                    }
                }
                db.SaveChanges();
            }
        }
        internal void AddSecondName()
        {
            using (db_contex db = new db_contex())
            {
                using (StreamReader reader = File.OpenText(@"..\Surnames.txt"))
                {
                    string line = reader.ReadLine();
                    string[] split;

                    while ((line = reader.ReadLine()) != null)
                    {
                        split = line.Split(';');

                        SecondName sn = new SecondName { Second = split[1], Gender = CheckGender(split[2]) };

                        db.Add<SecondName>(sn);
                    }
                }
                db.SaveChanges();
            }
        }
        internal bool CheckGender(string gender)
        {
            if (gender == "М")
                return true;
            else
                return false;
        }
        internal void FillLibraryT()
        {
            var rand = new Random();
            int n;

            using (db_contex db = new db_contex())
            {
                int k = 1;
                while (k <= 500)
                {
                    n = rand.Next(1, db.FirstName.OrderByDescending(x => x.Id).FirstOrDefault().Id);
                    string firstname = db.FirstName.Where(e => e.Id == n).FirstOrDefault().First;

                    var gender = db.FirstName.Where(e => e.Id == n).FirstOrDefault().Gender;

                    n = rand.Next(1, db.SecondName.OrderByDescending(x => x.Id).FirstOrDefault().Id);

                    while (gender != db.SecondName.Where(e => e.Id == n).FirstOrDefault().Gender)
                        n = rand.Next(1, db.SecondName.OrderByDescending(x => x.Id).FirstOrDefault().Id);

                    string secondname = db.SecondName.Where(e => e.Id == n).FirstOrDefault().Second;

                    string roomtype = TakeRoomType();

                    var daterand = new RandomDateTime();

                    DateTime roomDateOccupide = new DateTime();
                    roomDateOccupide = daterand.Next();

                    DateTime roomDateFree = new DateTime();
                    while(roomDateOccupide > roomDateFree)
                        roomDateFree = daterand.Next();

                    Hostel l = new Hostel
                    {
                        FirstName = firstname,
                        SecondName = secondname,
                        RoomType = roomtype,
                        RoomNumber = rand.Next(1,505),
                        DateRoomOccupied = roomDateOccupide,
                        DateRoomFree = roomDateFree
                    };
                    db.Hostels.Add(l);
                    k++;

                }
                db.SaveChanges();
            }
        }
        static string TakeRoomType()
        {
            string roomtype = "roomtype";
            var random = new Random();
            int n;


            using (StreamReader reader = File.OpenText(@"..\RoomType.txt"))
            {
                n = random.Next(1, 7);
                string line = null;
                string[] split;
                while ((line = reader.ReadLine()) != null && roomtype == "roomtype")
                {
                    split = line.Split(';');
                    if (split[0] == n.ToString())
                    {
                        roomtype = split[1];
                    }
                }
                reader.Close();
            }
            return roomtype;
        }
    }
}