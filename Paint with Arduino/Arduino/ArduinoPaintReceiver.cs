using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace Paint_with_Arduino.Arduino
{
    public class ArduinoPaintReceiver : IDisposable
    {
        public delegate void PointEventHandler(object sender, PointEventArgs args);

        private SerialPort _port;
        private Thread _thread;
        private bool _running;
        public event PointEventHandler DataReceived;

        public ArduinoPaintReceiver()
        {
        }

        private void Run()
        {
            while(_running)
            {
                try
                {
                    byte start = (byte)_port.ReadByte();
                    if (start == 0xff)
                    {
                        int x = _port.ReadByte();
                        x += (0xff) * _port.ReadByte();

                        int y = _port.ReadByte();
                        y += (0xff) * _port.ReadByte();

                        int color = _port.ReadByte();
                        color += (0xff) * _port.ReadByte();
                        
                        Trace.WriteLine($"x : {x} y : {y} color : {color}");

                        int G = (color & 0xff);
                        int R = (color & 0xf800) >> 8;
                        int B = (color & 0xff0000) >> 16;

                        DataReceived(this, new PointEventArgs(x, y, Color.FromArgb(R, G, B)));
                    }
                }
                catch(Exception ex)
                {
                    Trace.WriteLine("Error " + ex.Message + "\n" + ex.StackTrace.ToString());
                    return; 
                }
            }
        }

        public void Connect(string com)
        {
            if (_port != null)
                Close();

            try
            {
                _port = new SerialPort(com);
                _port.Open();

                _running = true;
                _thread = new Thread(Run);
                _thread.Start();
                MessageBox.Show(com + " 포트에 연결되었습니다");
            }
            catch(Exception ex)
            {
                MessageBox.Show("포트에 연결할 수 없습니다.");
            }
        }

        public void Close()
        {
            _running = false;
            
            if(_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }

            if(_port != null)
            {
                _port.Close();
                _port = null;
            }
        }

        public void Dispose()
        {
            if(_port != null)
            {
                _port.Dispose();
            }
        }
    }
}