using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Windows.Markup;

namespace MySQL
{

    class Record

    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Dated { get; set; }

        public String Treaty_number { get; set; }
        public Record(int Id, String Name, String Dated, String Treaty_number)

        {
            this.Id = Id;

            this.Name = Name;

            this.Dated = Dated;

            this.Treaty_number = Treaty_number;
        }
    }

    /*================================================================================*/

    public partial class MainWindow : Window
    {
        OdbcConnection conn;

        OdbcCommand cmd;

        OdbcDataAdapter adapter;

        DataTable dt;

        DataRow dr;

        String ConnStr = "Driver={MySQL Odbc 5.1 Driver};" +
                          "Server=LocalHost;" +
                          "Port=3308;" +
                          "Integrated Security=SSPI;" +
                          "DataBase=petrenko;" +
                          "UId=root;" +
                          "PassWord=3;";

        String SelectText = "Select * From travel_agency";

        String InsertText = "Insert Into travel_agency Values " +
                             " ( ? , ? , ? , ? ) ";

        String UpdateText = "Update travel_agency Set " +
                             "Name = ? , " +
                             "Dated = ? , " +
                             "Treaty_number = ? " +
                             "Where Id = ? ";

        String DeleteText = "Delete From travel_agency " +
                             "Where Id = ? ";

        List<Record> RecordList;

        int i, n;

        /*================================================================================*/

        public MainWindow()
        {
            InitializeComponent();
        }

        /*================================================================================*/

        void Refresh()

        {


                dg.ItemsSource = null;

                conn = new OdbcConnection();

                conn.ConnectionString = ConnStr;

                cmd = new OdbcCommand();

                cmd.Connection = conn;

                cmd.CommandText = SelectText;

                adapter = new OdbcDataAdapter();

                adapter.SelectCommand = cmd;

                dt = new DataTable();

                RecordList = new List<Record>();

                adapter.Fill(dt);

                for (i = 0; i <= dt.Rows.Count - 1; i++)

                    RecordList.Add(new Record((int)dt.Rows[i][0],
                                                (string)dt.Rows[i][1],
                                                (string)dt.Rows[i][2],
                                                (string)dt.Rows[i][3]
                                              )
                                  );

                dg.ItemsSource = RecordList;


            RecordList = null;

            GC.Collect();
        }

        /*================================================================================*/

        private void miSelect_Click(object sender, RoutedEventArgs e)

        {
            Refresh();
        }

        /*================================================================================*/

        private void miInsert_Click(object sender, RoutedEventArgs e)

        {
            conn = new OdbcConnection();

            conn.ConnectionString = ConnStr;

            cmd = new OdbcCommand();

            cmd.Connection = conn;

            cmd.CommandText = SelectText;

            adapter = new OdbcDataAdapter();

            adapter.SelectCommand = cmd;

            dt = new DataTable();

            adapter.Fill(dt);

            cmd.CommandText = InsertText;

            cmd.Parameters.Add("@Id", OdbcType.Int, 4, "Id");

            cmd.Parameters.Add("@Name", OdbcType.Char, 20, "Name");

            cmd.Parameters.Add("@Dated", OdbcType.Char, 20, "Dated");

            cmd.Parameters.Add("@Treaty_number", OdbcType.Char, 20, "Treaty_number");

            dr = dt.NewRow();

            dr[0] = Convert.ToInt32(EId.Text);

            dr[1] = EName.Text;

            dr[2] = EDated.Text;

            dr[3] = ETreaty_number.Text;

            dt.Rows.Add(dr);

            adapter.InsertCommand = cmd;

            adapter.Update(dt);

            Refresh();
        }

        /*================================================================================*/

        private void miDelete_Click(object sender, RoutedEventArgs e)

        {
            conn = new OdbcConnection();

            conn.ConnectionString = ConnStr;

            cmd = new OdbcCommand();

            cmd.Connection = conn;

            cmd.CommandText = SelectText;

            adapter = new OdbcDataAdapter();

            adapter.SelectCommand = cmd;

            dt = new DataTable();

            adapter.Fill(dt);

            cmd.CommandText = DeleteText;

            cmd.Parameters.Add("@Id", OdbcType.Int, 4, "Id");

            n = dg.SelectedIndex;

            dt.Rows[n].Delete();

            adapter.DeleteCommand = cmd;

            adapter.Update(dt);

            Refresh();
        }

        /*================================================================================*/

        private void miUpdate_Click(object sender, RoutedEventArgs e)

        {
            conn = new OdbcConnection();

            conn.ConnectionString = ConnStr;

            cmd = new OdbcCommand();

            cmd.Connection = conn;

            cmd.CommandText = SelectText;

            adapter = new OdbcDataAdapter();

            adapter.SelectCommand = cmd;

            dt = new DataTable();

            adapter.Fill(dt);

            cmd.CommandText = UpdateText;

            cmd.Parameters.Add("@Name", OdbcType.VarChar, 20, "Name");

            cmd.Parameters.Add("@Dated", OdbcType.VarChar, 20, "Dated");

            cmd.Parameters.Add("@Treaty_number", OdbcType.VarChar, 20, "Treaty_number");

            cmd.Parameters.Add("@Id", OdbcType.Int, 4, "Id");

            n = dg.SelectedIndex;

            dt.Rows[n][1] = EName.Text;

            dt.Rows[n][2] = EDated.Text;

            dt.Rows[n][3] = ETreaty_number.Text;

            adapter.UpdateCommand = cmd;

            adapter.Update(dt);

            Refresh();
        }
        /*================================================================================*/
        private void miInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Работу выполнил Петренко Артём, группа 02-1ИСП", "Туристическая фирма");
        }

        /*================================================================================*/

        private void dg_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)

        {
            n = dg.SelectedIndex;

            if (n == -1) return;

            EId.Text = Convert.ToString(dt.Rows[n][0]);

            EName.Text = (string)dt.Rows[n][1];

            EDated.Text = (string)dt.Rows[n][2];

            ETreaty_number.Text = (string)dt.Rows[n][3];
        }
    }
}