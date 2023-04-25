using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Data.Odbc;


namespace Data_Base
{
    class Record

    {
        public int Id { get; set; }

        public String Client { get; set; }

        public String Treaty_nuber { get; set; }

        public String Contract_number { get; set; }

        public Record(int Id, String Client, String Treaty_nuber, String Contract_number)

        {
            this.Id = Id;

            this.Client = Client;

            this.Treaty_nuber = Treaty_nuber;

            this.Contract_number = Contract_number;
        }
    }

    public partial class MainWindow : Window
    {

        OdbcConnection conn;

        OdbcCommand cmd;

        OdbcDataAdapter adapter;

        DataTable dt;

        String ConnStr = "Driver={MySQL Odbc 5.1 Driver};" +
                          "Server=LocalHost;" +
                          "Port=3308" +
                          "Integrated Security=SSPI;" +
                          "DataBase=travel_agency;" +
                          "UId=root;" +
                          "PassWord=3;";

        String SelectText = "Select * From travel_agency ";
        String InsertText = "Insert Into travel_agency Values " +
             " ( ? , ? , ? , ? ) ";
        String UpdateText = "Update travel_agency Set " +
                  " Client = ? , " +
                  " Treaty_nuber = ? , " +
                  " Contract_number = ? " +
                  " Where Id = ? ";
        DataRow dr;
        String DeleteText = "Delete From travel_agency" +
                    " Where Id = ? ";

        List<Record> RecordList;

        int i,n;

        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void MSelect_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void MInsert_Click(object sender, RoutedEventArgs e)
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

            cmd.Parameters.Add("@Client", OdbcType.Char, 20, "Client");

            cmd.Parameters.Add("@Treaty_nuber", OdbcType.Char, 20, "Treaty_nuber");

            cmd.Parameters.Add("@Contract_number", OdbcType.Char, 20, "Contract_number");

            dr = dt.NewRow();

            dr[0] = Convert.ToInt32(EId.Text);

            dr[1] = EClient.Text;

            dr[2] = ETreaty_nuber.Text;

            dr[3] = EContract_number.Text;

            dt.Rows.Add(dr);

            adapter.InsertCommand = cmd;

            adapter.Update(dt);

            Refresh();

        }
        private void MUpdate_Click(object sender, RoutedEventArgs e)
        {
            int n;

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

                cmd.Parameters.Add("@Client", OdbcType.Char, 20, "Client");

                cmd.Parameters.Add("@Treaty_nuber", OdbcType.Char, 20, "Treaty_nuber");

                cmd.Parameters.Add("@Contract_number", OdbcType.Char, 20, "Contract_number");

                cmd.Parameters.Add("@Id", OdbcType.Int, 4, "Id");

                n = dg.SelectedIndex;

                dt.Rows[n][1] = EClient.Text;

                dt.Rows[n][2] = ETreaty_nuber.Text;

                dt.Rows[n][3] = EContract_number.Text;

                adapter.UpdateCommand = cmd;

                adapter.Update(dt);

                Refresh();

            }
        }

        private void MDelete_Click(object sender, RoutedEventArgs e)
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

        private void dg_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            n = dg.SelectedIndex;

            if (n == -1) return;

            EId.Text = Convert.ToString(dt.Rows[n][0]);

            EClient.Text = (string)dt.Rows[n][1];

            ETreaty_nuber.Text = (string)dt.Rows[n][2];

            EContract_number.Text = (string)dt.Rows[n][3];
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Data Base Петренко 02-1ИСП", "Textile_and_design_salon");
        }
    }
}
