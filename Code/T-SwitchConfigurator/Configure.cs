﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;

namespace T_SwitchConfigurator
{
    class Configure
    {
        private string _path;
        private string _userName;
        private string _password;
        private SerialConfiguration _Tswitch;
        private string _switchIP;

        protected Configure(string port, string path) 
        {
            _path = path;
            _Tswitch = new SerialConfiguration(port);
            _userName = XMLparser.switchUserName;
            _password = XMLparser.switcPassword;
        }

        public int Init()
        {
            int resVal;
            resVal = _Tswitch.init();

            return resVal;

        }

        protected virtual int RunScript(int val,string type, bool router)
        {
            int resVal = 0;
            string line, rcv = null;
            string[] tokens;
            _path = GetFilePath(type, router);
            StreamReader script = File.OpenText(_path);

            if (_Tswitch.Open() == -1)
                return -1;

            while ((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace(type, val.ToString());
               _Tswitch.SendData(line);
                Thread.Sleep(200);

                //Check configured value
                tokens = line.Split(' ');
                _Tswitch.SendData(tokens[0]);
                _Tswitch.ReadData(out rcv,"");
         
            }
            script.Close();
            _Tswitch.Close();
            return resVal;
        }

        protected int LogIn()
        {
            int isLogIn = 0;
            string rcv;

            _Tswitch.Flush();
            _Tswitch.SendData("\n");
            Thread.Sleep(200);
            _Tswitch.ReadData(out rcv, "");
            if (rcv != "Username: ")
                return -1;
            _Tswitch.SendData(_userName);
            Thread.Sleep(200);
            _Tswitch.ReadData(out rcv, _userName);
            if (rcv != "Password: ")
                return -1;
            _Tswitch.SendData(_password);
            Thread.Sleep(200);
            _Tswitch.ReadData(out rcv, "");
            if (rcv != "Switch#")
                return -1;

            return isLogIn;
        }

        protected bool SaveSettings()
        {
            bool isSaved = false;
            string rcv;
            UInt16 attempts = 0;

            while ((attempts < 3) && (!isSaved))
            {
                _Tswitch.SendData("WR");
                _Tswitch.SendData("Y");
                //_Tswitch.ReadData(out rcv);

                //if (rcv == "Copy Succeded")
                //    isSaved = true;

                attempts++;
            }

            return isSaved;
        }

        protected int VerifyConfig()
        {
            Ping p = new Ping();
            PingReply pingState;

            pingState = p.Send(_switchIP);

            if (pingState.Status.ToString().Equals("Success"))
                return 0;
            return -1;
        }

        private string GetFilePath(string type, bool router)
        {
            string scriptPath = _path;

            switch(type)
            {
                case "MN":
                    if (router)
                        scriptPath = scriptPath + "\\" + Switch.routeMN;
                    else
                        scriptPath = scriptPath + "\\" + Switch.switchMN;
                    break;
                case "BN":
                    if (router)
                        scriptPath = scriptPath + "\\" + Switch.routeBN;
                    else
                        scriptPath = scriptPath + "\\" + Switch.switchBN;
                    break;
            }

            return scriptPath;
        }
    }
}
