using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CloudPlayer.Models
{
    public class Player
    {
        public const string StatePlaying = "playing";
        public const string StateStopped = "stopped";
        public const string StatePaused = "paused";

        public List<QueueItem> Queue { get; set; }
        public bool RepeatAll { get; set; } = true;
        public string PlayerState { get; set; }

        public Player()
        {
            DependencyService.Get<PlayMusic>().playbackCompleted += PlaybackCompleted;
        }

        private void PlaybackCompleted(object sender, EventArgs e)
        {
            Next().Wait();
        }

        public interface PlayMusic
        {
            void Play(string filePath, int position);
            Task<bool> SetVolume(float left, float right);
            int Stop();
            int Pause();

            event EventHandler playbackCompleted;
            
        }

        /// <summary>
        ///     Start playing a track by getting the download URL from onedrive and passing it to the native MediaPlayer
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public async Task<bool> Play(Track track, int postition = 0)
        {            
            string url = await App.OneDrive.GetTrackURL(track.OneDrive_ID);
            DependencyService.Get<PlayMusic>().Play(url, postition);
            PlayerState = StatePlaying;
            return true;
        }

        public async Task<bool> Play()
        {
            QueueItem queueItem = await GetNowPlaying();
           
            string url = await App.OneDrive.GetTrackURL((await queueItem.GetTrack()).OneDrive_ID);
            DependencyService.Get<PlayMusic>().Play(url, queueItem.Position);
            PlayerState = StatePlaying;
            return true;
        }

        /// <summary>
        ///     Load the audio queue from the library
        /// </summary>
        /// <returns></returns>
        public async Task LoadQueue()
        {
            Queue = await App.Library.GetQueueItems();
        }

        public async Task SetQueue(List<Track> tracks, int nowPlaying, int postition)
        {
            Queue = new List<QueueItem>();
            for(int i = 0; i < tracks.Count; i++)
            {
                QueueItem queueItem = new QueueItem();
                queueItem.track_ID = tracks[i].ID;
                queueItem.AlbumArtPath = await GetAlbumArtPath(tracks[i].Album_ID);
                if (i == nowPlaying)
                {
                    queueItem.NowPlaying = true;
                    queueItem.Position = postition;                    
                }
                else
                {
                    queueItem.NowPlaying = false;
                    queueItem.Position = 0;
                }
                Queue.Add(queueItem);
            }
        }

        public async Task SetQueue(List<Track> tracks)
        {
            await SetQueue(tracks, 0, 0);
        }

        public async Task SetQueue()
        {
            await SetQueue(await App.Library.GetTracks());
        }

        public async Task Next()
        {
            QueueItem nowPlaying = await GetNowPlaying();
            int i = Queue.IndexOf(nowPlaying);
            Queue[i].NowPlaying = false;
            Queue[i].Position = 0;
            if (Queue.Count <= i + 1)
            {
                if (RepeatAll)
                    Queue[0].NowPlaying = true;
                else
                    await Stop();
            }
            else
                Queue[i + 1].NowPlaying = true;
            if(PlayerState == StatePlaying)
                await Play();
        }

        public async Task Previous()
        {
            QueueItem nowPlaying = await GetNowPlaying();
            int i = Queue.IndexOf(nowPlaying);
            Queue[i].NowPlaying = false;
            Queue[i].Position = 0;
            if (i == 0)
            {
                if (RepeatAll)
                    Queue[Queue.Count - 1].NowPlaying = true;
                else
                    await Stop();
            }
            else
                Queue[i - 1].NowPlaying = true;
            if (PlayerState == StatePlaying)
                await Play();
        }

        public async Task Stop()
        {
            DependencyService.Get<PlayMusic>().Stop();
            QueueItem nowPlaying = await GetNowPlaying();
            int i = Queue.IndexOf(nowPlaying);
            Queue[i].Position = 0;
            PlayerState = StateStopped;
        }

        public async Task Pause()
        {
            int postition = DependencyService.Get<PlayMusic>().Pause();
            QueueItem nowPlaying = await GetNowPlaying();
            int i = Queue.IndexOf(nowPlaying);
            Queue[i].Position = postition;
            PlayerState = StatePaused;
        }

        public async Task<QueueItem> GetNowPlaying()
        {
            QueueItem nowPlaying;
            if(Queue != null && Queue.Count > 0)
            {
                nowPlaying = Queue.Find(x => x.NowPlaying);
                if(nowPlaying == null)
                {
                    nowPlaying = Queue[0];
                }
                return nowPlaying;
            }
            else
            {
                await SetQueue();
                return Queue[0];
            }
        }

        public async Task<string> GetAlbumArtPath(int id)
        {
            List<Album> albums = await App.Library.GetAlbum(id);
            if (albums.Count > 0)
                return albums[0].ArtworkFile;
            else
                return "1.jpeg";
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

        public bool IsPlaying()
        {
            return PlayerState == StatePlaying;
        }

        public bool IsPaused()
        {
            return PlayerState == StatePaused;
        }

        public bool IsStopped()
        {
            return PlayerState == StateStopped;
        }
    }
}
