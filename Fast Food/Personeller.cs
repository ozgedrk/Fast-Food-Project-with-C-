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
    public partial class Personeller : Form
    {
        Form form;
        string personelID;
        string subeID;
        DataSet dataSetPersoneller = new DataSet();
        DataSet dataSetAdresler = new DataSet();
        DataSet dataSetSehirler = new DataSet();
        DataSet dataSetIlceler = new DataSet();
        DataSet dataSetSubeler = new DataSet();
        DataSet dataSetUnvanlar = new DataSet();

        public Personeller()
        {
            InitializeComponent();

        }

        public Personeller(string personelID, string subeID, Form form)
            : this()
        {
            this.form = form;
            this.personelID = personelID;
            this.subeID = subeID;
            try
            {
                if (Database.dbConnection().State == ConnectionState.Closed)
                {
                    Database.dbConnection().Open();
                }
                string personelAd = Database.dbCommand(
                "select ad from personel where personel_id=" + personelID
                ).ExecuteScalar().ToString();
                string personelSoyad = Database.dbCommand(
               "select soyad from personel where personel_id=" + personelID
               ).ExecuteScalar().ToString();
                string personelUnvanID = Database.dbCommand(
               "select unvan from personel where personel_id=" + personelID
               ).ExecuteScalar().ToString();
                string personelUnvan = "Belirtilmemiş";
                if (int.Parse(personelUnvanID) > 0)
                {
                    personelUnvan = Database.dbCommand(
                   "select unvan from unvan where unvan_id=" + personelUnvanID
                   ).ExecuteScalar().ToString();
                }
                if (Database.dbConnection().State == ConnectionState.Open)
                {
                    Database.dbConnection().Close();
                }
                labelPersonelAd.Text = personelAd;
                labelPersonelSoyad.Text = personelSoyad;
                labelPersonelUnvan.Text = personelUnvan;
                labelPersonelID.Text = personelID;
                this.tabloyaVeriDoldur();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
        }

        public void tabloyaVeriDoldur()
        {
            dataSetPersoneller = new DataSet();
            dataSetAdresler = new DataSet();
            dataSetSehirler = new DataSet();
            dataSetIlceler = new DataSet();
            dataSetSubeler = new DataSet();
            dataSetUnvanlar = new DataSet();
            if (!KullaniciGirisi.isAdmin(personelID) && personelID != null)
            {
                Database.dbAdapter("SELECT * FROM personel_view WHERE personel_id =" + personelID + " ORDER BY personel_id ASC").Fill(dataSetPersoneller);
                dataGridViewPersoneller.DataSource = dataSetPersoneller.Tables[0];
                string adresID = dataGridViewPersoneller.Rows[0].Cells[9].Value.ToString();
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
                string subeID = dataGridViewPersoneller.Rows[0].Cells[10].Value.ToString();
                subeID = subeID.Trim();
                subeID = subeID.Substring(subeID.LastIndexOf("ID:"));
                subeID = subeID.Replace("ID:", "").Trim();
                if (subeID != "")
                {
                    Database.dbAdapter("SELECT * FROM sube WHERE sube_id=" + subeID + " ORDER BY sube_id ASC").Fill(dataSetSubeler);
                }
                else
                {
                    Database.dbAdapter("SELECT * FROM sube ORDER BY sube_id ASC").Fill(dataSetSubeler);
                }
                dataGridViewSubeler.DataSource = dataSetSubeler.Tables[0];
                string unvanID = dataGridViewPersoneller.Rows[0].Cells[8].Value.ToString();
                unvanID = unvanID.Trim();
                unvanID = unvanID.Substring(unvanID.LastIndexOf("ID:"));
                unvanID = unvanID.Replace("ID:", "").Trim();
                if (unvanID != "")
                {
                    Database.dbAdapter("SELECT * FROM unvan WHERE unvan_id=" + unvanID + " ORDER BY unvan_id ASC").Fill(dataSetUnvanlar);
                }
                else
                {
                    Database.dbAdapter("SELECT * FROM unvan ORDER BY unvan_id ASC").Fill(dataSetUnvanlar);
                }

                dataGridViewUnvan.DataSource = dataSetUnvanlar.Tables[0];
            }
            else
            {
                Database.dbAdapter("SELECT * FROM personel_view ORDER BY personel_id ASC").Fill(dataSetPersoneller);
                dataGridViewPersoneller.DataSource = dataSetPersoneller.Tables[0];
                Database.dbAdapter("SELECT * FROM adres_view ORDER BY adres_id ASC").Fill(dataSetAdresler);
                dataGridViewAdresler.DataSource = dataSetAdresler.Tables[0];
                Database.dbAdapter("SELECT * FROM sehirler ORDER BY plaka ASC").Fill(dataSetSehirler);
                dataGridViewSehirler.DataSource = dataSetSehirler.Tables[0];
                Database.dbAdapter("SELECT * FROM ilceler ORDER BY ilce_id ASC").Fill(dataSetIlceler);
                dataGridViewIlceler.DataSource = dataSetIlceler.Tables[0];
                Database.dbAdapter("SELECT * FROM sube ORDER BY sube_id ASC").Fill(dataSetSubeler);
                dataGridViewSubeler.DataSource = dataSetSubeler.Tables[0];
                Database.dbAdapter("SELECT * FROM unvan ORDER BY unvan_id ASC").Fill(dataSetUnvanlar);
                dataGridViewUnvan.DataSource = dataSetUnvanlar.Tables[0];
            }

        }

        private void buttonGeri_Click(object sender, EventArgs e)
        {
            form.Show();
            this.Close();
        }

        private void dataGridViewPersoneller_Click(object sender, EventArgs e)
        {
            adresDoldur();
            string subeID = dataGridViewPersoneller.CurrentRow.Cells[10].Value.ToString();
            subeID = subeID.Trim();
            if (!subeID.Equals(""))
            {
                subeID = subeID.Substring(subeID.LastIndexOf("ID:"));
                subeID = subeID.Replace("ID:", "").Trim();
            }
            string unvanID = dataGridViewPersoneller.CurrentRow.Cells[8].Value.ToString();
            unvanID = unvanID.Trim();
            if (!unvanID.Equals(""))
            {
                unvanID = unvanID.Substring(unvanID.LastIndexOf("ID:"));
                unvanID = unvanID.Replace("ID:", "").Trim();
            }
            string adresID = dataGridViewPersoneller.CurrentRow.Cells[9].Value.ToString();
            adresID = adresID.Trim();
            adresID = adresID.Replace("ID:", "");
            adresID = adresID.Trim();
            if (!adresID.Equals(""))
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
            }
            this.rowSelect(dataGridViewSubeler, subeID);
            this.rowSelect(dataGridViewUnvan, unvanID);
            this.rowSelect(dataGridViewAdresler, adresID);
            
            string ise_giris_tarihi = dataGridViewPersoneller.CurrentRow.Cells[7].Value.ToString();
            int year, month, day; 
            if(ise_giris_tarihi.Equals("")){
                dateTimePickerIseGirisTarihi.Value = DateTime.Today;
            }else
            {
                
                    ise_giris_tarihi = "0" + ise_giris_tarihi;
                

                year = int.Parse(ise_giris_tarihi.Substring(6, 4).Replace(".", ""));
                month = int.Parse(ise_giris_tarihi.Substring(3, 2).Replace(".", ""));
                day = int.Parse(ise_giris_tarihi.Substring(0, 2).Replace(".",""));
                if (year < 2000)
                {
                    year = int.Parse(year.ToString() + "0");
                }
                dateTimePickerIseGirisTarihi.Value = new DateTime(year, month, day);
            }
            string cinsiyet = dataGridViewPersoneller.CurrentRow.Cells[11].Value.ToString();
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
            string adresid = dataGridViewPersoneller.CurrentRow.Cells[9].Value.ToString();
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

        public void reload(string personelID)
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
                    if (ex.ToString().Contains("siparis_personel_fkey"))
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
            this.reload(personelID);
        }

        private void buttonPersonelEkle_Click(object sender, EventArgs e)
        {
            if (dataGridViewAdresler.NewRowIndex > 0 && dataGridViewAdresler.Rows[dataGridViewAdresler.NewRowIndex - 1].Cells[0].Value.ToString().Equals(""))
            {
                this.adresInsert();
            }
            this.personelInsert();
            this.reload(personelID);
        }

        public void personelInsert()
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string ad = dataGridViewPersoneller.Rows[dataGridViewPersoneller.NewRowIndex - 1].Cells[1].Value.ToString();
            string soyad = dataGridViewPersoneller.Rows[dataGridViewPersoneller.NewRowIndex - 1].Cells[2].Value.ToString();
            string tckimlik = dataGridViewPersoneller.Rows[dataGridViewPersoneller.NewRowIndex - 1].Cells[3].Value.ToString();
            string telefon = dataGridViewPersoneller.Rows[dataGridViewPersoneller.NewRowIndex - 1].Cells[4].Value.ToString();
            string email = dataGridViewPersoneller.Rows[dataGridViewPersoneller.NewRowIndex - 1].Cells[5].Value.ToString();
            string maas = dataGridViewPersoneller.Rows[dataGridViewPersoneller.NewRowIndex - 1].Cells[6].Value.ToString();
            maas = maas.Replace(",", ".");
            string ise_giris_tarihi = dateTimePickerIseGirisTarihi.Value.ToString();
            string unvan = dataGridViewUnvan.CurrentRow.Cells[0].Value.ToString();
            string adres = Database.dbCommand(
                "select MAX(adres_id) from adres").ExecuteScalar().ToString();
            string sube = dataGridViewSubeler.CurrentRow.Cells[0].Value.ToString();
            string cinsiyet = comboBoxCinsiyet.Text.ToString();
            string sifre = dataGridViewPersoneller.Rows[dataGridViewPersoneller.NewRowIndex - 1].Cells[12].Value.ToString();
            sifre = this.SHA256Hash(sifre);
            //PERSONEL INSERT
            string sql = "INSERT INTO personel ";
            string columns = "ad, soyad, tckimlik, telefon, email, maas, ise_giris_tarihi, unvan, adres, sube, cinsiyet, sifre";
            string values = "'" + ad + "','" + soyad + "', '" + tckimlik + "', '" + telefon + "','" + email + "'," + maas + ",'" + ise_giris_tarihi + "'," +
                unvan + "," + adres + "," + sube + ",'" + cinsiyet + "','" + sifre + "'";
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Personel Başarıyla Oluşturuldu");
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

        public void personelUpdate()
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string personelid = dataGridViewPersoneller.CurrentRow.Cells[0].Value.ToString();
            string ad = dataGridViewPersoneller.CurrentRow.Cells[1].Value.ToString();
            string soyad = dataGridViewPersoneller.CurrentRow.Cells[2].Value.ToString();
            string tckimlik = dataGridViewPersoneller.CurrentRow.Cells[3].Value.ToString();
            string telefon = dataGridViewPersoneller.CurrentRow.Cells[4].Value.ToString();
            string email = dataGridViewPersoneller.CurrentRow.Cells[5].Value.ToString();
            string maas = dataGridViewPersoneller.CurrentRow.Cells[6].Value.ToString();
            maas = maas.Replace(",", ".");
            string ise_giris_tarihi = dateTimePickerIseGirisTarihi.Value.ToString();
            string punvan = dataGridViewUnvan.CurrentRow.Cells[0].Value.ToString();
            string sube = dataGridViewSubeler.CurrentRow.Cells[0].Value.ToString();
            string cinsiyet = comboBoxCinsiyet.Text.ToString();
            string sifre = dataGridViewPersoneller.CurrentRow.Cells[12].Value.ToString();
            if (sifre.Length < 50)
            {
                sifre = this.SHA256Hash(sifre);
            }
            //PERSONEL UPDATE
            string sql = "UPDATE personel SET ";
            string values = " ad='" + ad + "',soyad='" + soyad + "', tckimlik='" + tckimlik + "', telefon='" + telefon + "',email='" + email +
                "',maas=" + maas + ",ise_giris_tarihi='" + ise_giris_tarihi + "',unvan=" + punvan + ",sube=" + sube + ",cinsiyet='" + cinsiyet +
                "',sifre='" + sifre + "'";
            sql += values + " where personel_id=" + personelid;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Personel Başarıyla Güncellendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString() + " " + sql);
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);

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
            if (sehirID.Equals(""))
            {
                Database.dbAdapter("SELECT * FROM ilceler WHERE sehir=" + sehirID + " ORDER BY ilce_id ASC").Fill(dataSetIlceler);
                dataGridViewIlceler.DataSource = dataSetIlceler.Tables[0];
            }
            
        }

        private void buttonPersonelGuncelle_Click(object sender, EventArgs e)
        {
            this.adresUpdate();
            this.personelUpdate();
            this.reload(personelID);
        }

        public void adresUpdate()
        {
            string adresid = dataGridViewPersoneller.CurrentRow.Cells[9].Value.ToString();
            if (dataGridViewAdresler.Rows.Count > 1)
            {
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

        }

        private void buttonPersonelSil_Click(object sender, EventArgs e)
        {
            try
            {
                this.deleteTable(dataGridViewPersoneller, "personel", "Personel");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Kullanıcıya Ait Siparişler Var Silinemez");
            }

        }

        private void buttonSeciliAdresiGuncelle_Click(object sender, EventArgs e)
        {
            this.adresUpdate();
            this.reload(personelID);
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
            string personelid = dataGridViewPersoneller.CurrentRow.Cells[0].Value.ToString();
            string adresid = dataGridViewAdresler.CurrentRow.Cells[0].Value.ToString();
            string sql = "UPDATE personel SET ";
            string values = " adres=" + adresid;
            sql += values + " where personel_id=" + personelid;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Adres Başarıyla Seçilen Personele Eklendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString() + " " + sql);
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelid);
        }
    }
}
