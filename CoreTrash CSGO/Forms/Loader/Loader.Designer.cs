namespace Ruby_CSGO.Forms.Loader
{
    partial class Loader
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.diaLogo = new System.Windows.Forms.PictureBox();
            this.loaderSpinner = new MetroFramework.Controls.MetroProgressSpinner();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.diaLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // diaLogo
            // 
            this.diaLogo.Image = global::Ruby_CSGO.Properties.Resources.ruby_lang;
            this.diaLogo.Location = new System.Drawing.Point(69, 127);
            this.diaLogo.Name = "diaLogo";
            this.diaLogo.Size = new System.Drawing.Size(40, 38);
            this.diaLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.diaLogo.TabIndex = 0;
            this.diaLogo.TabStop = false;
            // 
            // loaderSpinner
            // 
            this.loaderSpinner.Location = new System.Drawing.Point(32, 87);
            this.loaderSpinner.Maximum = 100;
            this.loaderSpinner.Name = "loaderSpinner";
            this.loaderSpinner.Size = new System.Drawing.Size(120, 120);
            this.loaderSpinner.Style = MetroFramework.MetroColorStyle.Red;
            this.loaderSpinner.TabIndex = 1;
            this.loaderSpinner.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.loaderSpinner.UseSelectable = true;
            this.loaderSpinner.UseStyleColors = true;
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTextBox.Cursor = System.Windows.Forms.Cursors.No;
            this.logTextBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logTextBox.ForeColor = System.Drawing.Color.Red;
            this.logTextBox.Location = new System.Drawing.Point(182, 87);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(408, 120);
            this.logTextBox.TabIndex = 2;
            this.logTextBox.Text = "";
            // 
            // Loader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 234);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.diaLogo);
            this.Controls.Add(this.loaderSpinner);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Loader";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.Text = "CoreTrash Loader";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Loader_FormClosing);
            this.Shown += new System.EventHandler(this.Loader_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.diaLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox diaLogo;
        private MetroFramework.Controls.MetroProgressSpinner loaderSpinner;
        private System.Windows.Forms.RichTextBox logTextBox;
    }
}

