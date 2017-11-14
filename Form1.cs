/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using DirectShowLib;
using System.Timers;

namespace RockShot
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
    public class Form1 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Button button2;
        private Capture cam;
        private CheckBox saveOnBarcodeScan;
        private Panel panel1;
        private TextBox textBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem setSaveLocationToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog1;
        private String locationToOpenForWindowsPictureViewer;
        private Bitmap stillImage = null;
        private PictureBox pictureBox3;
        private List<PictureBox> onDiskImages = new List<PictureBox>();
        private ToolStripMenuItem setFPSToolStripMenuItem;
        private ToolStripMenuItem fPSToolStripMenuItem;
        private ToolStripMenuItem fPSToolStripMenuItem1;
        private Label label1;
        private CheckBox captureOnBarcodeScan;
        private System.Timers.Timer aTimer;
        private Point initDragLoc = Point.Empty;
        private Point draggedTo = Point.Empty;
        private Point recentClick = Point.Empty;
        private Point totalDrag = new Point(0, 0);
        private Button resetImageButton;
        private DateTime mouseDownStart;
        private ToolStripMenuItem backgroundColorToolStripMenuItem;
        private ColorDialog colorDialog1;
        private ToolStripMenuItem fPSToolStripMenuItem3;
        private int imageMultiplier = 1;
        private Dictionary<PictureBox, string> map = new Dictionary<PictureBox, string>();
        private Point panelDragStart;
        private int panelVsvStart;
        private int totalChangeVal = 0;
        private bool isScrolling = false;
        private ToolStripMenuItem exitToolStripMenuItem;
        private DateTime allowMouseMoveAt = DateTime.Now;
        private string scannedText = "";
        private DateTime lastInputDate = DateTime.Now;
        private int VIDEODEVICE = 2; // zero based index of video capture device to use
        private int VIDEOWIDTH = 1920; // Depends on video device caps
        private int VIDEOHEIGHT = 1080;
        private Button button3;
        private ToolStripMenuItem resetCameraImmediateToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1; // Depends on video device caps
        private short VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            try
            {
                cam = new Capture(this.VIDEODEVICE, this.VIDEOWIDTH, this.VIDEOHEIGHT, this.VIDEOBITSPERPIXEL, pictureBox2);
            }
            catch (Exception e)
            {
                this.label1.Text = "Couldn't find the camera. Reconnect the camera then restart the program";
            }

            // Encountered a huge problem where video preview worked on computer during dev but did not work on 
            // the tablet. So we are going to use an FPS setter and capture the image as still image
            // then put it in the preview pane

            // I did not figure out enough about the setup for the CameraGraph to change the resolution for image
            // capture on the fly.  I did try to set up a smaller camera, then on click: dispose of cam, set up the new
            // camera at full resolution for the main capture, but it would come back with errors on the second setup
            // saying that the camera was not ready. Decided it was better to have low FPS than a wait on scan / capture

            this.setupFpsCheckboxes();
            this.refreshImageListPanel();
            this.captureOnBarcodeScan.Checked = Properties.Settings.Default.captureOnScan;
            this.saveOnBarcodeScan.Checked = Properties.Settings.Default.saveOnScan;
        }

        private void setupFpsCheckboxes()
        {
            this.fPSToolStripMenuItem.Checked = (Properties.Settings.Default.fpsDelaySetting == 200);
            this.fPSToolStripMenuItem1.Checked = (Properties.Settings.Default.fpsDelaySetting == 100);
            this.fPSToolStripMenuItem3.Checked = (Properties.Settings.Default.fpsDelaySetting == 1000);

        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            Properties.Settings.Default.Save();
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);

                if (m_ip != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(m_ip);
                    m_ip = IntPtr.Zero;
                }
            }
            catch (Exception e)
            {
                rockShotErrorHandle(e);
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.saveOnBarcodeScan = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSaveLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fPSToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.fPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fPSToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCameraImmediateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.aTimer = new System.Timers.Timer();
            this.label1 = new System.Windows.Forms.Label();
            this.captureOnBarcodeScan = new System.Windows.Forms.CheckBox();
            this.resetImageButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Trebuchet MS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(757, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 67);
            this.button1.TabIndex = 0;
            this.button1.Text = "Capture";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox1.Location = new System.Drawing.Point(0, 177);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1040, 606);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click_1);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(766, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(33, 38);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(757, 140);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(212, 31);
            this.button2.TabIndex = 3;
            this.button2.Text = "Save Now / Overwrite";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // saveOnBarcodeScan
            // 
            this.saveOnBarcodeScan.AutoSize = true;
            this.saveOnBarcodeScan.Checked = true;
            this.saveOnBarcodeScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveOnBarcodeScan.Location = new System.Drawing.Point(451, 77);
            this.saveOnBarcodeScan.Name = "saveOnBarcodeScan";
            this.saveOnBarcodeScan.Size = new System.Drawing.Size(139, 17);
            this.saveOnBarcodeScan.TabIndex = 4;
            this.saveOnBarcodeScan.Text = "Save On Barcode Scan";
            this.saveOnBarcodeScan.UseVisualStyleBackColor = true;
            this.saveOnBarcodeScan.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1044, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 754);
            this.panel1.TabIndex = 6;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(295, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(434, 38);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1242, 31);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setSaveLocationToolStripMenuItem,
            this.setFPSToolStripMenuItem,
            this.backgroundColorToolStripMenuItem,
            this.resetCameraImmediateToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 27);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // setSaveLocationToolStripMenuItem
            // 
            this.setSaveLocationToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setSaveLocationToolStripMenuItem.Name = "setSaveLocationToolStripMenuItem";
            this.setSaveLocationToolStripMenuItem.Size = new System.Drawing.Size(382, 34);
            this.setSaveLocationToolStripMenuItem.Text = "Set Save Location...";
            this.setSaveLocationToolStripMenuItem.Click += new System.EventHandler(this.setSaveLocationToolStripMenuItem_Click);
            // 
            // setFPSToolStripMenuItem
            // 
            this.setFPSToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fPSToolStripMenuItem3,
            this.fPSToolStripMenuItem,
            this.fPSToolStripMenuItem1});
            this.setFPSToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setFPSToolStripMenuItem.Name = "setFPSToolStripMenuItem";
            this.setFPSToolStripMenuItem.Size = new System.Drawing.Size(382, 34);
            this.setFPSToolStripMenuItem.Text = "Set FPS";
            this.setFPSToolStripMenuItem.Click += new System.EventHandler(this.setFPSToolStripMenuItem_Click);
            // 
            // fPSToolStripMenuItem3
            // 
            this.fPSToolStripMenuItem3.Name = "fPSToolStripMenuItem3";
            this.fPSToolStripMenuItem3.Size = new System.Drawing.Size(163, 34);
            this.fPSToolStripMenuItem3.Text = "1 FPS";
            this.fPSToolStripMenuItem3.Click += new System.EventHandler(this.fPSToolStripMenuItem3_Click);
            // 
            // fPSToolStripMenuItem
            // 
            this.fPSToolStripMenuItem.Name = "fPSToolStripMenuItem";
            this.fPSToolStripMenuItem.Size = new System.Drawing.Size(163, 34);
            this.fPSToolStripMenuItem.Text = "5 FPS";
            this.fPSToolStripMenuItem.Click += new System.EventHandler(this.fPSToolStripMenuItem_Click);
            // 
            // fPSToolStripMenuItem1
            // 
            this.fPSToolStripMenuItem1.Name = "fPSToolStripMenuItem1";
            this.fPSToolStripMenuItem1.Size = new System.Drawing.Size(163, 34);
            this.fPSToolStripMenuItem1.Text = "10 FPS";
            this.fPSToolStripMenuItem1.Click += new System.EventHandler(this.fPSToolStripMenuItem1_Click);
            // 
            // backgroundColorToolStripMenuItem
            // 
            this.backgroundColorToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(382, 34);
            this.backgroundColorToolStripMenuItem.Text = "Background Color...";
            this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.backgroundColorToolStripMenuItem_Click);
            // 
            // resetCameraImmediateToolStripMenuItem
            // 
            this.resetCameraImmediateToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetCameraImmediateToolStripMenuItem.Name = "resetCameraImmediateToolStripMenuItem";
            this.resetCameraImmediateToolStripMenuItem.Size = new System.Drawing.Size(382, 34);
            this.resetCameraImmediateToolStripMenuItem.Text = "Reset Camera (Immediate)";
            this.resetCameraImmediateToolStripMenuItem.Click += new System.EventHandler(this.resetCameraImmediateToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(379, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(382, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(0, 31);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(249, 140);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // aTimer
            // 
            this.aTimer.Enabled = true;
            this.aTimer.Interval = global::RockShot.Properties.Settings.Default.fpsDelaySetting;
            this.aTimer.SynchronizingObject = this;
            this.aTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimedEvent);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(265, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 27);
            this.label1.TabIndex = 9;
            this.label1.Text = "Click \'Capture\' or Scan Barcode";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // captureOnBarcodeScan
            // 
            this.captureOnBarcodeScan.AutoSize = true;
            this.captureOnBarcodeScan.Checked = true;
            this.captureOnBarcodeScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.captureOnBarcodeScan.Location = new System.Drawing.Point(295, 77);
            this.captureOnBarcodeScan.Name = "captureOnBarcodeScan";
            this.captureOnBarcodeScan.Size = new System.Drawing.Size(149, 17);
            this.captureOnBarcodeScan.TabIndex = 10;
            this.captureOnBarcodeScan.Text = "Capture on Barcode Scan";
            this.captureOnBarcodeScan.UseVisualStyleBackColor = true;
            this.captureOnBarcodeScan.CheckedChanged += new System.EventHandler(this.captureOnBarcodeScan_CheckedChanged);
            // 
            // resetImageButton
            // 
            this.resetImageButton.Location = new System.Drawing.Point(270, 140);
            this.resetImageButton.Name = "resetImageButton";
            this.resetImageButton.Size = new System.Drawing.Size(174, 31);
            this.resetImageButton.TabIndex = 11;
            this.resetImageButton.Text = "Reset Image Zoom";
            this.resetImageButton.UseVisualStyleBackColor = true;
            this.resetImageButton.Click += new System.EventHandler(this.resetImageButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(596, 81);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(133, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Clear Text";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = global::RockShot.Properties.Settings.Default.backgroundColor;
            this.ClientSize = new System.Drawing.Size(1242, 785);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.resetImageButton);
            this.Controls.Add(this.captureOnBarcodeScan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.saveOnBarcodeScan);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "RockShot";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Click += new System.EventHandler(this.Form1_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aTimer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var f1 = new Form1();
            f1.textBox1.Focus();

            try
            {
                Application.Run(f1);
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                // Close it up, cant get back from here
                f1.Dispose();
            }
        }

        IntPtr m_ip = IntPtr.Zero;

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                // The preview pane works fine on a Windows machine with a connected LifeCam Studio
                // camera -- but the preview pane did not work properly with that camera
                // on the Surface tablet. However, the still image did work correctly.
                // So here we're using the still image stream and faking a video stream (putting in the FPS
                // chooser to have it emulate video). This is annoying but acceptable. Did
                // not have the budget to debug why preview did not work on Surface tablet.
                // As a note, the preview video pane did work for the integrated Surface cameras but
                // not for the usb camera "LifeCam Studio" - model 1425 which was customer hardware.
                
                if (cam == null)
                {
                    return;
                }
                this.textBox1.Focus();
                // Release any previous buffer
                if (m_ip != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(m_ip);
                    m_ip = IntPtr.Zero;
                }
                // capture image
                try
                {
                    m_ip = cam.Click();
                    Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);

                    // If the image is upsidedown
                    b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    pictureBox3.Image = b;
                }
                catch (Exception exc)
                {
                    cam.Dispose();
                    cam = null;
                }
            }
            catch (Exception exc2)
            {
                this.rockShotErrorHandle(exc2);
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.captureMain();
                this.label1.Text = "Image Not Saved";
                this.textBox1.Focus();
                this.textBox1.Text = "";
            }
            catch (Exception exc)
            {
                rockShotErrorHandle(exc);
            }

        }
        private void resetCam()
        {
            try
            {
                if (cam != null)
                {
                    cam.Dispose();
                }
                if (m_ip != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(m_ip);
                    m_ip = IntPtr.Zero;
                }

                System.Threading.Thread.Sleep(200);
                cam = new Capture(this.VIDEODEVICE, this.VIDEOWIDTH, this.VIDEOHEIGHT, this.VIDEOBITSPERPIXEL, pictureBox2);
            }
            catch (Exception exc)
            {
                rockShotErrorHandle(exc);
            }
        }
        private void captureMain()
        {
            if (cam == null)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            // Release any previous buffer
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }

            // capture image
            try
            {
                m_ip = cam.Click();
                Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);

                // If the image is upsidedown
                b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                pictureBox1.Image = b;
                this.stillImage = b;
            }
            catch (Exception exc)
            {
                cam.Dispose();
                if (m_ip != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(m_ip);
                    m_ip = IntPtr.Zero;
                }
                cam = new Capture(this.VIDEODEVICE, this.VIDEOWIDTH, this.VIDEOHEIGHT, this.VIDEOBITSPERPIXEL, pictureBox2);
                m_ip = cam.Click();
            }
            Cursor.Current = Cursors.Default;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            cam.Dispose();

            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Focus();
                if (textBox1.Text == "")
                {
                    this.label1.Text = "No filename in the text box";
                    return;
                }
                try
                {
                    if (this.stillImage != null && Properties.Settings.Default.saveLocation != null)
                    {
                        foreach (PictureBox pic in onDiskImages)
                        {
                            this.panel1.Controls.Remove(pic);
                            pic.Image.Dispose();
                            pic.Dispose();
                        }
                        File.Delete(Properties.Settings.Default.saveLocation + "\\" + textBox1.Text + ".jpg");
                        this.locationToOpenForWindowsPictureViewer = Properties.Settings.Default.saveLocation + "\\" + textBox1.Text + ".jpg";
                        this.stillImage.Save(Properties.Settings.Default.saveLocation + "\\" + textBox1.Text + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        this.label1.Text = "Saved as: " + textBox1.Text + ".jpg";
                        this.textBox1.Text = "";
                        refreshImageListPanel();
                    }
                }
                catch (DirectoryNotFoundException dnfe)
                {
                    this.label1.Text = "Set a new save directory, can't find: " + Properties.Settings.Default.saveLocation;
                    this.textBox1.SelectAll();
                }
            }
            catch (Exception exc)
            {
                rockShotErrorHandle(exc);
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.Length > 0)
                {
                    if (this.captureOnBarcodeScan.Checked)
                    {
                        this.captureMain();
                    }
                    if (this.stillImage != null && Properties.Settings.Default.saveLocation != null & this.saveOnBarcodeScan.Checked)
                    {
                        try
                        {
                            FileInfo[] canGetDirectory = new DirectoryInfo(Properties.Settings.Default.saveLocation).GetFiles();

                            try
                            {
                                this.locationToOpenForWindowsPictureViewer = Properties.Settings.Default.saveLocation + "\\" + textBox1.Text + ".jpg";
                                this.stillImage.Save(Properties.Settings.Default.saveLocation + "\\" + textBox1.Text + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                this.label1.Text = "Saved as: " + textBox1.Text + ".jpg";
                                this.textBox1.Text = "";
                                refreshImageListPanel();
                            }
                            catch (Exception exc)
                            {
                                this.label1.Text = "ALREADY EXISTS. Use 'Save Now / Overwrite' to save as:" + textBox1.Text + ".jpg";
                                this.textBox1.SelectAll();
                            }
                        }
                        catch (DirectoryNotFoundException dnfe)
                        {
                            this.label1.Text = "Set a new save directory, can't find: " + Properties.Settings.Default.saveLocation;
                            this.textBox1.SelectAll();
                        }
                    }
                }
            }
            else
            {
                if (DateTime.Now > this.lastInputDate.AddSeconds(0.1))
                {
                    // Start of new typing
                    char toAdd = this.getChar(e);
                    if (toAdd > 31)
                    {
                        this.scannedText = "" + this.getChar(e);
                        this.lastInputDate = DateTime.Now;
                    }
                }
                else {
                    char toAdd = this.getChar(e);
                    if (toAdd > 31)
                    {
                        this.scannedText += this.getChar(e);
                    }
                }
                if (this.scannedText.Length > 7)
                {
                    // If we get 8 characters in under 0.1 then assume it was a scanner input and wipe the text, replace it with the scanner text so far
                    this.scannedText = "";
                    // Put the cursor at the end
                    //this.textBox1.Select(textBox1.Text.Length, 0);
                    //label2.Text = DateTime.Now.Second + " scanned";
                }
            }
            
        }
        char getChar(KeyEventArgs e)
        {
            int keyValue = e.KeyValue;
            if (!e.Shift && keyValue >= (int)Keys.A && keyValue <= (int)Keys.Z)
                return (char)(keyValue + 32);
            return (char)keyValue;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void setSaveLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.saveLocation = folderBrowserDialog1.SelectedPath;
                refreshImageListPanel();
            }
            this.textBox1.Focus();

        }

        private void panel1_Resize(object sender, EventArgs e)
        {
        }

        private void setFPSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.aTimer.Interval = 200;
            Properties.Settings.Default.fpsDelaySetting = 200;
            this.setupFpsCheckboxes();

        }

        private void fPSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.aTimer.Interval = 100;
            Properties.Settings.Default.fpsDelaySetting = 100;
            this.setupFpsCheckboxes();
        }

        private void fPSToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.aTimer.Interval = 1000;
            Properties.Settings.Default.fpsDelaySetting = 1000;
            this.setupFpsCheckboxes();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.locationToOpenForWindowsPictureViewer != null && this.locationToOpenForWindowsPictureViewer != "")
                {
                    runImageView(this.locationToOpenForWindowsPictureViewer);
                }
                this.textBox1.Focus();
            }
            catch (Exception exc) {
                rockShotErrorHandle(exc);
            }
        }

        private void runImageView(string location)
        {
            String exe = "C:\\Windows\\System32\\rundll32.exe";
            String arguments = "\"C:\\Program Files (x86)\\Windows Photo Viewer\\PhotoViewer.dll\", ImageView_Fullscreen " + location;
            System.Diagnostics.Process.Start(exe, arguments);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }
                double priorWidth = 0;
                double newWidth = 0;

                // Longer than 0.2 sec means its a drag not a click
                if (DateTime.Now.Subtract(this.mouseDownStart).TotalSeconds < 0.2)
                {
                    // Starts with stretch
                    if (pictureBox1.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        priorWidth = pictureBox1.Width;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                        this.imageMultiplier = 1;
                        newWidth = pictureBox1.Image.Width * this.imageMultiplier;
                    }
                    else if (pictureBox1.SizeMode == PictureBoxSizeMode.Normal && this.imageMultiplier < 4)
                    {
                        priorWidth = pictureBox1.Image.Width * this.imageMultiplier;
                        this.imageMultiplier = this.imageMultiplier * 2;
                        newWidth = pictureBox1.Image.Width * this.imageMultiplier;
                    }
                    else
                    {
                        priorWidth = pictureBox1.Image.Width * this.imageMultiplier;
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        this.imageMultiplier = 1;
                        newWidth = pictureBox1.Width;

                    }
                    if (pictureBox1.SizeMode != PictureBoxSizeMode.StretchImage)
                    {
                        double percentChange = newWidth / priorWidth;
                        totalDrag.X = -recentClick.X + (int)(totalDrag.X * percentChange);
                        totalDrag.Y = -recentClick.Y + (int)(totalDrag.Y * percentChange);
                    }
                    else
                    {
                        totalDrag.X = 0;
                        totalDrag.Y = 0;
                    }

                }
            }
            catch (Exception exc) {
                rockShotErrorHandle(exc);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                this.recentClick = new Point(e.X, e.Y);
                if (this.draggedTo.Equals(Point.Empty))
                {
                    this.initDragLoc = new Point(e.X, e.Y);
                }
                this.mouseDownStart = DateTime.Now;
            }
            catch (Exception exc)
            {
                rockShotErrorHandle(exc);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                this.initDragLoc = Point.Empty;
                this.totalDrag = new Point(this.draggedTo.X + this.totalDrag.X, this.draggedTo.Y + this.totalDrag.Y);
                this.draggedTo = Point.Empty;
            }
            catch (Exception exc)
            {
                rockShotErrorHandle(exc);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.initDragLoc.Equals(Point.Empty))
            {
                this.draggedTo = new Point(e.X - this.initDragLoc.X, e.Y - this.initDragLoc.Y);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                System.Drawing.SolidBrush brush1 = new System.Drawing.SolidBrush(System.Drawing.Color.GhostWhite);
                System.Drawing.Graphics formGraphics = this.CreateGraphics();
                e.Graphics.FillRectangle(brush1, new System.Drawing.Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));

                Rectangle destRect = new Rectangle((this.totalDrag.X + this.draggedTo.X), (this.totalDrag.Y + this.draggedTo.Y), pictureBox1.Width, pictureBox1.Height);

                if (pictureBox1.SizeMode == PictureBoxSizeMode.Normal)
                {
                    destRect = new Rectangle((this.totalDrag.X + this.draggedTo.X), (this.totalDrag.Y + this.draggedTo.Y), pictureBox1.Image.Width * this.imageMultiplier, pictureBox1.Image.Height * this.imageMultiplier);
                }

                // Create rectangle for source image.
                Rectangle srcRect = new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
                GraphicsUnit units = GraphicsUnit.Pixel;

                //e.Graphics.DrawImage(pictureBox1.Image, this.totalDrag.X + this.draggedTo.X, this.totalDrag.Y + this.draggedTo.Y);
                e.Graphics.DrawImage(pictureBox1.Image, destRect, srcRect, units);
            }
        }
        private void thumbnailImage_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                PictureBox pb = sender as PictureBox;
                Label lbl = sender as Label;
                if (pb != null)
                {
                    string imageLoc = map[pb];
                    runImageView(imageLoc);
                }
                this.textBox1.Focus();
            }
            catch (Exception exc)
            {
                rockShotErrorHandle(exc);
            }
        }
        private void resetImageButton_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.draggedTo = Point.Empty;
            this.totalDrag = Point.Empty;
            this.textBox1.Focus();
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                this.BackColor = colorDialog1.Color;
                Properties.Settings.Default.backgroundColor = this.BackColor;
                this.textBox1.Focus();
            }
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // Figure out where we are on the main panel instead of the event sender
            // The items that can send to here are the panel and its children
            Control c = sender as Control;
            if (e.Button == MouseButtons.Left)
            {
                Panel p = sender as Panel;
                if (p != null)
                {
                    panelDragStart = new Point(c.Location.X, c.Location.Y + e.Location.Y);
                    panelVsvStart = panel1.VerticalScroll.Value;
                }
                else
                {
                    panelVsvStart = panel1.VerticalScroll.Value;
                    panelDragStart = new Point(c.Parent.Location.X, c.Parent.Location.Y + c.Location.Y + e.Location.Y);
                }
                c.MouseUp += new MouseEventHandler(panel1_MouseUp);
                c.MouseMove += new MouseEventHandler(panel1_MouseMove);
                totalChangeVal = 0;
            }
        }
        void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            c.MouseMove -= new MouseEventHandler(panel1_MouseMove);
            c.MouseUp -= new MouseEventHandler(panel1_MouseUp);
        }

        void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Had issues with flashing during draw if I didn't put a wait in here
            if (DateTime.Now > this.allowMouseMoveAt)
            {
                this.allowMouseMoveAt = DateTime.Now.AddSeconds(0.01);
                // Was initial attempt to control flashing, since it counted holding the mouse down during
                // a scroll to be a mouse move, wasn't hurting anything so I left it
                if (!this.isScrolling)
                {
                    Panel p = sender as Panel;
                    Control c = sender as Control;
                    Point corrected;
                    if (p != null)
                    {
                        corrected = p.Parent.Location;
                    }
                    else
                    {
                        corrected = new Point(c.Location.X, c.Parent.Location.Y + c.Location.Y + e.Location.Y);
                    }
                    int changeVal = panelDragStart.Y - corrected.Y;
                    this.totalChangeVal += changeVal;
                    int newVal = this.panelVsvStart + totalChangeVal;
                    this.isScrolling = true;
                    if (newVal < 0)
                    {
                        newVal = 0;
                        totalChangeVal = 0;
                        panelVsvStart = 0;
                    }
                    if (newVal > panel1.VerticalScroll.Maximum)
                    {
                        newVal = panel1.VerticalScroll.Maximum;
                        totalChangeVal = 0;
                    }
                    panel1.VerticalScroll.Value = newVal;
                    this.isScrolling = false;
                    this.panelDragStart = corrected;
                }
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.Width < 998)
            {
                if (this.panel1.Dock == DockStyle.Right)
                {
                    this.pictureBox1.Width = 800;
                    this.pictureBox1.Height = 472;
                    this.label1.Location = new System.Drawing.Point(0, 750);
                    this.panel1.Dock = DockStyle.Bottom;
                    this.panel1.Size = new System.Drawing.Size(this.Width, 140);
                    this.panel1.Location = new System.Drawing.Point(500, 524);
                    this.button1.Location = new System.Drawing.Point(0, 799);
                    this.button2.Location = new System.Drawing.Point(517, 799);
                    refreshImageListPanel();
                }
            }
            else {
                if (this.panel1.Dock == DockStyle.Bottom)
                {
                    this.pictureBox1.Width = 1040;
                    this.pictureBox1.Height = 613;
                    this.label1.Location = new System.Drawing.Point(267, 97);
                    this.panel1.Dock = DockStyle.Right;
                    this.panel1.Size = new System.Drawing.Size(198, this.Height);
                    this.panel1.Location = new System.Drawing.Point(0, 24);
                    this.button1.Location = new System.Drawing.Point(757, 27);
                    this.button2.Location = new System.Drawing.Point(757, 128);
                    refreshImageListPanel();
                }
            }
 
        }

        private void refreshImageListPanel()
        {
            try
            {
                var sortedFiles = new DirectoryInfo(Properties.Settings.Default.saveLocation).GetFiles()
                                              .OrderBy(f => f.LastWriteTime)
                                              .Reverse()
                                              .ToList();
                string[] dirs = Directory.GetFiles(Properties.Settings.Default.saveLocation, "*.JPG");
                int x = 0;
                int y = 0;
                // Release the grip on the files
                foreach (PictureBox pic in onDiskImages)
                {
                    pic.Dispose();
                }

                // Remove the stuff that's already there
                this.panel1.Controls.Clear();
                foreach (FileInfo sortedFile in sortedFiles)
                {
                    int di = Array.IndexOf(dirs, sortedFile.FullName);
                    if (di > -1)
                    {
                        String img = dirs[di];
                        PictureBox pic = new PictureBox();
                        pic.Image = Image.FromFile(img);
                        pic.Location = new Point(x, y);
                        pic.SizeMode = PictureBoxSizeMode.StretchImage;
                        pic.Height = 98;
                        pic.Width = 173;
                        pic.DoubleClick += new System.EventHandler(this.thumbnailImage_DoubleClick);
                        pic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);

                        this.panel1.Controls.Add(pic);
                        onDiskImages.Add(pic);
                        map.Add(pic, img);
                        Label picLabel = new Label();
                        picLabel.Text = Path.GetFileName(img.Substring(0, img.Length - 4));
                        picLabel.Location = new Point(x, y + pic.Height);
                        picLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
                        this.panel1.Controls.Add(picLabel);

                        if (this.panel1.Dock == DockStyle.Right)
                        {
                            y += pic.Height + picLabel.Height + 2;
                        }
                        else
                        {
                            x += pic.Width + 2;
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException dnfe)
            {
                // Ignore, maybe a disconnect from network drive, etc.
                // User will know there's an issue from the label or from no images appearing on the sidebar
            }

        }

        private void resetCameraImmediateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.resetCam();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void captureOnBarcodeScan_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.captureOnScan = this.captureOnBarcodeScan.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.saveOnScan = this.saveOnBarcodeScan.Checked;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void rockShotErrorHandle(Exception e)
        {
            // For production releases, simply avoid bubbling these to user
        }

    }
}
