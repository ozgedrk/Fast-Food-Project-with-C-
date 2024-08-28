using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fast_Food
{
    public partial class Anasayfa : Form
    {
        string musteriID;
        string personelID;
        string subeID;
        Form form;
        public Anasayfa()
        {
            InitializeComponent();
        }
        public Anasayfa(string personelID, string subeID, string musteriID, Form form) : this()
        {
            this.form = form;
            this.personelID = personelID;
            this.musteriID = musteriID;
            this.subeID = subeID;
            if (personelID == null)
            {
                buttonSubeler.Hide();
                buttonPersoneller.Hide();
                buttonSiparisYönetimi.Hide();
            }
            else
            {

                if (!KullaniciGirisi.isAdmin(personelID))
                {
                    buttonMusteriler.Hide();
                    buttonSubeler.Hide();
                }
            }
        }

        private void buttonPersoneller_Click(object sender, EventArgs e)
        {
            
            new Personeller(personelID, subeID, this).Show();
            this.Hide();
        }

        private void buttonSiparisYönetimi_Click(object sender, EventArgs e)
        {
            new SiparisYonetimi(personelID, subeID, this).Show();
            this.Hide();
        }

        private void buttonMusteriler_Click(object sender, EventArgs e)
        {
            new Musteriler(personelID, musteriID, this).Show();
            this.Hide();
        }

        private void buttonAdmin_Click(object sender, EventArgs e)
        {
            new Subeler(personelID, musteriID, this).Show();
            this.Hide();
        }

        private void buttonCikis_Click(object sender, EventArgs e)
        {
            form.Show();
            this.Hide();
        }
    }
}
