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
    public partial class KullaniciGirisi : Form
    {
        public string personelID = "1";
        public KullaniciGirisi()
        {
            InitializeComponent();

        }

        private void buttonGirisYap_Click(object sender, EventArgs e)
        {
            this.girisYap();
        }

        public void girisYap()
        {
            try
            {
                if (Database.dbConnection().State == ConnectionState.Closed)
                {
                    Database.dbConnection().Open();
                }

                if (textBoxKullaniciAdi.Text.Contains("@"))
                {
                    string sifre = this.SHA256Hash(textBoxSifre.Text);
                    string musteriID = Database.dbCommand(
                "select musteri_id from musteri where email='" + textBoxKullaniciAdi.Text +
                "' and sifre='" + this.SHA256Hash(textBoxSifre.Text) + "'"
                ).ExecuteScalar().ToString();
                    if (Database.dbConnection().State == ConnectionState.Open)
                    {
                        Database.dbConnection().Close();
                    }

                    if (
                     int.Parse(musteriID) > 0
                    )
                    {
                        Form form = new Anasayfa(null, null, musteriID, this);
                         form.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Böyle bir müşteri kayıtlı değil veya şifre veya kullanıcı adı yanlış");
                    }
                }
                else
                {
                    string personelID = Database.dbCommand(
                "select personel_id from personel where tckimlik='" + textBoxKullaniciAdi.Text +
                "' and sifre='" + this.SHA256Hash(textBoxSifre.Text) + "'"
                ).ExecuteScalar().ToString();
                    string subeID = Database.dbCommand(
                    "select personel_id from personel where tckimlik='" + textBoxKullaniciAdi.Text +
                    "' and sifre='" + this.SHA256Hash(textBoxSifre.Text) + "'"
                    ).ExecuteScalar().ToString();
                    if (Database.dbConnection().State == ConnectionState.Open)
                    {
                        Database.dbConnection().Close();
                    }

                    if (
                     int.Parse(personelID) > 0
                    )
                    {
                        Form form = new Anasayfa(personelID, subeID, null, this);
                        form.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Böyle bir personel kayıtlı değil veya şifre veya kullanıcı adı yanlış");
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Böyle bir personel kayıtlı değil veya şifre veya kullanıcı adı yanlış");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.ToString());
            }
        }

        public void reload()
        {
            Form form = new KullaniciGirisi();
            form.Show();
            this.Hide();
        }

        public static bool isAdmin(string personelID)
        {
            if (personelID != null)
            {
                string unvan = Database.dbCommand(
                "select unvan from personel where personel_id='" + personelID + "'"
                ).ExecuteScalar().ToString();
                if (int.Parse(unvan) == 10)
                {
                    return true;
                }
            }
            return false;
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
    }
}
