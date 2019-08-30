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
            Task<bool> Play(string filePath);
            Task<bool> SetVolume(float left, float right);
            int Stop();
            int Pause();
            void Initialize();
        }
        public async Task<bool> Play(Track track)
        {
            
            string url = await App.OneDrive.GetTrackURL(track.OneDrive_ID);
            return await DependencyService.Get<PlayMusic>().Play(url);
        }

        public async Task Initialize()
        {
            DependencyService.Get<PlayMusic>().Initialize();
            Queue = await App.Library.GetQueue();
        }

        public async Task LoadQueue()
        {
            Queue = await App.Library.GetQueue();
        }

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
