using DB_Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZPLPrint
{
    public partial class Form3 : Form
    {
        INI_Class ic = new INI_Class();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            tbIP.Text = ic.GetLogin_Info("PRINT_IP"); 
        }


        private void Barcode_Print()
        {
            try
            {
                Socket ClientSocket;
                IPEndPoint ipEndPoint;
                string ZplString = "";

                #region [ 바코드리더기 ]   
                ZplString = "^XA" +
                            //용지크기 지정
                            "^PON^PW600" +
                             //용지시작위치
                             //*시작좌표
                             "^LH0012,10" +
                             //*F0(x,y)^Font,가로크기,세로크기^  (++=→ , ++=↓)
                             "^SEE:UHangul.DAT^FS^CW1,E:KFONT3.FNT^CI26^FS" +
                             //외곽라인
                             "^FO0010,0040^GB0600,5,5,B,0^FS" + //상
                             //"^FO0000,0040^GB2,0450,5,5,B,0^FS" + //좌
                             //"^FO0610,0030^GB2,0460,5,5,B,0^FS" + //우
                             //"^FO0010,0460^GB0600,5,5,B,0^FS" + //하
                             
                             //내부 표

                             "^FO0185,0040^GB2,0460,2,2,B,0^FS" + //내용 좌측 세로열 라인
                             //"^FO0185,0135^GB0800,2,2,B,0^FS" + //제품/모델명 라인 나누기

                             "^FO0017,0010^A1N,30,30^FD" + "2021년 스마트공장구축사업(기초)" + "^FS" + // 입고일 Index 좌표
                             //"^FO0200,00^A1N,30,30^FD" + "2021년 스마트공장구축사업(기초)"/*이곳에 데이터 입력*/ + //입고일 데이터 입력부
                             "^FO0010,0100^GB0800,2,2,B,0^FS" +

                             "^FO0017,0060^A1N,30,30^FD" + "과제번호" + "^FS" + // 과제번호 Index 좌표
                             "^FO0197,060^A1N,25,25^FD" + tbNO.Text.Trim() + "^FS" + //과제번호 데이터 입력부
                             "^FO0010,0170^GB0800,2,2,B,0^FS" +

                             "^FO0197,0105^A1N,25,25^FD" + tbProd.Text.Trim() + "^FS" + //제품 데이터 입력부
                             "^FO0010,0380^GB0800,2,2,B,0^FS" +

                             "^FO0017,0122^A1N,30,30^FD" + "제품/모델명" + "^FS" + // 모델명 Index 좌표
                             "^FO0197,0138^A1N,25,25^FD" + tbModelNm.Text.Trim() + "^FS" + //모델명 데이터 입력부
                             "^FO0010,0380^GB0800,2,2,B,0^FS" +

                             "^FO0017,0190^A1N,30,30^FD" + "일련번호" + "^FS" + // 일련번호 Index 좌표
                             "^FO0197,0190^A1N,25,25^FD" + tbSNNO.Text.Trim() + "^FS" + //일련번호 데이터 입력부
                             "^FO0010,0240^GB0800,2,2,B,0^FS" +

                             //"^FO0060,0400^A1N,30,30^FD" + "수량" + "^FS" + // 수량 Index 좌표
                             //"^FO0200,0405^A1N,30,30^FD" + tbQty.Text.Trim() + "^FS" + //수량 데이터 입력부

                             "^XZ";

                // X,Y값을 변수로 설정하고 실햏화면에서 값을 입력했을때, 변경될 수 있도록 만들어 보기

                #endregion
                try
                {
                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ipEndPoint = new IPEndPoint(IPAddress.Parse(tbIP.Text.Trim()), int.Parse("9100"));
                    var result = ClientSocket.BeginConnect(ipEndPoint, null, null);
                    //5000 5초 응답이 없으면 종료
                    bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                    if (success)
                    {
                        byte[] bytePrintData;
                        bytePrintData = Encoding.Default.GetBytes(ZplString);
                        ClientSocket.Send(bytePrintData);
                    }
                    ClientSocket.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int ierror = 0, qty = 0;

                if (int.TryParse(tbQty.Text.Trim().Replace(",", ""), out ierror))
                    qty = int.Parse(tbQty.Text.Trim().Replace(",", ""));

                if (qty > 0)
                {
                    for (int row = 0; row < qty; row++)
                    {
                        Barcode_Print();
                    }
                }
                else
                {
                    MessageBox.Show("수량을 확인해주세요.!!", "도움말");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tbIP_TextChanged(object sender, EventArgs e) 
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
