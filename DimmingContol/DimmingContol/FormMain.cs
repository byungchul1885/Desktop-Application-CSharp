﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Bunifu.Framework.UI;

namespace DimmingContol
{
    public partial class FormMain : Form
    {
        private readonly List<Master> MBmaster = new List<Master>();
        private readonly List<DispatcherTimer> ReqInterval = new List<DispatcherTimer>();

        /* 상행-주행 */
        private readonly List<Control> H70AA70AB = new List<Control>();
        private readonly List<Control> H70A870A9 = new List<Control>();
        private readonly List<Control> H709B = new List<Control>();
        private readonly List<Control> H709C = new List<Control>();
        private readonly List<Control> H7081B02 = new List<Control>(); /* button */
        private readonly List<Control> H7081B06 = new List<Control>(); /* button */

        /* 상행-추월 */
        private readonly List<Control> H709A = new List<Control>();
        private readonly List<Control> H709D = new List<Control>();
        private readonly List<Control> H7081B00 = new List<Control>(); /* button */
        private readonly List<Control> H7081B04 = new List<Control>(); /* button */

        /* 하행-추월 */
        private readonly List<Control> H70AE70AF = new List<Control>();
        private readonly List<Control> H70AC70AD = new List<Control>();
        private readonly List<Control> H70A0 = new List<Control>();
        private readonly List<Control> H70A2 = new List<Control>();
        private readonly List<Control> H7081B08 = new List<Control>(); /* button */
        private readonly List<Control> H7081B12 = new List<Control>(); /* button */

        /* 하행-주행 */
        private readonly List<Control> H70A1 = new List<Control>();
        private readonly List<Control> H70A3 = new List<Control>();
        private readonly List<Control> H7081B10 = new List<Control>(); /* button */
        private readonly List<Control> H7081B14 = new List<Control>(); /* button */


        /* 디밍 레벨 */
        private readonly List<string> H73A9 = new List<string>();
        private readonly List<string> H73AA = new List<string>();
        private readonly List<string> H73AB = new List<string>();
        private readonly List<string> H73AC = new List<string>();
        private readonly List<string> H73AD = new List<string>();
        private readonly List<string> H73AE = new List<string>();
        private readonly List<string> H73AF = new List<string>();
        private readonly List<string> H73B0 = new List<string>();
        private readonly List<string> H73B1 = new List<string>();
        private readonly List<string> H73B2 = new List<string>();
        private readonly List<string> H73B3 = new List<string>();
        private readonly List<string> H73B4 = new List<string>();
        private readonly List<string> H73B5 = new List<string>();
        private readonly List<string> H73B6 = new List<string>();
        private readonly List<string> H73B7 = new List<string>();
        private readonly List<string> H73B8 = new List<string>();
        private readonly List<string> H73B9 = new List<string>();
        private readonly List<string> H73BA = new List<string>();
        private readonly List<string> H73BB = new List<string>();
        private readonly List<string> H73BC = new List<string>();
        private readonly List<string> H73BD = new List<string>();

        /* 보수율 */
        private readonly List<string> H73A3 = new List<string>();
        private readonly List<string> H73A4 = new List<string>();
        private readonly List<string> H73A5 = new List<string>();
        private readonly List<string> H73A6 = new List<string>();
        private readonly List<string> H73A7 = new List<string>();
        private readonly List<string> H73A8 = new List<string>();

        /* 운전 모드 */
        private readonly List<string> OpMode = new List<string>();

        /* 연결 버튼 */
        private readonly List<Control> ConnectButton = new List<Control>();

        /* 연결시도 회수 */
        private readonly List<int> reqCnt = new List<int>() { 0, 0, 0 };


        public static event EventHandler DimmLevelValueReceivedFromController;

        public static event EventHandler MaintenanceFactorReceivedFromController;

        public static event EventHandler OpModeReceivedFromController;

        public static event EventHandler OnOffReceivedFromController;

