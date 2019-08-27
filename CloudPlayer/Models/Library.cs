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
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoSQLite.db3"));
            database.DropTableAsync<Track>().Wait();
            database.CreateTableAsync<Track>().Wait();
        }

        public Task ClearTracks()
        {
            return database.ExecuteAsync("Delete From [Track]");
        }

        public Task<int> SaveTrack(Track track)
        {
            return database.InsertAsync(track);
        }

        public Task<List<Track>> GetTracks()
        {
            return database.QueryAsync<Track>("Select * from [Track]");
        }
    }
}
