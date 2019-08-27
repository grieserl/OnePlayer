﻿using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
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
                IDriveItemChildrenCollectionPage driveItems = await GraphClient.Me.Drive.Root.ItemWithPath("/Music/Audioslave/Audioslave").Children.Request().GetAsync();

                List<string> audioExtensions = new List<string>();
                audioExtensions.Add(".flac");
                audioExtensions.Add(".mp3");
                
                foreach (var item in driveItems)
                {
                    object downloadURL = new object();
                    if (audioExtensions.Contains(item.Name.Substring(item.Name.LastIndexOf("."), item.Name.Length - item.Name.LastIndexOf("."))))
                    {
                        item.AdditionalData?.TryGetValue(@"@microsoft.graph.downloadUrl", out downloadURL);
                        PartialHTTPStream httpResponseStream = new PartialHTTPStream(downloadURL.ToString(), 100000);
                        TagLib.Tag tag = AudioTagHelper.FileTagReader(httpResponseStream, "test.flac");
                       

                        Track track = new Track();
                        track.Title = tag.Title;
                        track.OneDrive_ID = item.Id;
                        track.FileName = item.Name;


                        await App.Library.SaveTrack(track);
                    }

                }
            }
            catch(Exception e)
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
