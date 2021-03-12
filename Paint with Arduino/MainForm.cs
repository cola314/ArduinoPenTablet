using Paint_with_Arduino.Arduino;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint_with_Arduino
{
    public partial class MainForm : Form
    {
        private ArduinoPaintReceiver _arduino = new ArduinoPaintReceiver();

        private Graphics _canvas;
        private Bitmap _bitmap = new Bitmap(240, 320);  

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _arduino.DataReceived += PointReceived;

            foreach(var portName in SerialPort.GetPortNames())
            {
                listView1.Items.Add(portName);
            }

            _bitmap = new Bitmap(240, 320);
            _canvas = Graphics.FromImage(_bitmap);
            _canvas.Clear(Color.White);
        }

        private void PointReceived(object sender, PointEventArgs e)
        {
            _canvas.FillEllipse(new SolidBrush(e.Color), e.X, e.Y, 6, 6);
            pictureBox1.Image = _bitmap;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _arduino.Dispose();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(!string.IsNullOrEmpty(e.Node.Name))
                _arduino.Connect(e.Node.Name);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                _arduino.Connect(listView1.SelectedItems[0].Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _canvas.Clear(Color.White);
            pictureBox1.Image = _bitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _bitmap.Save(dialog.FileName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("저장에 실패했습니다\n" + ex.Message);
                }
            }
        }
    }
}
