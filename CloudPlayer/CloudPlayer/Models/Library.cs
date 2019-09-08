using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlayer.Models
{
    public class Library
    {
        readonly SQLiteAsyncConnection database;

        public Library()
        {
            database = new SQLiteAsyncConnection(Path.Combine(App.LocalStoragePath, "CPLibrary.db3"));
            database.CreateTableAsync<Track>().Wait();
            database.CreateTableAsync<Artist>().Wait();
            database.CreateTableAsync<Album>().Wait();
            database.CreateTableAsync<UserSettings>().Wait();
            database.CreateTableAsync<QueueItem>().Wait();
        }

        #region [Track]
        public Task ClearTracks()
        {
            return database.ExecuteAsync("Delete From [Track]");
        }

        public async Task SaveTrack(Track track)
        {
            await database.InsertAsync(track);
        }

        public Task<List<Track>> GetTracks()
        {
            return database.QueryAsync<Track>("Select * from [Track] Order By [Title]");
        }

        public Task<List<Track>> GetTrack(int? ID)
        {
            return database.QueryAsync<Track>($"Select * from [Track] Where ID = {ID}");
        }

        public Task<List<Track>> GetTracks(Artist artist)
        {
            return database.QueryAsync<Track>($"Select * from [Track] Where Artist_ID = {artist.ID} Order By [Title]");
        }

        public Task<List<Track>> GetTracks(Album album)
        {
            return database.QueryAsync<Track>($"Select * from [Track] Where Album_ID = {album.ID} Order By [Title]");
        }

        public async Task<List<Track>> GetTrackByOneDrive_ID(string OneDrive_ID)
        {
            return await database.QueryAsync<Track>($@"Select * from [Track] where OneDrive_ID = '{OneDrive_ID}'");
        }

        public async Task UpdateTrack(Track track)
        {
            await database.UpdateAsync(track);
        }
        #endregion

        #region [Artist]
        public async Task<List<Artist>> GetArtistByName(string name)
        {
            return await database.QueryAsync<Artist>($@"Select * from [Artist] where Name = '{name}'");
        }

        public async Task<List<Artist>> GetArtists()
        {
            return await database.QueryAsync<Artist>($@"Select * from [Artist]");
        }    

        public async Task SaveArtist(Artist artist)
        {
            await database.InsertAsync(artist);      
        }

        public async Task<List<Artist>> GetArtist(int id)
        {
            return await database.QueryAsync<Artist>($@"Select * from [Artist] where ID = {id}");
        }
        #endregion

        #region [Album]
        public async Task<List<Album>> GetAlbumByTitleAndAlbumArtist(string title, string artistName)
        {
            return await database.QueryAsync<Album>
                ($@"Select [Album].* from [Album] 
                    Inner Join [Artist] on [Album].AlbumArtist_ID = [Artist].ID where [Artist].Name = '{artistName}' and [Album].Title = '{title}'");
        }

        public async Task InsertAlbum(Album album)
        {
            await database.InsertAsync(album);
        }

        public async Task UpdateAlbum(Album album)
        {
            await database.UpdateAsync(album);
        }

        public  async Task<List<Album>> GetAlbum(int? ID)
        {
            return await database.QueryAsync<Album>($"Select * from [Album] Where ID = {ID}");
        }
        #endregion

        #region Settings

        public async Task SaveSettings(UserSettings settings)
        {
            await database.ExecuteAsync("Delete From [UserSettings]");
            await database.InsertAsync(settings);
        }

        public async Task<UserSettings> GetSettings()
        {
            List<UserSettings> settingsList = await database.QueryAsync<UserSettings>("Select * From [UserSettings]");
            if (settingsList.Count > 0)
                return settingsList[0];
            else
                return new UserSettings();
        }

        #endregion

        #region Queue

        public async Task<List<QueueItem>> GetQueueItems()
        {
            return await database.QueryAsync<QueueItem>("Select * From [QueueItem]");
        }

        public async Task AddToQueue(QueueItem queue)
        {
            await database.InsertAsync(queue);
        }

        public async Task AddAllToQueue(List<QueueItem> queue)
        {
            await database.InsertAllAsync(queue, true);
        }


        public async Task ClearQueue()
        {
            await database.ExecuteAsync("Delete From [QueueItem]");
        }

        #endregion

        public async Task RecreateTables()
        {         
            database.DropTableAsync<Track>().Wait();
            database.DropTableAsync<Artist>().Wait();
            database.DropTableAsync<Album>().Wait();
            database.CreateTableAsync<Track>().Wait();
            database.CreateTableAsync<Artist>().Wait();
            database.CreateTableAsync<Album>().Wait();
        }


    }
}
