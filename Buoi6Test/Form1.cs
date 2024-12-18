using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buoi6Test.Models;
using System.Data.Entity;


namespace Buoi6Test
{
    public partial class form1 : Form
    {
        private Model2 context;

        public form1()
        {
            InitializeComponent();
            context = new Model2();

        }
        private void LoadData()
        {
            Model2 db = new Model2();
            var students = db.Students.Select(s => new
            {
                s.StudentID,
                s.FullName,
                s.FacultyID,
                s.Faculty.FacultyName
            }).ToList();
            dataGridView1.DataSource = students;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadFacultyData();

        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu các trường không hợp lệ
            if (string.IsNullOrEmpty(txtMSSV.Text) || string.IsNullOrEmpty(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên.");
                return;
            }

            var student = new Student
            {
                StudentID = txtMSSV.Text,
                FullName = txtHoTen.Text,
                AverageScore = string.IsNullOrEmpty(txtDiemTB.Text) ? (double?)null : Convert.ToDouble(txtDiemTB.Text),
                FacultyID = Convert.ToInt32(cmbKhoa.SelectedValue) // Lấy giá trị FacultyID từ ComboBox
            };

            context.Students.Add(student);
            context.SaveChanges();

            dataGridView1.DataSource = context.Students.Include(s => s.Faculty).ToList();
            LoadData();
        }
        private void LoadFacultyData()
        {
            // Lấy danh sách các khoa và gán vào ComboBox
            cmbKhoa.DataSource = context.Faculties.ToList();
            cmbKhoa.DisplayMember = "FacultyName"; // Hiển thị tên khoa
            cmbKhoa.ValueMember = "FacultyID"; // Giá trị tương ứng với ID khoa
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var studentID = txtMSSV.Text;
            var student = context.Students.FirstOrDefault(s => s.StudentID == studentID);

            if (student != null)
            {
                student.FullName = txtHoTen.Text;
                student.AverageScore = string.IsNullOrEmpty(txtDiemTB.Text) ? (double?)null : Convert.ToDouble(txtDiemTB.Text);
                student.FacultyID = Convert.ToInt32(cmbKhoa.SelectedValue); // Cập nhật FacultyID từ ComboBox

                context.SaveChanges();

                dataGridView1.DataSource = context.Students.Include(s => s.Faculty).ToList();
                LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var studentID = txtMSSV.Text;
            var student = context.Students.FirstOrDefault(s => s.StudentID == studentID);

            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
             
                dataGridView1.DataSource = context.Students.Include(s => s.Faculty).ToList();
                LoadData();
            }
        }
    }
}
