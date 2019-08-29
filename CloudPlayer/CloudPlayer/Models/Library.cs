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
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoSQLite2.db3"));
            database.CreateTableAsync<Track>().Wait();
            database.CreateTableAsync<Artist>().Wait();
            database.CreateTableAsync<Album>().Wait();

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
            return database.QueryAsync<Track>("Select * from [Track]");
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
    }
}
