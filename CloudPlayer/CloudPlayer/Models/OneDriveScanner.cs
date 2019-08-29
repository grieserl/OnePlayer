using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace CloudPlayer.Models
{
    class OneDrive
    {
        string accessToken { get; set; }
        
        public static IPublicClientApplication PCA = null;
        public static string[] Scopes = { "User.Read", "files.readwrite.all" };
        public static string ClientID = "e3cd9192-2df0-4636-8a9a-49810911e671";
        public static GraphServiceClient GraphClient;



        public OneDrive()
        {
            PCA = PublicClientApplicationBuilder.Create(ClientID)
                .WithRedirectUri($"msal{ClientID}://auth")
                .Build();
        }

        public async Task GetToken()
        {


            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await PCA.GetAccountsAsync();
            try
            {
                IAccount firstAccount = accounts.FirstOrDefault();
                authResult = await PCA.AcquireTokenSilent(Scopes, firstAccount)
                                      .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                try
                {
                    authResult = await PCA.AcquireTokenInteractive(Scopes).WithParentActivityOrWindow(App.ParentWindow).ExecuteAsync();
                    accessToken = authResult.AccessToken;
                }
                catch (Exception ex2)
                {

                }
            }

            GraphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                        async (request) =>
                        {
                            // Add the access token to the "Authorization" header
                            request.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                        }
                     ));
        
        }

        public async Task scanDriveAsync()
        {
            try
            {


                IDriveItemChildrenCollectionPage driveItems = await GraphClient.Me.Drive.Root.ItemWithPath("Music/").Children.Request().GetAsync();           

               
                
                
                foreach (var item in driveItems)
                {
                    await ScanFolder(item.ParentReference.Path + "/" + item.Name + "/");
                }
            }
            catch(Exception e)
            {

            }
        }

        public async Task ScanFolder(string path)
        {
            IDriveItemChildrenCollectionPage driveItems = await GraphClient.Me.ItemWithPath(path).Children.Request().GetAsync();
            foreach (var item in driveItems)
            {
                DateTimeOffset offset = new DateTimeOffset(new DateTime(2019, 02, 28));
                Debug.WriteLine(path);
                if (item.Folder != null)
                    await ScanFolder(item.ParentReference.Path + "/" + item.Name + "/");
                
                else if (item.LastModifiedDateTime > offset)
                    await SaveTrackToLibrary(item);                
            }
        }

        public async Task SaveTrackToLibrary(DriveItem item)
        {
            try
            {
                List<string> audioExtensions = new List<string>();
                audioExtensions.Add(".flac");
                audioExtensions.Add(".mp3");
                audioExtensions.Add(".m4a");

                object downloadURL = new object();
                if (audioExtensions.Contains(item.Name.Substring(item.Name.LastIndexOf("."), item.Name.Length - item.Name.LastIndexOf("."))))
                {
                    item.AdditionalData?.TryGetValue(@"@microsoft.graph.downloadUrl", out downloadURL);
                    PartialHTTPStream httpResponseStream = new PartialHTTPStream(downloadURL.ToString(), 100000);
                    TagLib.Tag tag = AudioTagHelper.FileTagReader(httpResponseStream, "test" + item.Name.Substring(item.Name.LastIndexOf("."), item.Name.Length - item.Name.LastIndexOf(".")));


                    Track track = new Track();
                    track.Title = tag.Title;
                    track.Year = (int)tag.Year;
                    track.OneDrive_ID = item.Id;
                    track.FileName = item.Name;
                    track.lastUpdate = item.LastModifiedDateTime;

                    List<Track> tracks = (await App.Library.GetTrackByOneDrive_ID(item.Id));
                    if (tracks.Count() == 0 || tracks[0].lastUpdate < track.lastUpdate)
                    {
                        List<Artist> artists = await App.Library.GetArtistByName(tag.Artists.First());
                        if (artists.Count() == 0)
                        {
                            Artist artist = new Artist();
                            artist.Name = tag.Artists.First();
                            await App.Library.SaveArtist(artist);
                            track.Artist_ID = artist.ID;
                        }
                        else
                            track.Artist_ID = artists[0].ID;

                        List<Album> albums = await App.Library.GetAlbumByTitleAndAlbumArtist(tag.Album, (tag.AlbumArtists == null || tag.AlbumArtists.Length == 0 ? tag.Artists[0] : tag.AlbumArtists[0]));
                        if (albums.Count() == 0)
                        {
                            Album album = new Album();
                            Artist albumArtist = new Artist();
                            List<Artist> albumArtists = await App.Library.GetArtistByName((tag.AlbumArtists == null || tag.AlbumArtists.Length == 0 ? tag.Artists[0] : tag.AlbumArtists[0]));
                            if (albumArtists.Count() == 0)
                            {
                                albumArtist.Name = tag.AlbumArtists.First();
                                await App.Library.SaveArtist(albumArtist);
                                album.AlbumArtist_ID = albumArtist.ID;
                            }
                            else
                            {
                                album.AlbumArtist_ID = albumArtists.First().ID;
                            }
                            album.Title = tag.Album;
                            await App.Library.SaveAlbum(album);
                            track.Album_ID = album.ID;

                        }
                        else
                            track.Album_ID = albums[0].ID;

                        if (tracks.Count() > 0 && tracks[0].lastUpdate < track.lastUpdate)
                        {
                            track.ID = tracks[0].ID;
                            await App.Library.UpdateTrack(track);
                        }
                        else
                            await App.Library.SaveTrack(track);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
           


        public async Task<string> GetTrackURL(string ID)
        {
            DriveItem item = await GraphClient.Me.Drive.Items[ID].Request().GetAsync();
            Object downloadURL = "";
            if(item != null)
            {
               item.AdditionalData?.TryGetValue(@"@microsoft.graph.downloadUrl", out downloadURL);
            }
            return downloadURL.ToString();
        }
    }
}
