using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WorkTimeAuto
{
    class Program
    {
        static Stopwatch workTime = new Stopwatch();

        static void SESS(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                Console.WriteLine(System.DateTime.Now + ": SessionLock   - Блокировка сесии");

                workTime.Stop();
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {            
                Console.WriteLine(System.DateTime.Now + ": SessionUnlock - Разблокировка сессии");

                workTime.Start();
            }
        }

        static void Main(string[] args)
        {
            workTime.Start();
            Thread myThread = new Thread(func); //Создаем новый объект потока (Thread)

            myThread.Start(); //запускаем поток

            Start:
            switch (Console.ReadLine())
            {
                case "exit":
                    {
                        myThread.Abort();
                        Process.GetCurrentProcess().Kill();
                        goto Start; break;
                    }
                case "work":
                    {
                        Console.WriteLine(System.DateTime.Now + " Отработано: " + workTime.Elapsed);
                        goto Start; break;
                    }
                default:
                    {
                        goto Start;
                    }
            } 
        }

        static void func()
        {
            SystemEvents.SessionSwitch += SESS;
            Thread.Sleep(100);
        }


    }
}
