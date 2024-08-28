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
    public partial class SiparisYonetimi : Form
    {
        Form form;
        string personelID;
        string subeID;
        DataSet dataSetSiparisler = new DataSet();
        DataSet dataSetMenuler = new DataSet();
        DataSet dataSetKategori= new DataSet();
        DataSet dataSetBoy = new DataSet();
        DataSet dataSetSiparisTurleri = new DataSet();
        DataSet dataSetSiparisinUrunleri = new DataSet();
        DataSet dataSetSiparisinMenuleri = new DataSet();
        DataSet dataSetUrunler = new DataSet();

        public SiparisYonetimi()
        {
            InitializeComponent();
            this.tabloyaVeriDoldur();
        }
        public SiparisYonetimi(string personelID, string subeID, Form form) : this()
        {
            this.form = form;
            this.personelID = personelID;
            this.subeID = subeID;
            try{
             if (Database.dbConnection().State == ConnectionState.Closed) { 
                Database.dbConnection().Open();
                }
                string personelAd = Database.dbCommand(
                "select ad from personel where personel_id=" + personelID
                ).ExecuteScalar().ToString();
                string personelSoyad = Database.dbCommand(
               "select soyad from personel where personel_id=" + personelID
               ).ExecuteScalar().ToString();
                if (Database.dbConnection().State == ConnectionState.Open) { 
                Database.dbConnection().Close();
                }
                labelPersonelAd.Text = personelAd;
                labelPersonelSoyad.Text = personelSoyad;
                labelPersonelID.Text = personelID;
                
                
            }
            
            catch(Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.ToString());
            }
        }

        public void tabloyaVeriDoldur()
        {
            dataSetSiparisler = new DataSet();
            dataSetMenuler = new DataSet();
            dataSetKategori = new DataSet();
            dataSetBoy = new DataSet();
            dataSetSiparisTurleri = new DataSet();
            dataSetUrunler = new DataSet();
            Database.dbAdapter("SELECT * FROM siparis_view ORDER BY siparis_id DESC").Fill(dataSetSiparisler);
            dataGridViewSiparisler.DataSource = dataSetSiparisler.Tables[0];
            Database.dbAdapter("SELECT * FROM menu ORDER BY menu_id ASC").Fill(dataSetMenuler);
            dataGridViewMenuler.DataSource = dataSetMenuler.Tables[0];
            Database.dbAdapter("SELECT * FROM kategori ORDER BY kategori_id ASC").Fill(dataSetKategori);
            dataGridViewKategori.DataSource = dataSetKategori.Tables[0];
            Database.dbAdapter("SELECT * FROM boylar ORDER BY boy_id ASC").Fill(dataSetBoy);
            dataGridViewBoy.DataSource = dataSetBoy.Tables[0];
            Database.dbAdapter("SELECT * FROM siparis_turleri ORDER BY tur_id ASC").Fill(dataSetSiparisTurleri);
            dataGridViewSiparisTurleri.DataSource = dataSetSiparisTurleri.Tables[0];
            Database.dbAdapter("SELECT * FROM urun ORDER BY urun_id ASC").Fill(dataSetUrunler);
            dataGridViewUrunler.DataSource = dataSetUrunler.Tables[0];
        }

        private void buttonBoyEkle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewBoy;
            //BOYLAR INSERT
            string sql = "INSERT INTO boylar ";
            string columns = "";
            string values = "";
            columns += dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].OwningColumn.HeaderText.ToString();
            values += "'" + dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].Value.ToString() + "'";
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Boy Başarıyla Kaydedildi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        public void reload(string personelID)
        {
            this.tabloyaVeriDoldur();
        }

        private void buttonBoyGuncelle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewBoy;
            //BOYLAR UPDATE
            string sql = "UPDATE boylar  ";
            string boy_ad = "";
            string boy_id = "";
            boy_ad = "'" + dataGrid.CurrentRow.Cells[1].Value.ToString() + "'";
            boy_id = dataGrid.CurrentRow.Cells[0].Value.ToString();
            sql = sql + " set boy_ad = " + boy_ad + " where boy_id= " + boy_id;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Boy Başarıyla Güncellendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonKategoriEkle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewKategori;
            //KATEGORİ INSERT
            string sql = "INSERT INTO kategori ";
            string columns = "";
            string values = "";
            columns += dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].OwningColumn.HeaderText.ToString();
            values += "'" + dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].Value.ToString() + "'";
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Kategori Başarıyla Kaydedildi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonKategoriGuncelle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewKategori;
            //KATEGORİ UPDATE
            string sql = "UPDATE kategori  ";
            string kategori_ad = "";
            string kategori_id = "";
            kategori_ad = "'" + dataGrid.CurrentRow.Cells[1].Value.ToString() + "'";
            kategori_id = dataGrid.CurrentRow.Cells[0].Value.ToString();
            sql = sql + " set kategori_ad = " + kategori_ad + " where kategori_id= " + kategori_id;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Kategori Başarıyla Güncellendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonMenuEkle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewMenuler;
            //MENÜ INSERT
            string sql = "INSERT INTO menu ";
            string columns = "";
            string values = "";
            columns += dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].OwningColumn.HeaderText.ToString() + ",";
            columns += dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[2].OwningColumn.HeaderText.ToString();
            values += "'" + dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].Value.ToString() + "',";
            values += dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[2].Value.ToString();
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Menü Başarıyla Kaydedildi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonMenuGuncelle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewMenuler;
            //MENÜ UPDATE
            string sql = "UPDATE menu  ";
            string menu_ad, fiyat = "";
            string menu_id = "";
            menu_ad = "'" + dataGrid.CurrentRow.Cells[1].Value.ToString() + "'";
            fiyat = dataGrid.CurrentRow.Cells[2].Value.ToString();
            menu_id = dataGrid.CurrentRow.Cells[0].Value.ToString();
            sql = sql + " set menu_ad = " + menu_ad +
                ", fiyat = " + fiyat + " where menu_id= " + menu_id;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Menü Başarıyla Güncellendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonBoySil_Click(object sender, EventArgs e)
        {
            this.deleteTable(dataGridViewBoy, "boylar", "Boy");
        
        }

        private void buttonKategoriSil_Click(object sender, EventArgs e)
        {
            this.deleteTable(dataGridViewKategori, "kategori", "Kategori");
        }

        private void buttonMenuSil_Click(object sender, EventArgs e)
        {
            this.deleteTable(dataGridViewMenuler, "menu", "Menü");
        }

        private void buttonUrunEkle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewUrunler;
            //URUNLER INSERT
            string sql = "INSERT INTO urun ";
            string columns = "urun_ad, fiyat";
            string values = "";
            values += "'" + dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].Value.ToString() + "', ";
            values += dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[2].Value.ToString();
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Ürün Başarıyla Kaydedildi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonUrunGuncelle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewUrunler;
            //URUNLER UPDATE
            string sql = "UPDATE urun  ";
            string urun_ad = "";
            string urun_id = "";
            string fiyat = "";
            urun_ad = "'" + dataGrid.CurrentRow.Cells[1].Value.ToString() + "'";
            urun_id = dataGrid.CurrentRow.Cells[0].Value.ToString();
            fiyat = dataGrid.CurrentRow.Cells[2].Value.ToString();
            sql = sql + " set urun_ad = " + urun_ad + ", fiyat = " + fiyat + " where urun_id= " + urun_id;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Ürün Başarıyla Güncellendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonUrunSil_Click(object sender, EventArgs e)
        {
            this.deleteTable(dataGridViewUrunler, "urun", "Ürün");
        }

        private void buttonSiparisTuruEkle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewSiparisTurleri;
            //SİPARİŞ TÜRÜ INSERT
            string sql = "INSERT INTO siparis_turleri ";
            string columns = "";
            string values = "";
            columns += dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].OwningColumn.HeaderText.ToString();
            values += "'" + dataGrid.Rows[dataGrid.NewRowIndex - 1].Cells[1].Value.ToString() + "'";
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Sipariş Türü Başarıyla Kaydedildi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonSiparisTuruGuncelle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewSiparisTurleri;
            //SİPARİŞ TÜRLERİ UPDATE
            string sql = "UPDATE siparis_turleri  ";
            string tur_ad = "";
            string tur_id = "";
            tur_ad = "'" + dataGrid.CurrentRow.Cells[1].Value.ToString() + "'";
            tur_id = dataGrid.CurrentRow.Cells[0].Value.ToString();
            sql = sql + " set tur_ad = " + tur_ad + " where tur_id= " + tur_id;

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Sipariş Türü Başarıyla Güncellendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonSiparisTuruSil_Click(object sender, EventArgs e)
        {
            this.deleteTable(dataGridViewSiparisTurleri, "siparis_turleri", "Sipariş Türü");
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
                MessageBox.Show( nesne + " Başarıyla Silindi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void buttonSiparisOlustur_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewSiparisTurleri;
            string siparisTuruID = dataGridViewSiparisTurleri.CurrentRow.Cells[0].Value.ToString();
            string siparisTarihi = DateTime.Now.ToString();
            //SİPARİŞ INSERT
            string sql = "INSERT INTO siparis ";
            string columns = "siparis_tarihi, personel, sube, siparis_turu";
            string values = "'" + siparisTarihi + "'," + personelID + ", " + subeID + ", " + siparisTuruID;
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Sipariş Başarıyla Oluşturuldu");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.reload(personelID);
        }

        private void dataGridViewSiparisler_Click(object sender, EventArgs e)
        {
            this.siparisinUrunleriDoldur();
            this.siparisinMenuleriDoldur();
        }

        public void siparisinUrunleriDoldur()
        {
            string siparisID = dataGridViewSiparisler.CurrentRow.Cells[0].Value.ToString();
            dataSetSiparisinUrunleri = new DataSet();
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            if (!siparisID.Equals(""))
            {
                Database.dbAdapter("SELECT * FROM siparis_urun_ve_boy_adi where siparis = " + siparisID
                ).Fill(dataSetSiparisinUrunleri);
                dataGridViewSiparisinUrunleri.DataSource = dataSetSiparisinUrunleri.Tables[0];
            }
            
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
        }

        public void siparisinMenuleriDoldur()
        {
            string siparisID = dataGridViewSiparisler.CurrentRow.Cells[0].Value.ToString();
            dataSetSiparisinMenuleri = new DataSet();
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            if (!siparisID.Equals(""))
            {
                Database.dbAdapter("SELECT * FROM siparis_menu_ve_boy_adi where siparis = " + siparisID 
                ).Fill(dataSetSiparisinMenuleri);
                dataGridViewSiparisinMenuleri.DataSource = dataSetSiparisinMenuleri.Tables[0];
            }

            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
        }

        private void buttonMenuyuSipariseEkle_Click(object sender, EventArgs e)
        {
            string siparisTarihi;

            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewBoy;
            //SİPARİŞİN MENÜLERİ INSERT
            string sql = "INSERT INTO siparis_menu ";
            string columns = "menu, siparis, boy";
            string values = "";
            string menuID = dataGridViewMenuler.CurrentRow.Cells[0].Value.ToString();
            string siparisID = dataGridViewSiparisler.CurrentRow.Cells[0].Value.ToString();
            string boyID = dataGridViewBoy.CurrentRow.Cells[0].Value.ToString();
            values += menuID + ", " + siparisID + ", " + boyID;
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Siparişe Menü Başarıyla Eklendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.siparisinMenuleriDoldur();
            this.reload(personelID);
        }

        private void buttonUrunuSipariseEkle_Click(object sender, EventArgs e)
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            DataGridView dataGrid = dataGridViewBoy;
            //SİPARİŞİN ÜRÜNLERİ INSERT
            string sql = "INSERT INTO siparis_urun ";
            string columns = "siparis, urun, boy";
            string values = "";
            string urunID = dataGridViewUrunler.CurrentRow.Cells[0].Value.ToString();
            string siparisID = dataGridViewSiparisler.CurrentRow.Cells[0].Value.ToString();
            string boyID = dataGridViewBoy.CurrentRow.Cells[0].Value.ToString();
            values += siparisID + ", " + urunID + ", " + boyID;
            sql = sql + "( " + columns + " ) VALUES( " + values + ")";

            try
            {
                Database.dbCommand(sql).ExecuteNonQuery();
                MessageBox.Show("Siparişe Ürün Başarıyla Eklendi");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
            }
            if (Database.dbConnection().State == ConnectionState.Open)
            {
                Database.dbConnection().Close();
            }
            this.siparisinUrunleriDoldur();
            this.reload(personelID);
        }

        private void buttonMenuyuSiparistenKaldir_Click(object sender, EventArgs e)
        {
            this.deleteTable(dataGridViewSiparisinMenuleri, "siparis_menu", "Siparişe Ait Seçilen Menü");
            this.fiyatguncelle();
            this.siparisinMenuleriDoldur();
        }

        private void buttonUrunuSiparistenKaldir_Click(object sender, EventArgs e)
        {
            this.deleteTable(dataGridViewSiparisinUrunleri, "siparis_urun", "Siparişe Ait Seçilen Ürün");
            this.fiyatguncelle();
            this.siparisinUrunleriDoldur();
        }
        private void fiyatguncelle()
        {
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            try
            {
                string siparisID = dataGridViewSiparisler.CurrentRow.Cells[0].Value.ToString();
                string sql = "CALL public.siparis_tutar(" + siparisID +")";
                Database.dbCommand(sql).ExecuteNonQuery();
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

        private void buttonSiparisSil_Click(object sender, EventArgs e)
        {
            int menuSayisi = dataGridViewSiparisinMenuleri.RowCount - 1;
            int urunSayisi = dataGridViewSiparisinUrunleri.RowCount - 1;
            if (Database.dbConnection().State == ConnectionState.Closed)
            {
                Database.dbConnection().Open();
            }
            string menuID;
            string urunID;
            string sql; 

            for (int i = 0; i < menuSayisi; i++)
            {
                menuID = dataGridViewSiparisinMenuleri.Rows[i].Cells[0].Value.ToString();
                sql = "DELETE FROM siparis_menu where id =  " + menuID;
                try
                {
                    Database.dbCommand(sql).ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
                }
            }
            for (int i = 0; i < urunSayisi; i++)
            {
                urunID = dataGridViewSiparisinUrunleri.Rows[i].Cells[0].Value.ToString();
                sql = "DELETE FROM siparis_urun where id =  " + urunID;
                try
                {
                    Database.dbCommand(sql).ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("Veritabanı Hatası: " + ex.ToString());
                }
            }
            
                this.deleteTable(dataGridViewSiparisler, "siparis", "Sipariş");
        }

        private void buttonCikis_Click(object sender, EventArgs e)
        {
            form.Show();
            this.Close();
        }

       

    }
}
