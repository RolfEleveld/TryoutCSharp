// <copyright file="Program.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

namespace Microsoft.ServiceModel.WebSockets.Samples
{
    internal partial class GameWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label service;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon gameService;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            this.service = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gameService = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // Service
            // 
            this.service.AutoSize = true;
            this.service.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.service.Location = new System.Drawing.Point(120, 57);
            this.service.Name = "Service";
            this.service.Size = new System.Drawing.Size(279, 32);
            this.service.TabIndex = 0;
            this.service.Text = "Web Sockets Demo";
            this.service.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(123, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Game Service Running...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameService
            // 
            this.gameService.Icon = ((System.Drawing.Icon)(resources.GetObject("GameService.Icon")));
            this.gameService.Text = "notifyIcon1";
            this.gameService.Visible = true;
            this.gameService.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GameService_MouseDoubleClick);
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(520, 212);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.service);
            this.Name = "GameWindow";
            this.Text = "Web Sockets Demo - Game Service";
            this.Load += new System.EventHandler(this.GameWindow_Load);
            this.Resize += new System.EventHandler(this.GameWindow_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}