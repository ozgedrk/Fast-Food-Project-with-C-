using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Fast_Food
{
    public partial class Subeler : Form
    {
        Form form;
        string musteriID;
        string personelID;
        DataSet dataSetSubeler = new DataSet();
        DataSet dataSetAdresler = new DataSet();
        DataSet dataSetSehirler = new DataSet();
        DataSet dataSetIlceler = new DataSet();


        public Subeler()
        {
            InitializeComponent();

        }

        public Subeler(string personelID, string musteriID, Form form)
            : this()
        {
            this.form = form;
            this.personelID = personelID;
            this.musteriID = musteriID;
            try
            {
                if (Database.dbConnection().State == ConnectionState.Closed)
                {
                    Database.dbConnection().Open();
                }
                string Ad = "", Soyad = "";
                if (personelID != null && !personelID.Equals(""))
                {
                    Ad = Database.dbCommand(
               "select ad from personel where personel_id=" + personelID
               ).ExecuteScalar().ToString();
                    Soyad = Database.dbCommand(
                   "select soyad from personel where personel_id=" + personelID
                   ).ExecuteScalar().ToString();
                }

                if (Database.dbConnection().State == ConnectionState.Open)
                {
                    Database.dbConnection().Close();
                }
                labelSubeAd.Text = Ad;
                labelSubeSoyad.Text = Soyad;
                labelSubeID.Text = musteriID;
                this.tabloyaVeriDoldur();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
        }

        public void tabloyaVeriDoldur()
        {
            dataSetSubeler = new DataSet();
            dataSetAdresler = new DataSet();
            dataSetSehirler = new DataSet();
            dataSetIlceler = new DataSet();


            Database.dbAdapter("SELECT * FROM sube ORDER BY sube_id ASC").Fill(dataSetSubeler);
            dataGridViewSubeler.DataSource = dataSetSubeler.Tables[0];
            Database.dbAdapter("SELECT * FROM adres_view ORDER BY adres_id ASC").Fill(dataSetAdresler);
            dataGridViewAdresler.DataSource = dataSetAdresler.Tables[0];
            Database.dbAdapter("SELECT * FROM sehirler ORDER BY plaka ASC").Fill(dataSetSehirler);
            dataGridViewSehirler.DataSource = dataSetSehirler.Tables[0];
            Database.dbAdapter("SELECT * FROM ilceler ORDER BY ilce_id ASC").Fill(dataSetIlceler);
            dataGridViewIlceler.DataSource = dataSetIlceler.Tables[0];



        }

        private void buttonGeri_Click(object sender, EventArgs e)
        {
            form.Show();
            this.Close();
        }

        private void dataGridViewSubeler_Click(object sender, EventArgs e)
        {
            adresDoldur();

            string adresID = dataGridViewSubeler.CurrentRow.Cells[3].Value.ToString();
            adresID = adresID.Trim();
            adresID = adresID.Replace("ID:", "");
            adresID = adresID.Trim();
            if (!adresID.Equals(""))
            {
                if (dataGridViewAdresler.CurrentRow.Cells[5].Value != null)
                {
                    string sehirID = dataGridViewAdresler.CurrentRow.Cells[5].Value.ToString();
                    sehirID = sehirID.Trim();
                    sehirID = sehirID.Substring(sehirID.LastIndexOf("ID:"));
                    sehirID = sehirID.Replace("ID:", "").Trim();
                    this.rowSelect(dataGridViewSehirler, sehirID);
                    this.dataGridViewSehirler_Click(dataGridViewSehirler, new EventArgs());
                }
                if (dataGridViewAdresler.CurrentRow.Cells[6].Value != null)
                {
                    string ilceID = dataGridViewAdresler.CurrentRow.Cells[6].Value.ToString();
                    ilceID = ilceID.Trim();
                    ilceID = ilceID.Substring(ilceID.LastIndexOf("ID:")); ;
                    ilceID = ilceID.Replace("ID:", "").Trim();
                    this.rowSelect(dataGridViewIlceler, ilceID);
                }
                
                
                
                
            }

            this.rowSelect(dataGridViewAdresler, adresID);

        }

        public void rowSelect(DataGridView dataGrid, string searchValue)
        {
            if (!searchValue.Equals(""))
            {
                dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                for (int i = 0; i < dataGrid.RowCount - 1; i++)
                {
                    DataGridViewRow row = dataGrid.Rows[i];
                    if (row.Cells[0].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }
        }

        public void adresDoldur()
        {
            string adresid = dataGridViewSubeler.CurrentRow.Cells[3].Value.ToString();
            dataSetAdresler = new DataSet();
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            adresid = adresid.Trim();
            adresid = adresid.Replace("ID:", "");
            adresid = adresid.Trim();
            if (!adresid.Equals(""))
            {
                Database.dbAdapter("SELECT * FROM adres_view where adres_id = " + adresid
                ).Fill(dataSetAdresler);
                dataGridViewAdresler.DataSource = dataSetAdresler.Tables[0];
            }
            else
            {
                Database.dbAdapter("SELECT * FROM adres_view where adres_id = 0"
                ).Fill(dataSetAdresler);
                dataGridViewAdresler.DataSource = dataSetAdresler.Tables[0];
            }

            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
        }

        public void reload(string musteriID)
        {
            this.tabloyaVeriDoldur();
        }


        public void deleteTable(DataGridView dataGrid, string tabloAdi, string nesne = "Tablodan")
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string idKolonu = dataGrid.CurrentRow.Cells[0].OwningColumn.HeaderText.ToString();
            string id = dataGrid.CurrentRow.Cells[0].Value.ToString();
            string sql = "DELETE FROM " + tabloAdi + " where " + idKolonu + " =  " + id;


            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show(nesne + " Başarıyla Silindi");
            }
            catch (NpgsqlException ex)
            {
                if (ex.ToString().Contains("23503"))
                {
                    
                        MessageBox.Show("Veritabanı Hatası: " + ex.ToString() + " " + sql);
                }
                else
                {
                    MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
                }
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(musteriID);
        }

        private void buttonSubeEkle_Click(object sender, EventArgs e)
        {
            this.adresInsert();
            this.musteriInsert();
            this.reload(musteriID);
        }

        public void musteriInsert()
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string sube_ad = dataGridViewSubeler.Rows[dataGridViewSubeler.NewRowIndex - 1].Cells[1].Value.ToString();
            string telefon = dataGridViewSubeler.Rows[dataGridViewSubeler.NewRowIndex - 1].Cells[2].Value.ToString();


            string adres = Database.dbCommand(
                "select MAX(adres_id) from adres").ExecuteScalar().ToString();
            //ŞUBE INSERT
            string sql = "INSERT INTO sube ";
            string columns = "sube_ad, telefon, adres";
            string values = "'" + sube_ad + "','" + telefon + "'," + adres;
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Şube Başarıyla Oluşturuldu");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString() + " " + sql);
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }

        }

        public void subeUpdate()
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string subeid = dataGridViewSubeler.CurrentRow.Cells[0].Value.ToString();
            string sube_ad = dataGridViewSubeler.CurrentRow.Cells[1].Value.ToString();
            string telefon = dataGridViewSubeler.CurrentRow.Cells[2].Value.ToString();
            //ŞUBE UPDATE
            string sql = "UPDATE sube SET ";
            string values = " sube_ad='" + sube_ad + "',telefon='" + telefon + "'";
            sql += values + " where sube_id=" + subeid;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Şube Başarıyla Güncellendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString() + " " + sql);
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(musteriID);

        }

        public void adresInsert()
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string mahalle = dataGridViewAdresler.CurrentRow.Cells[1].Value.ToString();
            string sokak = dataGridViewAdresler.CurrentRow.Cells[2].Value.ToString();
            string apartman_no = dataGridViewAdresler.CurrentRow.Cells[3].Value.ToString();
            string daire_no = dataGridViewAdresler.CurrentRow.Cells[4].Value.ToString();
            string sehir = dataGridViewSehirler.CurrentRow.Cells[0].Value.ToString();
            string ilce = dataGridViewIlceler.CurrentRow.Cells[0].Value.ToString();
            //ADRES INSERT
            string sql = "INSERT INTO adres ";
            string columns = "mahalle, sokak, apartman_no, daire_no, sehir, ilce";
            string values = "'" + mahalle + "','" + sokak + "', '" + apartman_no + "', '" + daire_no + "'," + sehir + "," + ilce;
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Adres Başarıyla Eklendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }

        }

        private void dataGridViewSehirler_Click(object sender, EventArgs e)
        {
            dataSetIlceler = new DataSet();
            string sehirID = dataGridViewSehirler.CurrentRow.Cells[0].Value.ToString();
            Database.dbAdapter("SELECT * FROM ilceler WHERE sehir=" + sehirID + " ORDER BY ilce_id ASC").Fill(dataSetIlceler);
            dataGridViewIlceler.DataSource = dataSetIlceler.Tables[0];
        }

        private void buttonSubeGuncelle_Click(object sender, EventArgs e)
        {
            this.adresUpdate();
            this.subeUpdate();
            this.reload(musteriID);
        }

        public void adresUpdate()
        {
            string adresid = dataGridViewSubeler.CurrentRow.Cells[3].Value.ToString();
            adresid = adresid.Trim().Replace("ID:", "").Trim();
            if (!adresid.Equals(""))
            {
                if (Database.dbConnection().State == ConnectionState.Closed)
                {
                    Database.dbConnection().Open();
                }
                adresid = dataGridViewAdresler.CurrentRow.Cells[0].Value.ToString();
                string mahalle = dataGridViewAdresler.CurrentRow.Cells[1].Value.ToString();
                string sokak = dataGridViewAdresler.CurrentRow.Cells[2].Value.ToString();
                string apartman_no = dataGridViewAdresler.CurrentRow.Cells[3].Value.ToString();
                string daire_no = dataGridViewAdresler.CurrentRow.Cells[4].Value.ToString();
                string sehir = dataGridViewSehirler.CurrentRow.Cells[0].Value.ToString();
                string ilce = dataGridViewIlceler.CurrentRow.Cells[0].Value.ToString();
                //ADRES UPDATE
                string sql = "UPDATE adres SET";
                string values = " mahalle='" + mahalle + "', sokak='" + sokak + "', apartman_no='" + apartman_no + "', daire_no='" + daire_no +
                    "', sehir=" + sehir + ", ilce=" + ilce;
                sql += values + " where adres_id=" + adresid;

                try
                {
                    Database.dbCommand(sql).ExecuteNonQuery();
                    MessageBox.Show("Adres Başarıyla Güncellendi");
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
                }
                if (Database.dbConnection().State == ConnectionState.Open)
                {
                    Database.dbConnection().Close();
                }
            }
            else
            {
                adresInsert();
            }


        }

        private void buttonSubeSil_Click(object sender, EventArgs e)
        {
            try
            {
                this.deleteTable(dataGridViewSubeler, "sube", "Şube");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Şubeye Ait Siparişler Var Silinemez");
            }

        }

        private void buttonSeciliAdresiGuncelle_Click(object sender, EventArgs e)
        {
            this.adresUpdate();
            this.reload(musteriID);
        }

        private void buttonSeciliAdresiSil_Click(object sender, EventArgs e)
        {
            try
            {
                this.deleteTable(dataGridViewAdresler, "adres", "Adres");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Adrese Bağlı Sipariş Var SİLİNEMEZ");
            }
        }

        private void buttonAdresTanimla_Click(object sender, EventArgs e)
        {
            adresInsert();
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string subeid = dataGridViewSubeler.CurrentRow.Cells[0].Value.ToString();
            string adresid = Database.dbCommand(
                "select MAX(adres_id) from adres").ExecuteScalar().ToString();
            string sql = "UPDATE sube SET ";
            string values = " adres=" + adresid;
            sql += values + " where sube_id=" + subeid;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Adres Başarıyla Seçilen Şubeye Eklendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString() + " " + sql);
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(musteriID);
        }
    }
}
