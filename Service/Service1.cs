using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Service
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer tmr;
       
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        List<string> mailList = new List<string>();
      

        public Service1()
        {
            InitializeComponent();
           
        }

        protected override void OnStart(string[] args)
        {
            tmr = new System.Timers.Timer();
            tmr.Interval = (10000);
            tmr.Enabled = true;
            tmr.Start();
            tmr.Elapsed += new ElapsedEventHandler(tmr_elapsed);

        }

        private void tmr_elapsed(object sender, ElapsedEventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=PCName\\SQLEXPRESS;Initial Catalog=database_name;Integrated Security=True");
            conn.Open();
            cmd = new SqlCommand("Database query", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                mailList.Add(dr["bakim_mail"].ToString());
                dr.Dispose();

            }







            if (conn.State == ConnectionState.Open)
            {
                //Connection Test
                FileStream TS = new FileStream(@"C:\bs.txt", FileMode.OpenOrCreate);
                string lines = "Service has been started at" + DateTime.Now.ToString();
                System.IO.StreamWriter file = new System.IO.StreamWriter(TS);
                file.WriteLine(lines);
                file.Close();
                TS.Close();
            }
            else {
                //Connection Test
                FileStream TS = new FileStream(@"C:\bs.txt", FileMode.OpenOrCreate);
                string lines = "Noo" + DateTime.Now.ToString();
                System.IO.StreamWriter file = new System.IO.StreamWriter(TS);
                file.WriteLine(lines);
                file.Close();
                TS.Close();
            }
            conn.Close();
        }

        private void mailGonder(List<string> kime, string konu, string baslik)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("Your Mail");
            //
            foreach (string item in kime)
            {
                ePosta.To.Add(item);
            }
            //
            //
            ePosta.Subject = baslik;
            //
            ePosta.Body = konu;
            //
            SmtpClient smtp = new SmtpClient();
            //
            smtp.Credentials = new System.Net.NetworkCredential("Your Mail", "Your MailPassword");
            smtp.Port = 587;
            smtp.Host = "smtp.live.com";
            smtp.EnableSsl = true;
            object userState = ePosta;
            try
            {
                smtp.SendAsync(ePosta, (object)ePosta);
            }
            catch (SmtpException ex)
            {

            }

        }

    
        protected override void OnStop()
        {
            tmr.Enabled = false;

        }
    }
}
