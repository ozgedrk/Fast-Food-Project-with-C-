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
using System.Security.Cryptography;

namespace Fast_Food
{
    public partial class Musteriler : Form
    {
        Form form;
        string musteriID;
        string personelID;
        DataSet dataSetMusteriler = new DataSet();
        DataSet dataSetAdresler = new DataSet();
        DataSet dataSetSehirler = new DataSet();
        DataSet dataSetIlceler = new DataSet();
        

        public Musteriler()
        {
            InitializeComponent();

        }

        public Musteriler(string personelID, string musteriID, Form form)
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
                else if (musteriID != null && !musteriID.Equals(""))
                {
                    Ad = Database.dbCommand(
               "select ad from musteri where musteri_id=" + musteriID
               ).ExecuteScalar().ToString();
                    Soyad = Database.dbCommand(
                   "select soyad from musteri where musteri_id=" + musteriID
                   ).ExecuteScalar().ToString();
                }
                
               
                if (Database.dbConnection().State == ConnectionState.Open)
                {
                    Database.dbConnection().Close();
                }
                labelMusteriAd.Text = Ad;
                labelMusteriSoyad.Text = Soyad;
                labelMusteriID.Text = musteriID;
                this.tabloyaVeriDoldur();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
        }

        public void tabloyaVeriDoldur()
        {
            dataSetMusteriler = new DataSet();
            dataSetAdresler = new DataSet();
            dataSetSehirler = new DataSet();
            dataSetIlceler = new DataSet();
            
            if (musteriID != null || !KullaniciGirisi.isAdmin(personelID))
            {
                Database.dbAdapter("SELECT * FROM musteri_view WHERE musteri_id =" + musteriID + " ORDER BY musteri_id DESC").Fill(dataSetMusteriler);
                dataGridViewMusteriler.DataSource = dataSetMusteriler.Tables[0];
                string adresID = dataGridViewMusteriler.Rows[0].Cells[5].Value.ToString();
                adresID = adresID.Trim();
                adresID = adresID.Replace("ID:", "");
                adresID = adresID.Trim();
                if (adresID != "")
                {
                    Database.dbAdapter("SELECT * FROM adres_view WHERE adres_id=" + adresID + " ORDER BY adres_id ASC").Fill(dataSetAdresler);
                }
                else
                {
                    Database.dbAdapter("SELECT * FROM adres_view ORDER BY adres_id ASC").Fill(dataSetAdresler);
                    dataSetAdresler.Tables[0].Clear();
                }

                dataGridViewAdresler.DataSource = dataSetAdresler.Tables[0];
                Database.dbAdapter("SELECT * FROM sehirler ORDER BY plaka ASC").Fill(dataSetSehirler);
                dataGridViewSehirler.DataSource = dataSetSehirler.Tables[0];
                Database.dbAdapter("SELECT * FROM ilceler ORDER BY ilce_id ASC").Fill(dataSetIlceler);
                dataGridViewIlceler.DataSource = dataSetIlceler.Tables[0];
                
                
                
                
            }
            else
            {
                Database.dbAdapter("SELECT * FROM musteri_view ORDER BY musteri_id ASC").Fill(dataSetMusteriler);
                dataGridViewMusteriler.DataSource = dataSetMusteriler.Tables[0];
                Database.dbAdapter("SELECT * FROM adres_view ORDER BY adres_id ASC").Fill(dataSetAdresler);
                dataGridViewAdresler.DataSource = dataSetAdresler.Tables[0];
                Database.dbAdapter("SELECT * FROM sehirler ORDER BY plaka ASC").Fill(dataSetSehirler);
                dataGridViewSehirler.DataSource = dataSetSehirler.Tables[0];
                Database.dbAdapter("SELECT * FROM ilceler ORDER BY ilce_id ASC").Fill(dataSetIlceler);
                dataGridViewIlceler.DataSource = dataSetIlceler.Tables[0];
               
            }

        }

        private void buttonGeri_Click(object sender, EventArgs e)
        {
            form.Show();
            this.Close();
        }

