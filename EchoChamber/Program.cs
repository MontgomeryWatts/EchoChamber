using System;
using System.Windows.Forms;
using WMPLib;

namespace EchoChamber
{
    class Program
    {
        private const Int32 HALF_SECOND = 500;

        [STAThread]
        static void Main(string[] args)
        {
            Prompt();
            int echoes = NumberPrompt("Enter the number of times you'd like the song to echo: ");
            int delay = NumberPrompt("Enter the delay between echos in seconds: ");
            string url = getMedia();

            startPlayerThread(url);
            for(int i = 0; i < echoes; i++)
            {
                System.Threading.Thread.Sleep(HALF_SECOND * 2 * delay);
                startPlayerThread(url);
            }

        }

        /**
         * Creates and returns a WindowsMediaPlayer that will play music from the given url
         * and lowers its volume so you keep your eardrums.
         */

        private static WindowsMediaPlayer createPlayer(string url)
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

        private static string getMedia()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Music Files(*.mp3, *.wav)|*.mp3;*.wav";
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

        private static void startPlayerThread(string url)
        {
            new System.Threading.Thread(() =>
            {
                WindowsMediaPlayer player = createPlayer(url);
                player.controls.play();

                //Duration can't be retrieved until song is started.
                System.Threading.Thread.Sleep(HALF_SECOND);
                Int32 duration = (Int32)player.currentMedia.duration * 1000;
                System.Threading.Thread.Sleep(duration);


            }).Start();
        }

        private static int NumberPrompt(string prompt)
        {
            int number;
            Console.WriteLine(prompt);
            if( int.TryParse(Console.ReadLine(), out number))
            {
                return number;
            }
            else
            {
                return NumberPrompt(prompt);
            }
        }
    }
}
