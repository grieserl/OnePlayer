using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static CloudPlayer.App;


[assembly: Dependency(typeof(CloudPlayer.UWP.LocalStorage_UWP))]
namespace CloudPlayer.UWP
{
    class LocalStorage_UWP : LocalStorage
    {
        public async Task<string> GetLocalStoragePath()
        {            
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}
