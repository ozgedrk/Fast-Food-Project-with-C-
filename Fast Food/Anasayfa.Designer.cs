namespace Fast_Food
{
    partial class Anasayfa
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonPersoneller = new System.Windows.Forms.Button();
            this.buttonSiparisYönetimi = new System.Windows.Forms.Button();
            this.buttonMusteriler = new System.Windows.Forms.Button();
            this.buttonSubeler = new System.Windows.Forms.Button();
            this.buttonCikis = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPersoneller
            // 
            this.buttonPersoneller.Location = new System.Drawing.Point(56, 48);
            this.buttonPersoneller.Name = "buttonPersoneller";
            this.buttonPersoneller.Size = new System.Drawing.Size(129, 70);
            this.buttonPersoneller.TabIndex = 0;
            this.buttonPersoneller.Text = "Personeller";
            this.buttonPersoneller.UseVisualStyleBackColor = true;
            this.buttonPersoneller.Click += new System.EventHandler(this.buttonPersoneller_Click);
            // 
            // buttonSiparisYönetimi
            // 
            this.buttonSiparisYönetimi.Location = new System.Drawing.Point(298, 48);
            this.buttonSiparisYönetimi.Name = "buttonSiparisYönetimi";
            this.buttonSiparisYönetimi.Size = new System.Drawing.Size(129, 70);
            this.buttonSiparisYönetimi.TabIndex = 1;
            this.buttonSiparisYönetimi.Text = "Sipariş Yönetimi";
            this.buttonSiparisYönetimi.UseVisualStyleBackColor = true;
            this.buttonSiparisYönetimi.Click += new System.EventHandler(this.buttonSiparisYönetimi_Click);
            // 
            // buttonMusteriler
            // 
            this.buttonMusteriler.Location = new System.Drawing.Point(56, 157);
            this.buttonMusteriler.Name = "buttonMusteriler";
            this.buttonMusteriler.Size = new System.Drawing.Size(129, 70);
            this.buttonMusteriler.TabIndex = 2;
            this.buttonMusteriler.Text = "Müşteriler";
            this.buttonMusteriler.UseVisualStyleBackColor = true;
            this.buttonMusteriler.Click += new System.EventHandler(this.buttonMusteriler_Click);
            // 
            // buttonSubeler
            // 
            this.buttonSubeler.Location = new System.Drawing.Point(298, 157);
            this.buttonSubeler.Name = "buttonSubeler";
            this.buttonSubeler.Size = new System.Drawing.Size(129, 70);
            this.buttonSubeler.TabIndex = 3;
            this.buttonSubeler.Text = "Şubeler";
            this.buttonSubeler.UseVisualStyleBackColor = true;
            this.buttonSubeler.Click += new System.EventHandler(this.buttonAdmin_Click);
            // 
            // buttonCikis
            // 
            this.buttonCikis.Location = new System.Drawing.Point(495, 232);
            this.buttonCikis.Name = "buttonCikis";
            this.buttonCikis.Size = new System.Drawing.Size(77, 39);
            this.buttonCikis.TabIndex = 4;
            this.buttonCikis.Text = "Çıkış";
            this.buttonCikis.UseVisualStyleBackColor = true;
            this.buttonCikis.Click += new System.EventHandler(this.buttonCikis_Click);
            // 
            // Anasayfa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 283);
            this.Controls.Add(this.buttonCikis);
            this.Controls.Add(this.buttonSubeler);
            this.Controls.Add(this.buttonMusteriler);
            this.Controls.Add(this.buttonSiparisYönetimi);
            this.Controls.Add(this.buttonPersoneller);
            this.Name = "Anasayfa";
            this.Text = "Anasayfa";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPersoneller;
        private System.Windows.Forms.Button buttonSiparisYönetimi;
        private System.Windows.Forms.Button buttonMusteriler;
        private System.Windows.Forms.Button buttonSubeler;
        private System.Windows.Forms.Button buttonCikis;
    }
}