        public FormMain()
        {
            InitializeComponent();

            InitControl();

            FormInputDimmLevel.UserChangedDimmLevelValue += DimmLevelChagnedByUser;

            FormInputMaintenanceFactor.UserChangedMaintenanceFactor += MaintenanceFactorChagnedByUser;

            FormControllerSetup.OpModeButtonClicked += ChangeOpMode;


        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void InitControl()
        {
            dayLampButtonX00.Enabled = false;

            /* 상행-주행 -------------------------------------*/
            /* 외부휘도 */
            H70AA70AB.Add(externalLuminanceX00); /* 제어기 #0 */
            H70AA70AB.Add(externalLuminanceX01); /* 제어기 #1 */
            H70AA70AB.Add(externalLuminanceX02); /* 제어기 #2 */

            /* 내부휘도 */
            H70A870A9.Add(internalLuminanceX00);
            H70A870A9.Add(internalLuminanceX01);
            H70A870A9.Add(internalLuminanceX02);

            /* 주간등 디밍% */
            H709B.Add(dayLampDimmPercentX00);
            H709B.Add(dayLampDimmPercentX01);
            H709B.Add(dayLampDimmPercentX02);

            /* 주간등 디밍 On-Off 버튼 */
            H7081B02.Add(dayLampButtonX00);
            H7081B02.Add(dayLampButtonX01);
            H7081B02.Add(dayLampButtonX02);

            /* 상시등 디밍% */
            //H709C.Add(regularLampDimmPercentX00);
            //H709C.Add(regularLampDimmPercentX01);
            //H709C.Add(regularLampDimmPercentX02);

            H709D.Add(regularLampDimmPercentX00);
            H709D.Add(regularLampDimmPercentX01);
            H709D.Add(regularLampDimmPercentX02);


            /* 상시등 디밍 On-Off 버튼 */
            H7081B06.Add(regularLampButtonX00);
            H7081B06.Add(regularLampButtonX01);
            H7081B06.Add(regularLampButtonX02);


            /* 상행-추월 -------------------------------------*/
            /* 외부휘도 */
            H70AA70AB.Add(externalLuminanceX10); /* 제어기 #0 */
            H70AA70AB.Add(externalLuminanceX11); /* 제어기 #1 */
            H70AA70AB.Add(externalLuminanceX12); /* 제어기 #2 */

            /* 내부휘도 */
            H70A870A9.Add(internalLuminanceX10);
            H70A870A9.Add(internalLuminanceX11);
            H70A870A9.Add(internalLuminanceX12);

            /* 주간등 디밍% */
            H709A.Add(dayLampDimmPercentX10);
            H709A.Add(dayLampDimmPercentX11);
            H709A.Add(dayLampDimmPercentX12);

            /* 주간등 디밍 On-Off 버튼 */
            H7081B00.Add(dayLampButtonX10);
            H7081B00.Add(dayLampButtonX11);
            H7081B00.Add(dayLampButtonX12);

            /* 상시등 디밍% */
            //H709D.Add(regularLampDimmPercentX10);
            //H709D.Add(regularLampDimmPercentX11);
            //H709D.Add(regularLampDimmPercentX12);

            H709C.Add(regularLampDimmPercentX10);
            H709C.Add(regularLampDimmPercentX11);
            H709C.Add(regularLampDimmPercentX12);


            /* 상시등 디밍 On-Off 버튼 */
            H7081B04.Add(regularLampButtonX10);
            H7081B04.Add(regularLampButtonX11);
            H7081B04.Add(regularLampButtonX12);


            /* 하행-추월 -------------------------------------*/
            /* 외부휘도 */
            H70AE70AF.Add(externalLuminanceX20); /* 제어기 #0 */
            H70AE70AF.Add(externalLuminanceX21); /* 제어기 #1 */
            H70AE70AF.Add(externalLuminanceX22); /* 제어기 #2 */

            /* 내부휘도 */
            H70AC70AD.Add(internalLuminanceX20);
            H70AC70AD.Add(internalLuminanceX21);
            H70AC70AD.Add(internalLuminanceX22);

            /* 주간등 디밍% */
            H70A0.Add(dayLampDimmPercentX20);
            H70A0.Add(dayLampDimmPercentX21);
            H70A0.Add(dayLampDimmPercentX22);

            /* 주간등 디밍 On-Off 버튼 */
            H7081B08.Add(dayLampButtonX20);
            H7081B08.Add(dayLampButtonX21);
            H7081B08.Add(dayLampButtonX22);

            /* 상시등 디밍% */
            H70A2.Add(regularLampDimmPercentX20);
            H70A2.Add(regularLampDimmPercentX21);
            H70A2.Add(regularLampDimmPercentX22);

            /* 상시등 디밍 On-Off 버튼 */
            H7081B12.Add(regularLampButtonX20);
            H7081B12.Add(regularLampButtonX21);
            H7081B12.Add(regularLampButtonX22);


            /* 하행-주행 -------------------------------------*/
            /* 외부휘도 */
            H70AE70AF.Add(externalLuminanceX30); /* 제어기 #0 */
            H70AE70AF.Add(externalLuminanceX31); /* 제어기 #1 */
            H70AE70AF.Add(externalLuminanceX32); /* 제어기 #2 */

            /* 내부휘도 */
            H70AC70AD.Add(internalLuminanceX30);
            H70AC70AD.Add(internalLuminanceX31);
            H70AC70AD.Add(internalLuminanceX32);

            /* 주간등 디밍% */
            H70A1.Add(dayLampDimmPercentX30);
            H70A1.Add(dayLampDimmPercentX31);
            H70A1.Add(dayLampDimmPercentX32);

            /* 주간등 디밍 On-Off 버튼 */
            H7081B10.Add(dayLampButtonX30);
            H7081B10.Add(dayLampButtonX31);
            H7081B10.Add(dayLampButtonX32);

            /* 상시등 디밍% */
            H70A3.Add(regularLampDimmPercentX30);
            H70A3.Add(regularLampDimmPercentX31);
            H70A3.Add(regularLampDimmPercentX32);

            /* 상시등 디밍 On-Off 버튼 */
            H7081B14.Add(regularLampButtonX30);
            H7081B14.Add(regularLampButtonX31);
            H7081B14.Add(regularLampButtonX32);

            /* 연결 버튼 */
            ConnectButton.Add(connButtonX0);
            ConnectButton.Add(connButtonX1);
            ConnectButton.Add(connButtonX2);

            for (int i = 0; i < Properties.Settings.Default.NumController; i++)
            {
                /* 디밍 레벨 */
                H73A9.Add("");
                H73AA.Add("");
                H73AB.Add("");
                H73AC.Add("");
                H73AD.Add("");
                H73AE.Add("");
                H73AF.Add("");
                H73B0.Add("");
                H73B1.Add("");
                H73B2.Add("");
                H73B3.Add("");
                H73B4.Add("");
                H73B5.Add("");
                H73B6.Add("");
                H73B7.Add("");
                H73B8.Add("");
                H73B9.Add("");
                H73BA.Add("");
                H73BB.Add("");
                H73BC.Add("");
                H73BD.Add("");

                /* 보수율 */
                H73A3.Add("");
                H73A4.Add("");
                H73A5.Add("");
                H73A6.Add("");
                H73A7.Add("");
                H73A8.Add("");

                /* 운전 모드 */
                OpMode.Add("");

                /* Modbus object */
                MBmaster.Add(new Master());

                /* 타이머 */
                ReqInterval.Add(new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 0, 0, 1000),
                    Tag = i
                });
                ReqInterval[i].Tick += ReqInterval_Tick;
            }

            /* 터널 이름 */
            tunnelLabelX0.Text = Properties.Settings.Default.ControllerName[0];
            tunnelLabelX1.Text = Properties.Settings.Default.ControllerName[1];
            tunnelLabelX2.Text = Properties.Settings.Default.ControllerName[2];

            /* 상행-하행 방면 */
            ascendingDirectionLabel.Text = Properties.Settings.Default.ascendingDirection;
            descendingDirectionLabel.Text = Properties.Settings.Default.descendingDirection;

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            string[] widthHeight = Properties.Settings.Default.WidthHeight.Split('*');

            Width = Int32.Parse(widthHeight[0].Trim());
            Height = Int32.Parse(widthHeight[1].Trim());

            float fontSizeMagnification;
            if (Width <= 1920)
            {
                fontSizeMagnification = 1.0F;
            }
            else if (Width <= 2048)
            {
                fontSizeMagnification = 1.03F;
            }
            else if (Width <= 2560)
            {
                fontSizeMagnification = 1.32F;
            }
            else if (Width <= 2880)
            {
                fontSizeMagnification = 1.52F;
            }
            else if (Width <= 3200)
            {
                fontSizeMagnification = 1.62F;
            }
            else if (Width <= 3840)
            {
                fontSizeMagnification = 1.68F;
            }
            else
            {
                fontSizeMagnification = 1.0F;
            }

            var c = GetAll(this, typeof(BunifuCustomLabel));
            foreach (var label in c)
            {
                if (label.Name != "titleLabel")
                {
                    label.Font = new Font(label.Font.FontFamily, label.Font.Size * fontSizeMagnification, FontStyle.Bold);
                }

                //if (label.BackColor == Color.FromArgb(32, 56, 100))
                //{
                //    label.Text = "";
                //}
            }

            //tunnelLabelX0.Text = Properties.Settings.Default.ControllerName[0];
            //tunnelLabelX1.Text = Properties.Settings.Default.ControllerName[1];
            //tunnelLabelX2.Text = Properties.Settings.Default.ControllerName[2];

