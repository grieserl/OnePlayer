using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlayer.Models
{
    class OneDriveScanner
    {
        string accessToken { get; set; }
        
        public static IPublicClientApplication PCA = null;
        public static string[] Scopes = { "User.Read", "files.readwrite.all" };
        public static string ClientID = "e3cd9192-2df0-4636-8a9a-49810911e671";
        public static GraphServiceClient GraphClient;



        public OneDriveScanner()
        {
            PCA = PublicClientApplicationBuilder.Create(ClientID)
                .WithRedirectUri($"msal{ClientID}://auth")
                .Build();
        }

        public async Task<bool> GetToken()
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

            return true;
        }

        public void scanDrive()
        {
            //IDriveItemChildrenCollectionPage driveItems = await App.GraphClient.Me.Drive.Root.ItemWithPath("/Music/Audioslave/Audioslave").Children.Request().GetAsync();

            //List<string> audioExtensions = new List<string>();
            //audioExtensions.Add(".flac");
            //audioExtensions.Add(".mp3");
            //List<Tag> fileTags = new List<Tag>();
            //Library library = new Library("/");
            //foreach (var item in driveItems)
            //{
            //    object downloadURL = new object();
            //    if (audioExtensions.Contains(item.Name.Substring(item.Name.LastIndexOf("."), item.Name.Length - item.Name.LastIndexOf("."))))
            //    {
            //        item.AdditionalData?.TryGetValue(@"@microsoft.graph.downloadUrl", out downloadURL);
            //        PartialHTTPStream httpResponseStream = new PartialHTTPStream(downloadURL.ToString(), 100000);
            //        TagLib.Tag tag = tags.FileTagReader(httpResponseStream, "test.flac");
            //        fileTags.Add(tag);

            //        Track track = new Track();
            //        track.Name = item.Name;
            //        track.Path = downloadURL.ToString();

            //        library.saveTrack(track);
            //    }

            //}
        }

    }
}
