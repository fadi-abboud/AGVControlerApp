using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.DirectInput;
using Lego.Ev3.Desktop;
using Lego.Ev3.Core;

namespace AGVControlerApp
{
    public partial class Form1 : Form
    {
        PathFinding find;
        public Form1()
        {
            InitializeComponent();

            GetSticks();
            sticks = GetSticks();
            timer1.Enabled = true;



        }

        //------------------------------------------------Brick Setup--------------------------------------------------------------------------------
        Brick brick;

        private async void ButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                brick = new Brick(new BluetoothCommunication(comboBoxPort.Text));
                Console.WriteLine(comboBoxPort.Text);
                brick.BrickChanged += brick_brickChanged;
                await brick.ConnectAsync();
                await brick.DirectCommand.PlayToneAsync(50, 1000, 200);
                await brick.DirectCommand.SetMotorPolarityAsync(OutputPort.A | OutputPort.D, Polarity.Backward);
                await brick.DirectCommand.StopMotorAsync(OutputPort.All, false); //Uncomment this only if the moters start moving
                brick.Ports[InputPort.One].SetMode(ColorMode.Color);
                find = new PathFinding(brick);
                timer2.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Connection error", "Please choose the correct Port and try again! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void brick_brickChanged(object sender, BrickChangedEventArgs e)
        {
            //Console.WriteLine("Brick changed!");                
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
        }


        private void Timer2_Tick(object sender, EventArgs e)
        {
            find.solve();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }

        //-------------------------------------------------Joystick Setup-----------------------------------------------------------------------------

        DirectInput Input = new DirectInput();
        SlimDX.DirectInput.Joystick stick;
        Joystick[] sticks;
        int yValue = 0;
        int xValue = 0;

        public Joystick[] GetSticks()
        {
            List<SlimDX.DirectInput.Joystick> sticks = new List<Joystick>();
            foreach (DeviceInstance device in Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    stick = new SlimDX.DirectInput.Joystick(Input, device.InstanceGuid);
                    stick.Acquire();

                    foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                        {
                            stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-5, 5);
                        }
                        sticks.Add(stick);
                    }
                }
                catch (DirectInputException)
                {

                }
            }
            return sticks.ToArray();
        }

        void stickHandle(Joystick stick, int id)
        {
            JoystickState state = new JoystickState();
            state = stick.GetCurrentState();

            yValue = -state.Y;
            xValue = state.X;

            bool[] buttons = state.GetButtons();

            CoorDisplay(xValue, yValue);

            if (buttons[id])
            {
                Console.WriteLine("Button " + id + " pressed!");
            }

        }

        public void CoorDisplay(int posx, int posy)
        {
            Console.WriteLine(posx + ", " + posy);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Joystick[] joystick = GetSticks();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < sticks.Length; i++)
            {
                stickHandle(sticks[i], i);
            }
        }


    }
}