            //ascendingDirectionLabel.Text = Properties.Settings.Default.ascendingDirection;
            //descendingDirectionLabel.Text = Properties.Settings.Default.descendingDirection;



            List<Bitmap> images = new List<Bitmap>();

            Bitmap finalImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("main");
            //channelPic.Image = (Image)O; //Set the Image property of channelPic to the returned object as Image

            //Bitmap bitmap0 = (Bitmap)Properties.Resources.ResourceManager.GetObject("main");
            //Bitmap bitmap1 = (Bitmap)Properties.Resources.ResourceManager.GetObject("fire10");
            Bitmap bitmap2 = (Bitmap)Properties.Resources.ResourceManager.GetObject("fire00");
            Bitmap bitmap3 = (Bitmap)Properties.Resources.ResourceManager.GetObject("fire01");
            Bitmap bitmap4 = (Bitmap)Properties.Resources.ResourceManager.GetObject("fire11");
            Bitmap bitmap5 = (Bitmap)Properties.Resources.ResourceManager.GetObject("fire02");
            Bitmap bitmap6 = (Bitmap)Properties.Resources.ResourceManager.GetObject("fire12");


            //images.Add((Bitmap)Properties.Resources.ResourceManager.GetObject("main"));
            images.Add((Bitmap)Properties.Resources.ResourceManager.GetObject("fire10"));
            //images.Add(bitmap2);
            //images.Add(bitmap3);
            //images.Add(bitmap4);
            //images.Add(bitmap5);
            //images.Add(bitmap6);


            using (Graphics g = Graphics.FromImage(finalImage))
            {
                foreach (Bitmap image in images)
                {
                    g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
                }
            }

            mainTLPanel.BackgroundImage = finalImage;
        }

        private void ReqInterval_Tick(object sender, EventArgs e)
        {
            if (sender is DispatcherTimer dt)
            {
                int controllerIdx = (int)dt.Tag;

                //Debug.WriteLine($"ReqInterval_Tick Tag: {controllerIdx}");

                reqCnt[controllerIdx]++;
                if (reqCnt[controllerIdx] > 5)
                {
                    ConnectButton[controllerIdx].Text = "끊어짐";
                    ConnectButton[controllerIdx].BackColor = Color.FromArgb(255, 0, 0);
                }

            }
            else
            {
                Debug.WriteLine($"why");
            }


#if false
            reqCnt++;
            if (reqCnt > 5)
            {
                App.Config.ConnStat = "접속끊김";
                App.Config.ConnStatBackgBrush = Brushes.Red;
                App.Config.ConnStatForgBrush = Brushes.Yellow;
            }
#endif
        }

