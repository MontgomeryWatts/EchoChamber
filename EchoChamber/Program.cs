using System;
using WMPLib;

namespace EchoChamber
{
    class Program
    {
        private const string URL = @"C:\Users\Montgomery\Music\Isaiah Rashad - Pieces of a Kid\02 - Hii (Fuck Love).mp3";
        private const Int32 HALF_SECOND = 500;

        static void Main(string[] args)
        {

            startPlayerThread(URL);
            System.Threading.Thread.Sleep(HALF_SECOND * 2);
            startPlayerThread(URL);


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
    }
}