        private void dataGridViewMusteriler_Click(object sender, EventArgs e)
        {
            adresDoldur();
            
            string adresID = dataGridViewMusteriler.CurrentRow.Cells[5].Value.ToString();
            adresID = adresID.Trim();
            adresID = adresID.Replace("ID:", "");
            adresID = adresID.Trim();
            if (adresID != null && !adresID.Equals(""))
            {
                string sehirID = dataGridViewAdresler.CurrentRow.Cells[5].Value.ToString();
                sehirID = sehirID.Trim();
                sehirID = sehirID.Substring(sehirID.LastIndexOf("ID:"));
                sehirID = sehirID.Replace("ID:", "").Trim();
                string ilceID = dataGridViewAdresler.CurrentRow.Cells[6].Value.ToString();
                ilceID = ilceID.Trim();
                ilceID = ilceID.Substring(ilceID.LastIndexOf("ID:")); ;
                ilceID = ilceID.Replace("ID:", "").Trim();
                this.rowSelect(dataGridViewSehirler, sehirID);
                this.dataGridViewSehirler_Click(dataGridViewSehirler, new EventArgs());
                this.rowSelect(dataGridViewIlceler, ilceID);
                this.dataGridViewSehirler_Click(dataGridViewSehirler, new EventArgs());
            }
            
            this.rowSelect(dataGridViewAdresler, adresID);

            string cinsiyet = dataGridViewMusteriler.CurrentRow.Cells[6].Value.ToString();
            if (!cinsiyet.Equals(""))
            {

                comboBoxCinsiyet.SelectedIndex = 1;
                if (cinsiyet.Equals("Erkek"))
                {
                    comboBoxCinsiyet.SelectedIndex = 0;
                }
            }

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
            string adresid = dataGridViewMusteriler.CurrentRow.Cells[5].Value.ToString();
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
        string SHA256Hash(string text)
        {
            string source = text;
            using (SHA256 sha1Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
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
                    if (ex.ToString().Contains("siparis_musteri_fkey"))
                        MessageBox.Show("Veritabanı Hatası: ADRESE BAĞLI SİPARİŞ VAR SİLİNEMEZ");
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

        private void buttonMusteriEkle_Click(object sender, EventArgs e)
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
            string ad = dataGridViewMusteriler.Rows[dataGridViewMusteriler.NewRowIndex - 1].Cells[1].Value.ToString();
            string soyad = dataGridViewMusteriler.Rows[dataGridViewMusteriler.NewRowIndex - 1].Cells[2].Value.ToString();
            string telefon = dataGridViewMusteriler.Rows[dataGridViewMusteriler.NewRowIndex - 1].Cells[3].Value.ToString();
            string email = dataGridViewMusteriler.Rows[dataGridViewMusteriler.NewRowIndex - 1].Cells[4].Value.ToString();
            
            
            string adres = Database.dbCommand(
                "select MAX(adres_id) from adres").ExecuteScalar().ToString();
            string cinsiyet = comboBoxCinsiyet.Text.ToString();
            string sifre = dataGridViewMusteriler.Rows[dataGridViewMusteriler.NewRowIndex - 1].Cells[7].Value.ToString();
            sifre = this.SHA256Hash(sifre);
            //MÜŞTERİ INSERT
            string sql = "INSERT INTO musteri ";
            string columns = "ad, soyad, telefon, email, adres, cinsiyet, sifre";
            string values = "'" + ad + "','" + soyad + "', '" + telefon + "','" + email + "'," + adres + ",'" + cinsiyet + "','" + sifre + "'";
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Müşteri Başarıyla Oluşturuldu");
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

        public void musteriUpdate()
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string musteriid = dataGridViewMusteriler.CurrentRow.Cells[0].Value.ToString();
            string ad = dataGridViewMusteriler.CurrentRow.Cells[1].Value.ToString();
            string soyad = dataGridViewMusteriler.CurrentRow.Cells[2].Value.ToString();
            string telefon = dataGridViewMusteriler.CurrentRow.Cells[3].Value.ToString();
            string email = dataGridViewMusteriler.CurrentRow.Cells[4].Value.ToString();
            string cinsiyet = comboBoxCinsiyet.Text.ToString();
            string sifre = dataGridViewMusteriler.CurrentRow.Cells[7].Value.ToString();
            sifre = this.SHA256Hash(sifre);
            //MÜŞTERİ UPDATE
            string sql = "UPDATE musteri SET ";
            string values = " ad='" + ad + "',soyad='" + soyad + "', telefon='" + telefon + "',email='" + email +
                "',cinsiyet='" + cinsiyet + "',sifre='" + sifre + "'";
            sql += values + " where musteri_id=" + musteriid;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Müşteri Başarıyla Güncellendi");
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

        private void buttonMusteriGuncelle_Click(object sender, EventArgs e)
        {
            this.adresUpdate();
            this.musteriUpdate();
            this.reload(musteriID);
        }

        public void adresUpdate()
        {
            string adresid = dataGridViewMusteriler.CurrentRow.Cells[5].Value.ToString();
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

        private void buttonMusteriSil_Click(object sender, EventArgs e)
        {
            try
            {
                this.deleteTable(dataGridViewMusteriler, "musteri", "Müşteri");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Kullanıcıya Ait Siparişler Var Silinemez");
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
            string musteriid = dataGridViewMusteriler.CurrentRow.Cells[0].Value.ToString();
            string adresid = dataGridViewAdresler.CurrentRow.Cells[0].Value.ToString();
            string sql = "UPDATE musteri SET ";
            string values = " adres=" + adresid;
            sql += values + " where musteri_id=" + musteriid;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Adres Başarıyla Seçilen Müşteriye Eklendi");
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
