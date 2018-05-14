using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class RPMController : MonoBehaviour {
    //RPM value;
    private int rpm = 0;

    //serial port
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    private SerialPort serialPort = new SerialPort("COM3", 9600);
#endif

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    private SerialPort serialPort = new SerialPort("/dev/tty.usbmodem1421", 57600);
#endif

    // a thread for geetting rmp
    private Thread rpmThread;
    private bool gameOver = false;


    private int rpmTemp = 0;

    // Use this for initialization
    void Start () {
        serialPort.Open();
        serialPort.Parity = Parity.None;
        serialPort.DataBits = 8;
        serialPort.StopBits = StopBits.One;
        serialPort.ReadTimeout = 1000;

        rpmThread = new Thread(getRPM);
        rpmThread.Start();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void getRPM()
    {
        while (!gameOver)
        {
            int i = serialPort.ReadByte();
            if (i == 10)
            {
                if (rpmTemp >= 100 && rpmTemp <= 1000)
                {
                    rpm = rpmTemp;
                    Debug.Log(rpm);
                }
                rpmTemp = 0;
            }
            else
            {
                rpmTemp = rpmTemp * 10 + i - 48;
            }
        }
    }

    private void OnDestroy()
    {
        gameOver = true;
        serialPort.Close();
        rpmThread.Abort();
    }

    public int RPM()
    {
        return rpm;
    }
}
