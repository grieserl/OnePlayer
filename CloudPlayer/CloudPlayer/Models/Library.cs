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

        public Library(String dbPath)
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CPLibrary.db3"));
            database.CreateTableAsync<Track>().Wait();
            database.CreateTableAsync<Artist>().Wait();
            database.CreateTableAsync<Album>().Wait();
            database.CreateTableAsync<UserSettings>().Wait();
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

        #endregion

        #region [Album]
        public async Task<List<Album>> GetAlbumByTitleAndAlbumArtist(string title, string artistName)
        {
            return await database.QueryAsync<Album>
                ($@"Select [Album].* from [Album] 
                    Inner Join [Artist] on [Album].AlbumArtist_ID = [Artist].ID where [Artist].Name = '{artistName}' and [Album].Title = '{title}'");
        }

        public async Task SaveAlbum(Album album)
        {
            await database.InsertAsync(album);
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
    }
}