        private void CloseProgramButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void ScreenSizeButton_Click(object sender, EventArgs e)
        {
            using (var form = new FormInputWidthHeight())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.CurrWidthHeight = Properties.Settings.Default.WidthHeight;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.WidthHeight = form.ReturnValue;
                }
            }
        }

        private void ConnButton_Click(object sender, EventArgs e)
        {
            if (sender is BunifuFlatButton button)
            {
                using (var form = new FormConn())
                {
                    form.StartPosition = FormStartPosition.CenterParent;

                    int buttonIndex = Int32.Parse(button.Name.Remove(0, "connButtonX".Length));

                    Debug.WriteLine($"buttonIndex {buttonIndex}");

                    form.IP = Properties.Settings.Default.IP[buttonIndex];
                    form.SubMask = Properties.Settings.Default.SubMask[buttonIndex];
                    form.Gateway = Properties.Settings.Default.Gateway[buttonIndex];
                    form.Port = Properties.Settings.Default.Port[buttonIndex];
                    form.ControllerName = Properties.Settings.Default.ControllerName[buttonIndex];

                    Properties.Settings.Default.Save();

                    form.ShowDialog();

                    if (form.ButtonAction == "conn")
                    {
                        Properties.Settings.Default.IP[buttonIndex] = form.IP;
                        Properties.Settings.Default.SubMask[buttonIndex] = form.SubMask;
                        Properties.Settings.Default.Gateway[buttonIndex] = form.Gateway;
                        Properties.Settings.Default.Port[buttonIndex] = form.Port;

                        try
                        {
                            reqCnt[buttonIndex] = 0;

                            MBmaster[buttonIndex].Connect(form.IP, ushort.Parse(form.Port));

                            MBmaster[buttonIndex].OnResponseData += new Master.ResponseData(MBmaster_OnResponseData);

                            MBmaster[buttonIndex].OnException += new Master.ExceptionData(MBmaster_OnException);

                            ReqInterval[buttonIndex].Start();

                            RequestNow(buttonIndex);
                        }
                        catch (SystemException error)
                        {
                            MessageBox.Show(error.Message, "Error!!!");
                        }
                    }
                    else if (form.ButtonAction == "close")
                    {
                        MBmaster[buttonIndex].Disconnect();

                        ReqInterval[buttonIndex].Stop();

                        ConnectButton[buttonIndex].Text = "끊어짐";
                        ConnectButton[buttonIndex].BackColor = Color.FromArgb(255, 0, 0);
#if false
                        App.RxValues.TxFrmCnt = App.RxValues.RxFrmCnt = 0;

                        App.Config.ConnStat = "접속끊김";
                        App.Config.ConnStatBackgBrush = Brushes.Red;
                        App.Config.ConnStatForgBrush = Brushes.Yellow;

                        App.RxValues.SetDefaultValue();
#endif
                    }
                }
            }
        }

        private void RequestNow(int controllerIdx)
        {
#if false
            reqCnt++;
            if (reqCnt > 5)
            {
                App.Config.ConnStat = "접속끊김";
                App.Config.ConnStatBackgBrush = Brushes.Red;
                App.Config.ConnStatForgBrush = Brushes.Yellow;
            }
#endif
            reqCnt[controllerIdx]++;
            if (reqCnt[controllerIdx] > 5)
            {
                ConnectButton[controllerIdx].Text = "끊어짐";
                ConnectButton[controllerIdx].BackColor = Color.FromArgb(255, 0, 0);
            }

            ushort ID = Convert.ToUInt16((controllerIdx + 1) * 100);

            Debug.WriteLine($"ID {ID}");

            MBmaster[controllerIdx].ReadHoldingRegister(ID, 1, 0x7080, 32); // 0x7080 ~ 0x709F
        }

        private void MBmaster_OnException(ushort id, byte unit, byte function, byte exception)
        {
            Debug.WriteLine($"MBmaster_OnException: {exception}");

            string exc = "Modbus says error: ";
            switch (exception)
            {
                case Master.excIllegalFunction: exc += "Illegal function!"; break;
                case Master.excIllegalDataAdr: exc += "Illegal data adress!"; break;
                case Master.excIllegalDataVal: exc += "Illegal data value!"; break;
                case Master.excSlaveDeviceFailure: exc += "Slave device failure!"; break;
                case Master.excAck: exc += "Acknoledge!"; break;
                case Master.excGatePathUnavailable: exc += "Gateway path unavailbale!"; break;
                case Master.excExceptionTimeout: exc += "Slave timed out!"; break;
                case Master.excExceptionConnectionLost: exc += "Connection is lost!"; break;
                case Master.excExceptionNotConnected: exc += "Not connected!"; break;
            }

            MessageBox.Show(exc, "Modbus slave exception");
        }

        private void MBmaster_OnResponseData(ushort ID, byte unit, byte function, byte[] values)
        {
            if (ID >= 1000) /* setup response don't care */
            {
                return;
            }

            int quotient = ID / 100;
            int remainder = ID % 100;
            int controllerIdx = quotient - 1;


            if (MBmaster[controllerIdx].Connected == false)
            {
                return;
            }

            Invoke(new Action(() =>
            {
                reqCnt[controllerIdx] = 0;

                ConnectButton[controllerIdx].Text = "연결";
                ConnectButton[controllerIdx].BackColor = Color.FromArgb(0, 176, 80);

            }));

            //reqCnt[controllerIdx] = 0;

            //ConnectButton[controllerIdx].Text = "연결";
            //ConnectButton[controllerIdx].BackColor = Color.FromArgb(0, 176, 80);

            if (remainder == 0)
            {
                Parsing_7080H(values, controllerIdx);
                System.Threading.Thread.Sleep(500);
                MBmaster[controllerIdx].ReadHoldingRegister(Convert.ToUInt16(ID + 1), 1, 0x70A0, 16);
            }
            else if (remainder == 1)
            {
                Parsing_70A0H(values, controllerIdx);
                System.Threading.Thread.Sleep(500);
                MBmaster[controllerIdx].ReadHoldingRegister(Convert.ToUInt16(ID + 1), 1, 0x73A0, 32);
            }
            else if (remainder == 2)
            {
                Parsing_73A0H(values, controllerIdx);
                System.Threading.Thread.Sleep(500);
                MBmaster[controllerIdx].ReadHoldingRegister(Convert.ToUInt16(ID + 1), 1, 0x73C0, 32);
            }
            else if (remainder == 3)
            {
                Parsing_73C0H(values, controllerIdx);
                System.Threading.Thread.Sleep(500);
                MBmaster[controllerIdx].ReadHoldingRegister(Convert.ToUInt16(ID + 1), 1, 0x73E0, 24);
            }
            else if (remainder == 4)
            {
                Parsing_73E0H(values, controllerIdx);
                System.Threading.Thread.Sleep(500);
                MBmaster[controllerIdx].ReadHoldingRegister(Convert.ToUInt16(ID - 4), 1, 0x7080, 32); // 0x7080 ~ 0x709F
            }

#if false
            if (MBmaster.Connected == false)
                return;

            reqCnt = 0;

            App.Config.ConnStat = "정상접속중";
            App.Config.ConnStatBackgBrush = Brushes.Green;
            App.Config.ConnStatForgBrush = Brushes.AntiqueWhite;

            if (remainder == 0)
            {
                Parsing_7080H(values);
                System.Threading.Thread.Sleep(500);
                MBmaster.ReadHoldingRegister(ID + 1, 1, 0x70A0, 16);
            }
            else if (remainder == 1)
            {
                Parsing_70A0H(values);
                System.Threading.Thread.Sleep(500);
                MBmaster.ReadHoldingRegister(ID + 1, 1, 0x73A0, 32);
            }
            else if (remainder == 2)
            {
                Parsing_73A0H(values);
                System.Threading.Thread.Sleep(500);
                MBmaster.ReadHoldingRegister(ID + 1, 1, 0x73C0, 32);
            }
            else if (remainder == 3)
            {
                Parsing_73C0H(values);
                System.Threading.Thread.Sleep(500);
                MBmaster.ReadHoldingRegister(ID + 1, 1, 0x73E0, 24);
            }
            else if (remainder == 4)
            {
                Parsing_73E0H(values);
                System.Threading.Thread.Sleep(500);
                MBmaster.ReadHoldingRegister(ID - 4, 1, 0x7080, 32); // 0x7080 ~ 0x709F
            }
#endif
        }

        private void ControllerSetupButton_Click(object sender, EventArgs e)
        {
            if (sender is BunifuFlatButton button)
            {
                using (var form = new FormControllerSetup())
                {
                    int idx = Int32.Parse(button.Name.Remove(0, "controllerSetupButtonX".Length));

                    string[] DimmLevel = new string[] {"",
                        H73A9[idx], H73AA[idx], H73AB[idx], H73AC[idx], H73AD[idx],
                        H73AE[idx], H73AF[idx], H73B0[idx], H73B1[idx], H73B2[idx],
                        H73B3[idx], H73B4[idx], H73B5[idx], H73B6[idx], H73B7[idx],
                        H73B8[idx], H73B9[idx], H73BA[idx], H73BB[idx], H73BC[idx],
                        H73BC[idx],
                    };

                    string[] MaintenanceFactor = new string[] {"",
                        H73A3[idx], H73A4[idx], H73A5[idx], H73A6[idx], H73A7[idx],
                        H73A8[idx]};

                    /* 아래 순서 중요 */
                    string[] OnOff = new string[] {"",
                        H7081B00[idx].Text, H7081B04[idx].Text, H7081B02[idx].Text, H7081B06[idx].Text,
                        H7081B08[idx].Text, H7081B12[idx].Text, H7081B10[idx].Text, H7081B14[idx].Text,};
                    
                    form.StartPosition = FormStartPosition.CenterParent;

                    form.ControllerIdx = idx;
                    form.ControllerName = Properties.Settings.Default.ControllerName[idx];

                    form.DimLevelValue.Clear();
                    form.DimLevelValue.AddRange(DimmLevel);

                    form.MaintenanceFactor.Clear();
                    form.MaintenanceFactor.AddRange(MaintenanceFactor);

                    form.OpMode = OpMode[idx];

                    form.OnOff.Clear();
                    form.OnOff.AddRange(OnOff);

                    form.ShowDialog();
                }
            }
        }

        private void DimmLevelChagnedByUser(object sender, EventArgs e)
        {
            if (sender is FormInputDimmLevel form)
            {
                byte[] temp = new byte[42];
                int intTemp;
                int controllerIdx = Int32.Parse(form.DimLevelValue[0]);

                intTemp = Int32.Parse(form.DimLevelValue[1]);
                temp[0] = (byte)((intTemp & 0xFF00) >> 8);
                temp[1] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[2]);
                temp[2] = (byte)((intTemp & 0xFF00) >> 8);
                temp[3] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[3]);
                temp[4] = (byte)((intTemp & 0xFF00) >> 8);
                temp[5] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[4]);
                temp[6] = (byte)((intTemp & 0xFF00) >> 8);
                temp[7] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[5]);
                temp[8] = (byte)((intTemp & 0xFF00) >> 8);
                temp[9] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[6]);
                temp[10] = (byte)((intTemp & 0xFF00) >> 8);
                temp[11] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[7]);
                temp[12] = (byte)((intTemp & 0xFF00) >> 8);
                temp[13] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[8]);
                temp[14] = (byte)((intTemp & 0xFF00) >> 8);
                temp[15] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[9]);
                temp[16] = (byte)((intTemp & 0xFF00) >> 8);
                temp[17] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[10]);
                temp[18] = (byte)((intTemp & 0xFF00) >> 8);
                temp[19] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[11]);
                temp[20] = (byte)((intTemp & 0xFF00) >> 8);
                temp[21] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[12]);
                temp[22] = (byte)((intTemp & 0xFF00) >> 8);
                temp[23] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[13]);
                temp[24] = (byte)((intTemp & 0xFF00) >> 8);
                temp[25] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[14]);
                temp[26] = (byte)((intTemp & 0xFF00) >> 8);
                temp[27] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[15]);
                temp[28] = (byte)((intTemp & 0xFF00) >> 8);
                temp[29] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[16]);
                temp[30] = (byte)((intTemp & 0xFF00) >> 8);
                temp[31] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[17]);
                temp[32] = (byte)((intTemp & 0xFF00) >> 8);
                temp[33] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[18]);
                temp[34] = (byte)((intTemp & 0xFF00) >> 8);
                temp[35] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[19]);
                temp[36] = (byte)((intTemp & 0xFF00) >> 8);
                temp[37] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[20]);
                temp[38] = (byte)((intTemp & 0xFF00) >> 8);
                temp[39] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.DimLevelValue[21]);
                temp[40] = (byte)((intTemp & 0xFF00) >> 8);
                temp[41] = (byte)((intTemp & 0x00FF));

                MBmaster[controllerIdx].WriteMultipleRegister(1001, 1, 0x73A9, temp);
            }
        }

        private void MaintenanceFactorChagnedByUser(object sender, EventArgs e)
        {
            if (sender is FormInputMaintenanceFactor form)
            {
                int intTemp;
                int controllerIdx = Int32.Parse(form.MaintenanceFactor[0]);

                byte[] temp = new byte[12];

                intTemp = Int32.Parse(form.MaintenanceFactor[1]);
                temp[0] = (byte)((intTemp & 0xFF00) >> 8);
                temp[1] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.MaintenanceFactor[2]);
                temp[2] = (byte)((intTemp & 0xFF00) >> 8);
                temp[3] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.MaintenanceFactor[3]);
                temp[4] = (byte)((intTemp & 0xFF00) >> 8);
                temp[5] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.MaintenanceFactor[4]);
                temp[6] = (byte)((intTemp & 0xFF00) >> 8);
                temp[7] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.MaintenanceFactor[5]);
                temp[8] = (byte)((intTemp & 0xFF00) >> 8);
                temp[9] = (byte)((intTemp & 0x00FF));

                intTemp = Int32.Parse(form.MaintenanceFactor[6]);
                temp[10] = (byte)((intTemp & 0xFF00) >> 8);
                temp[11] = (byte)((intTemp & 0x00FF));

                MBmaster[controllerIdx].WriteMultipleRegister(1002, 1, 0x73A3, temp);
            }
        }


        private void ChangeOpMode(object sender, EventArgs e)
        {
            if (sender is FormControllerSetup form)
            {
                byte[] temp = new byte[2];
                ushort address;

                BitArray H7083 = new BitArray(16);
                BitArray H7085 = new BitArray(16);

                if (form.OpModeChangeButtonNum == 0)
                {
                    H7083[0] = true;
                    H7083.CopyTo(temp, 0);
                    address = 0x7083;
                }
                else if (form.OpModeChangeButtonNum == 1)
                {
                    H7083[1] = true;
                    H7083.CopyTo(temp, 0);
                    address = 0x7083;
                }
                else
                {
                    H7085[4] = true;
                    H7085.CopyTo(temp, 0);
                    address = 0x7085;
                }

                Array.Reverse(temp);
                MBmaster[form.ControllerIdx].WriteMultipleRegister(1000, 1, address, temp);
            }
        }

        private void Parsing_7080H(byte[] values, int controllerIdx)
        {
            Debug.WriteLine($"Rx 7080H, controllerIdx:{controllerIdx}");

            byte[] temp;

            temp = values.Skip(0).Take(2).ToArray(); Array.Reverse(temp);
            BitArray H7080 = new BitArray(temp); // B1

            temp = values.Skip(2).Take(2).ToArray(); Array.Reverse(temp);
            BitArray H7081 = new BitArray(temp); // B2

#if false
            temp = values.Skip(4).Take(2).ToArray(); Array.Reverse(temp);
            BitArray H7082 = new BitArray(temp); // B3

            App.RxValues.H7080B00 = H7080[0]; // Remote/Local 상태             0:Local, 1:Rem
            App.RxValues.H7080B01 = H7080[1]; // 화재 운전 상태                 1:화재운전

            App.RxValues.H7080B02 = H7080[2]; // 맑음/흐림등조도 자동운전 상태   1:조도운전
            if (App.RxValues.H7080B02)
                App.RxValues.IlLuminanceVisible = Visibility.Visible;

            App.RxValues.H7080B03 = H7080[3]; // 맑음/흐림등휘도 자동운전 상태   1:휘도운전
            if (App.RxValues.H7080B03)
                App.RxValues.LuminanceVisible = Visibility.Visible;

            App.RxValues.H7080B04 = H7080[4]; // 주간등조도/휘도운전모드 상태    1:조도/휘도운전
            App.RxValues.H7080B05 = H7080[5]; // 주간등 시간 운전모드 상태       1:시간운전
            App.RxValues.H7080B06 = H7080[6];  // Remote 수동운전 상태	        1:수동운전
            App.RxValues.H7080B07 = H7080[7];  // Local 수동운전 상태	        1:수동운전
#endif
            //App.RxValues.H7080B00 = H7080[0]; // Remote/Local 상태             0:Local, 1:Rem
            //App.RxValues.H7080B01 = H7080[1]; // 화재 운전 상태                 1:화재운전
            //App.RxValues.H7080B06 = H7080[6];  // Remote 수동운전 상태	        1:수동운전

            string opMode = H7080[0] ? "Remote" : "Local";
            opMode = H7080[6] ? "Remote수동" : opMode;
            OpMode[controllerIdx] = opMode;

            ArrayList al = new ArrayList
            {
                controllerIdx,
                opMode
            };

            OpModeReceivedFromController?.Invoke(al, null);

#if false
            App.RxValues.H7081B00 = H7081[0]; // 상행 추월선 맑음등 상태
            App.RxValues.H7081B01 = H7081[1]; // 상행 주행선 맑음등 상태
            App.RxValues.H7081B02 = H7081[2]; // 상행 추월선 흐림등 상태
            App.RxValues.H7081B03 = H7081[3]; // 상행 주행선 흐림등 상태
            App.RxValues.H7081B04 = H7081[4]; // 상행 추월선 주간(일출일몰)등 상태
            App.RxValues.H7081B05 = H7081[5]; // 상행 주행선 주간(일출일몰)등 상태
            App.RxValues.H7081B06 = H7081[6];  // 상행 심야등 상태
            App.RxValues.H7081B07 = H7081[7];  // 상행 상시등 상태
            App.RxValues.H7081B08 = H7081[8];  // 하행 추월선 맑음등 상태
            App.RxValues.H7081B09 = H7081[9];  // 하행 주행선 맑음등 상태
            App.RxValues.H7081B10 = H7081[10];  // 하행 추월선 흐림등 상태
            App.RxValues.H7081B11 = H7081[11];  // 하행 주행선 흐림등 상태
            App.RxValues.H7081B12 = H7081[12];  // 하행 추월선 주간(일출일몰)등 상태
            App.RxValues.H7081B13 = H7081[13];  // 하행 주행선 주간(일출일몰)등 상태
            App.RxValues.H7081B14 = H7081[14];  // 하행 심야등 상태
            App.RxValues.H7081B15 = H7081[15];  // 하행 상시등 상태
#else
            Invoke(new Action(() =>
            {
                H7081B00[controllerIdx].Text = H7081[0] ? "ON" : "OFF"; // 상행 추월선 맑음등 상태
                H7081B02[controllerIdx].Text = H7081[2] ? "ON" : "OFF"; // 상행 추월선 흐림등 상태
                H7081B04[controllerIdx].Text = H7081[4] ? "ON" : "OFF"; // 상행 추월선 주간(일출일몰)등 상태
                H7081B06[controllerIdx].Text = H7081[6] ? "ON" : "OFF";  // 상행 심야등 상태

                H7081B08[controllerIdx].Text = H7081[8] ? "ON" : "OFF";  // 하행 추월선 맑음등 상태
                H7081B10[controllerIdx].Text = H7081[10] ? "ON" : "OFF";  // 하행 추월선 흐림등 상태
                H7081B12[controllerIdx].Text = H7081[12] ? "ON" : "OFF";  // 하행 추월선 주간(일출일몰)등 상태
                H7081B14[controllerIdx].Text = H7081[14] ? "ON" : "OFF";  // 하행 심야등 상태

                H7081B00[controllerIdx].BackColor = H7081[0] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
                H7081B02[controllerIdx].BackColor = H7081[2] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
                H7081B04[controllerIdx].BackColor = H7081[4] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
                H7081B06[controllerIdx].BackColor = H7081[6] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
                H7081B08[controllerIdx].BackColor = H7081[8] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
                H7081B10[controllerIdx].BackColor = H7081[10] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
                H7081B12[controllerIdx].BackColor = H7081[12] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
                H7081B14[controllerIdx].BackColor = H7081[14] ? Color.FromArgb(0, 176, 80) : Color.FromArgb(255, 0, 0);
            }));

            List<string> li = new List<string>
            {
                controllerIdx.ToString(),

                /* order is important */
                H7081B00[controllerIdx].Text,
                H7081B04[controllerIdx].Text,
                H7081B02[controllerIdx].Text,
                H7081B06[controllerIdx].Text,

                H7081B08[controllerIdx].Text,
                H7081B12[controllerIdx].Text,
                H7081B10[controllerIdx].Text,
                H7081B14[controllerIdx].Text
            };

            OnOffReceivedFromController?.Invoke(li, null);

#endif

#if false
            App.RxValues.H7082B00 = H7082[0]; // 상행 가로등상시/입구부상태
            App.RxValues.H7082B01 = H7082[1]; // 상행 가로등격등/출구부상태
            App.RxValues.H7082B02 = H7082[2]; // 하행 가로등상시/입구부 상태
            App.RxValues.H7082B03 = H7082[3]; // 하행 가로등격등/출구부 상태
            App.RxValues.H7082B04 = H7082[4]; // 교통량 설계값 많음
            App.RxValues.H7082B05 = H7082[5]; // 교통량 설계값 보통
            App.RxValues.H7082B06 = H7082[6];  // 교통량 설계값 적음
            App.RxValues.H7082B07 = H7082[7];  // 교통량 실제값 많음
            App.RxValues.H7082B08 = H7082[8];  // 교통량 실제값 보통
            App.RxValues.H7082B09 = H7082[9];  // 교통량 실제값 적음

            //BitArray H7083 = new BitArray(values.Skip(6).Take(2).ToArray()); // B4
            //BitArray H7084 = new BitArray(values.Skip(8).Take(2).ToArray()); // B5
            //BitArray H7085 = new BitArray(values.Skip(10).Take(2).ToArray()); // B6
            //BitArray H7086 = new BitArray(values.Skip(12).Take(2).ToArray()); // B7
            //BitArray H7087 = new BitArray(values.Skip(14).Take(2).ToArray()); // B8
            //BitArray H7088 = new BitArray(values.Skip(16).Take(2).ToArray()); // B9
            //BitArray H7089 = new BitArray(values.Skip(18).Take(2).ToArray()); // B10
            //BitArray H708A = new BitArray(values.Skip(20).Take(2).ToArray()); // B11

            temp = values.Skip(22).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H708B = BitConverter.ToInt16(temp, 0); // 등기구 총수량

            temp = values.Skip(24).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H708C = BitConverter.ToInt16(temp, 0); // 터널등 수량 (100W)

            temp = values.Skip(26).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H708D = BitConverter.ToInt16(temp, 0); // 터널등 수량 (200W)

            temp = values.Skip(28).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H708E = BitConverter.ToInt16(temp, 0); // 가로등 수량 (100W)

            temp = values.Skip(30).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H708F = BitConverter.ToInt16(temp, 0); // 가로등 수량 (150W)

            temp = values.Skip(32).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7090 = BitConverter.ToInt16(temp, 0); // 가로등 수량 (250W)

            temp = values.Skip(34).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7091 = BitConverter.ToInt16(temp, 0); // 모뎀 불량 총수량

            temp = values.Skip(36).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7092 = BitConverter.ToInt16(temp, 0); // LED 모듈 불량 총수량

            temp = values.Skip(38).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7093 = BitConverter.ToInt16(temp, 0); // SMPS 불량 총수량

            temp = values.Skip(40).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7094 = BitConverter.ToInt16(temp, 0); // 등기구 누전 총 수량

            temp = values.Skip(42).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7095 = BitConverter.ToInt16(temp, 0); // 중계기 고장 총수량

            temp = values.Skip(44).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7096 = BitConverter.ToInt16(temp, 0); // 조명 제어기 장애 총수량

            temp = values.Skip(46).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7097 = BitConverter.ToInt16(temp, 0); // GPS 장애 총수량

            temp = values.Skip(48).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7098 = BitConverter.ToInt16(temp, 0); // 휘도 센서 장애 총수량

            temp = values.Skip(50).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H7099 = BitConverter.ToInt16(temp, 0); // 조도센서 장애 총수량
#endif
            Invoke(new Action(() =>
            {
                temp = values.Skip(52).Take(2).ToArray(); Array.Reverse(temp);
                H709A[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 상행 맑음등디밍출력값 감시

                temp = values.Skip(54).Take(2).ToArray(); Array.Reverse(temp);
                H709B[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 상행 흐림등디밍출력값 감시

                temp = values.Skip(56).Take(2).ToArray(); Array.Reverse(temp);
                H709C[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 상행 주간(일출일몰)등 디밍출력값 감시

                temp = values.Skip(58).Take(2).ToArray(); Array.Reverse(temp);
                H709D[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 상행 심야등디밍출력값 감시
            }));
#if false
            temp = values.Skip(60).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H709E = BitConverter.ToInt16(temp, 0); // 상행 가로등상시등디밍출력값 감시

            temp = values.Skip(62).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H709F = BitConverter.ToInt16(temp, 0); // 상행 가로등격등디밍출력값 감시
#endif
        }

        private void Parsing_70A0H(byte[] values, int controllerIdx)
        {
            byte[] temp, temp1, temp2;

            Invoke(new Action(() =>
            {
                temp = values.Skip(0).Take(2).ToArray(); Array.Reverse(temp);
                H70A0[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 하행 맑음등디밍출력값 감시

                temp = values.Skip(2).Take(2).ToArray(); Array.Reverse(temp);
                H70A1[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 하행 흐림등디밍출력값 감시

                temp = values.Skip(4).Take(2).ToArray(); Array.Reverse(temp);
                H70A2[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 하행 주간(일출일몰)등 디밍출력값 감시

                temp = values.Skip(6).Take(2).ToArray(); Array.Reverse(temp);
                H70A3[controllerIdx].Text = BitConverter.ToInt16(temp, 0).ToString() + " %"; // 하행 심야등디밍출력값 감시
            }));

#if false
            temp = values.Skip(0).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A0 = BitConverter.ToInt16(temp, 0); // 하행 맑음등디밍출력값 감시 

            temp = values.Skip(2).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A1 = BitConverter.ToInt16(temp, 0); // 하행 흐림등디밍출력값 감시

            temp = values.Skip(4).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A2 = BitConverter.ToInt16(temp, 0); // 하행 심야등디밍출력값 감시

            temp = values.Skip(6).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A3 = BitConverter.ToInt16(temp, 0); // 하행 주간(일출일몰)등 디밍출력값 감시

            temp = values.Skip(8).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A4 = BitConverter.ToInt16(temp, 0); // 하행 가로등상시등디밍출력값 감시

            temp = values.Skip(10).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A5 = BitConverter.ToInt16(temp, 0); // 하행 가로등격등디밍출력값 감시

            temp = values.Skip(12).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A6 = BitConverter.ToInt16(temp, 0); // 상행 디밍단계값

            temp = values.Skip(14).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H70A7 = BitConverter.ToInt16(temp, 0); // 하행 디밍단계값
#endif
            Invoke(new Action(() =>
            {
                temp = values.Skip(16).Take(2).ToArray(); Array.Reverse(temp);
                temp1 = values.Skip(18).Take(2).ToArray(); Array.Reverse(temp1);
                temp2 = temp.Concat(temp1).ToArray();
                H70A870A9[controllerIdx].Text = BitConverter.ToSingle(temp2, 0).ToString() + " cd/㎡"; // 상행 외부 조도 센서값 전송

                temp = values.Skip(20).Take(2).ToArray(); Array.Reverse(temp);
                temp1 = values.Skip(22).Take(2).ToArray(); Array.Reverse(temp1);
                temp2 = temp.Concat(temp1).ToArray();
                H70AA70AB[controllerIdx].Text = BitConverter.ToSingle(temp2, 0).ToString() + " cd/㎡"; // 상행 외부 휘도센서값 전송

                temp = values.Skip(24).Take(2).ToArray(); Array.Reverse(temp);
                temp1 = values.Skip(26).Take(2).ToArray(); Array.Reverse(temp1);
                temp2 = temp.Concat(temp1).ToArray();
                H70AC70AD[controllerIdx].Text = BitConverter.ToSingle(temp2, 0).ToString() + " cd/㎡"; // 하행 외부 조도 센서값 전송

                temp = values.Skip(28).Take(2).ToArray(); Array.Reverse(temp);
                temp1 = values.Skip(30).Take(2).ToArray(); Array.Reverse(temp1);
                temp2 = temp.Concat(temp1).ToArray();
                H70AE70AF[controllerIdx].Text = BitConverter.ToSingle(temp2, 0).ToString() + " cd/㎡"; // 하행 외부 휘도센서값 전송
            }));
#if false
            temp = values.Skip(24).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(26).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H70AC70AD = BitConverter.ToSingle(temp2, 0); // 하행 외부 조도 센서값 전송

            temp = values.Skip(28).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(30).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H70AE70AF = BitConverter.ToSingle(temp2, 0); // 하행 외부 휘도센서값 전송
#endif
        }

        private void Parsing_73A0H(byte[] values, int controllerIdx)
        {
            byte[] temp;

            temp = values.Skip(0).Take(2).ToArray(); Array.Reverse(temp);
            BitArray H73A0 = new BitArray(temp);

            temp = values.Skip(2).Take(2).ToArray(); Array.Reverse(temp);
            BitArray H73A1 = new BitArray(temp);

            Invoke(new Action(() =>
            {
                temp = values.Skip(6).Take(2).ToArray(); Array.Reverse(temp);
                H73A3[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); // 맑음등보수율

                temp = values.Skip(8).Take(2).ToArray(); Array.Reverse(temp);
                H73A4[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); // 흐림등보수율

                temp = values.Skip(10).Take(2).ToArray(); Array.Reverse(temp);
                H73A5[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); // 주간(일출일몰)등보수율

                temp = values.Skip(12).Take(2).ToArray(); Array.Reverse(temp);
                H73A6[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); // 심야/상시등보수율

                temp = values.Skip(14).Take(2).ToArray(); Array.Reverse(temp);
                H73A7[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); // 가로등 상시/입구부보수율

                temp = values.Skip(16).Take(2).ToArray(); Array.Reverse(temp);
                H73A8[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); // 가로등격등/출구부보수율

                /* 디밍 레벨 */
                temp = values.Skip(18).Take(2).ToArray(); Array.Reverse(temp);
                H73A9[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString();

                temp = values.Skip(20).Take(2).ToArray(); Array.Reverse(temp);
                H73AA[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(22).Take(2).ToArray(); Array.Reverse(temp);
                H73AB[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(24).Take(2).ToArray(); Array.Reverse(temp);
                H73AC[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(26).Take(2).ToArray(); Array.Reverse(temp);
                H73AD[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(28).Take(2).ToArray(); Array.Reverse(temp);
                H73AE[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(30).Take(2).ToArray(); Array.Reverse(temp);
                H73AF[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(32).Take(2).ToArray(); Array.Reverse(temp);
                H73B0[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(34).Take(2).ToArray(); Array.Reverse(temp);
                H73B1[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(36).Take(2).ToArray(); Array.Reverse(temp);
                H73B2[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(38).Take(2).ToArray(); Array.Reverse(temp);
                H73B3[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(40).Take(2).ToArray(); Array.Reverse(temp);
                H73B4[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(42).Take(2).ToArray(); Array.Reverse(temp);
                H73B5[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(44).Take(2).ToArray(); Array.Reverse(temp);
                H73B6[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(46).Take(2).ToArray(); Array.Reverse(temp);
                H73B7[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(48).Take(2).ToArray(); Array.Reverse(temp);
                H73B8[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(50).Take(2).ToArray(); Array.Reverse(temp);
                H73B9[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(52).Take(2).ToArray(); Array.Reverse(temp);
                H73BA[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(54).Take(2).ToArray(); Array.Reverse(temp);
                H73BB[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(56).Take(2).ToArray(); Array.Reverse(temp);
                H73BC[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;

                temp = values.Skip(58).Take(2).ToArray(); Array.Reverse(temp);
                H73BD[controllerIdx] = BitConverter.ToInt16(temp, 0).ToString(); ;
            }));

            List<string> li2 = new List<string>
            {
                controllerIdx.ToString(),

                H73A3[controllerIdx], H73A4[controllerIdx], H73A5[controllerIdx],
                H73A6[controllerIdx], H73A7[controllerIdx], H73A8[controllerIdx],
            };
            MaintenanceFactorReceivedFromController?.Invoke(li2, null);


            List<string> li = new List<string>
            {
                controllerIdx.ToString(),

                H73A9[controllerIdx], H73AA[controllerIdx], H73AB[controllerIdx],
                H73AC[controllerIdx], H73AD[controllerIdx], H73AE[controllerIdx],
                H73AF[controllerIdx], H73B0[controllerIdx], H73B1[controllerIdx],
                H73B2[controllerIdx], H73B3[controllerIdx], H73B4[controllerIdx],
                H73B5[controllerIdx], H73B6[controllerIdx], H73B7[controllerIdx],
                H73B8[controllerIdx], H73B9[controllerIdx], H73BA[controllerIdx],
                H73BB[controllerIdx], H73BC[controllerIdx], H73BD[controllerIdx],
            };
            DimmLevelValueReceivedFromController?.Invoke(li, null);
#if false
            App.RxValues.H73A0B00 = H73A0[0]; // 통신이상
            App.RxValues.H73A1B00 = H73A1[0]; // CPU이상
            App.RxValues.H73A1B01 = H73A1[1]; // 파워모듈 이상

            //App.RxValues.H7080B01 = H73A1[1]; // 화재 운전 상태

            temp = values.Skip(6).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73A3 = BitConverter.ToInt16(temp, 0); // 맑음등보수율

            temp = values.Skip(8).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73A4 = BitConverter.ToInt16(temp, 0); // 흐림등보수율

            temp = values.Skip(10).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73A5 = BitConverter.ToInt16(temp, 0); // 주간(일출일몰)등보수율

            temp = values.Skip(12).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73A6 = BitConverter.ToInt16(temp, 0); // 심야/상시등보수율

            temp = values.Skip(14).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73A7 = BitConverter.ToInt16(temp, 0); // 가로등 상시/입구부보수율

            temp = values.Skip(16).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73A8 = BitConverter.ToInt16(temp, 0); // 가로등격등/출구부보수율

            temp = values.Skip(18).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73A9 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(20).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73AA = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(22).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73AB = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(24).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73AC = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(26).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73AD = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(28).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73AE = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(30).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73AF = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(32).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B0 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(34).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B1 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(36).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B2 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(38).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B3 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(40).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B4 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(42).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B5 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(44).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B6 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(46).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B7 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(48).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B8 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(50).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73B9 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(52).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73BA = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(54).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73BB = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(56).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73BC = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(58).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73BD = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(60).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73BE = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(62).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73BF = BitConverter.ToInt16(temp, 0);
#endif
        }

        private void Parsing_73C0H(byte[] values, int controllerIdx)
        {
#if false
            byte[] temp, temp1, temp2;

            temp = values.Skip(0).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C0 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(2).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C1 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(4).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C2 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(6).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C3 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(8).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C4 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(10).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C5 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(12).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C6 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(14).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C7 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(16).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C8 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(18).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73C9 = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(20).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73CA = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(22).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73CB = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(24).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73CC = BitConverter.ToInt16(temp, 0);

            temp = values.Skip(26).Take(2).ToArray(); Array.Reverse(temp);
            App.RxValues.H73CD = BitConverter.ToInt16(temp, 0);

            //1단계 - 맑음등1 조도디밍값
            temp = values.Skip(28).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(30).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73CE73CF = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(32).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(34).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73D073D1 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(36).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(38).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73D273D3 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(40).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(42).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73D473D5 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(44).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(46).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73D673D7 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(48).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(50).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73D873D9 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(52).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(54).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73DA73DB = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(56).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(58).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73DC73DD = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(60).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(62).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73DE73DF = BitConverter.ToSingle(temp2, 0);
#endif
        }

        private void Parsing_73E0H(byte[] values, int controllerIdx)
        {
#if false
            byte[] temp, temp1, temp2;

            temp = values.Skip(0).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(2).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73E073E1 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(4).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(6).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73E273E3 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(8).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(10).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73E473E5 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(12).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(14).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73E673E7 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(16).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(18).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73E873E9 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(20).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(22).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73EA73EB = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(24).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(26).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73EC73ED = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(28).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(30).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73EE73EF = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(32).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(34).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73F073F1 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(36).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(38).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73F273F3 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(40).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(42).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73F473F5 = BitConverter.ToSingle(temp2, 0);

            temp = values.Skip(44).Take(2).ToArray(); Array.Reverse(temp);
            temp1 = values.Skip(46).Take(2).ToArray(); Array.Reverse(temp1);
            temp2 = temp.Concat(temp1).ToArray();
            App.RxValues.H73F673F7 = BitConverter.ToSingle(temp2, 0);
#endif
        }
    }
}
