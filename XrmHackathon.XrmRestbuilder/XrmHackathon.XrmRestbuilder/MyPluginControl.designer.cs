using System.Drawing;
using System.Windows.Forms;

namespace XrmHackathon.XrmRestbuilder
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Button button1;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {

            this.browser = new WebBrowser();
            base.SuspendLayout();
            this.browser.Dock = DockStyle.Fill;
            this.browser.Location = new Point(0, 0);
            this.browser.MinimumSize = new Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new Size(784, 400);
            this.browser.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.browser);
            base.Name = "Ribbon Workbench 2016";
            base.Size = new Size(784, 400);
            base.ResumeLayout(false);
        }

        #endregion

        public WebBrowser browser { get; private set; }

        private Button button2;
    }
}
