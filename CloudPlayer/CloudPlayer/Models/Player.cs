using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CloudPlayer.Models
{
    public class Player
    {
        public List<Queue> Queue { get; set; }
        public interface PlayMusic
        {
            void Play(string filePath);
            Task<bool> SetVolume(float left, float right);
            int Stop();
            int Pause();
        }

        /// <summary>
        ///     Start playing a track by getting the download URL from onedrive and passing it to the native MediaPlayer
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public async Task<bool> Play(Track track)
        {            
            string url = await App.OneDrive.GetTrackURL(track.OneDrive_ID);
            DependencyService.Get<PlayMusic>().Play(url);
            return true;
        }

        /// <summary>
        ///     Load the audio queue from the library
        /// </summary>
        /// <returns></returns>
        public async Task LoadQueue()
        {
            Queue = await App.Library.GetQueue();
        }

        /// <summary>
        ///     Set and syncronize the audio queue with the library queue
        /// </summary>
        /// <param name="tracks"></param>
        /// <param name="shuffle"></param>
        /// <param name="nowPlaying"></param>
        /// <returns></returns>
        public async Task SetQueue(List<Track> tracks, bool shuffle, Track nowPlaying = null)
        {          
            await App.Library.ClearQueue();
            Queue.Clear();
            foreach(Track track in tracks)
            {
                Queue queue = new Queue();
                if (track == nowPlaying)
                {
                    queue.ID = track.ID;
                    queue.Position = 0;
                    queue.NowPlaying = true;
                }
                else
                {
                    queue.ID = track.ID;
                    queue.Position = 0;
                    queue.NowPlaying = false;
                }
                Queue.Add(queue);
            }
            if (shuffle)
                Queue = ShuffleList(Queue);
            await App.Library.AddAllToQueue(Queue);            
        }

        /// <summary>
        ///     IN PROGRESS - Start playing the audio queue
        /// </summary>
        /// <returns></returns>
        public async Task PlayQueueAsync()
        {
            if(Queue.Count == 0)
            {
                
            }
            Queue nowPlaying;

            nowPlaying = Queue.Find(x => x.NowPlaying == true);
            if(nowPlaying == null)
                nowPlaying = Queue[0];

            List<Track> tracks = await App.Library.GetTrack(nowPlaying.ID);
            await Play(tracks[0]);
        }

        /// <summary>
        ///     Used to shuffle the queue
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        private List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
    }
}
