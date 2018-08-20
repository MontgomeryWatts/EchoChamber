using System;
using System.Threading;
using System.Windows.Forms;
using WMPLib;

namespace EchoChamber
{
    class Program
    {
        private const int HALF_SECOND = 500;
        private const int ONE_SECOND = HALF_SECOND * 2;
        private const string FILE_FILTER = "Music Files(*.mp3, *.wav)|*.mp3;*.wav";

        [STAThread]
        static void Main(string[] args)
        {
            Prompt();
            int echoes = IntPrompt("Enter the number of times you'd like the song to echo: ");
            int  delay = (int)(DoublePrompt("Enter the delay between echos in seconds: ") * ONE_SECOND);
            string url = GetMedia();

            StartPlayerThread(url);
            for(int i = 0; i < echoes; i++)
            {
                Thread.Sleep(delay);
                StartPlayerThread(url);
            }

        }

        /**
         * Creates and returns a WindowsMediaPlayer that will play music from the given url
         * and lowers its volume so you keep your eardrums intact.
         */

        private static WindowsMediaPlayer CreatePlayer(string url)
        {
            WindowsMediaPlayer player = new WindowsMediaPlayer();
            player.URL = url;
            player.settings.volume = 10;
            return player;
        }

        /**
         * Opens FileExplorer so that the user may select a music file to play.
         * Returns the path to the chosen file.
         */

        private static string GetMedia()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = FILE_FILTER;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }
            return "";
        }

        private static void Prompt()
        {
            Console.WriteLine("EchoChamber is a program that selects a music file, and then echoes it a specified number of times with a specified delay.");
        }

        /**
         * Creates and starts a Thread that will play the media at a given url until it is over .
         */

        private static void StartPlayerThread(string url)
        {
            new Thread(() =>
            {
                WindowsMediaPlayer player = CreatePlayer(url);
                player.controls.play();

                //Duration can't be retrieved until song is started.
                Thread.Sleep(HALF_SECOND);
                Int32 duration = (Int32)player.currentMedia.duration * ONE_SECOND;
                Thread.Sleep(duration);

            }).Start();
        }

        private static int IntPrompt(string prompt)
        {
            int number;
            Console.WriteLine(prompt);
            if( int.TryParse(Console.ReadLine(), out number))
            {
                return number;
            }
            else
            {
                return IntPrompt(prompt);
            }
        }

        private static double DoublePrompt(string prompt)
        {
            double number;
            Console.WriteLine(prompt);
            if (double.TryParse(Console.ReadLine(), out number))
            {
                return number;
            }
            else
            {
                return DoublePrompt(prompt);
            }
        }
    }
}
