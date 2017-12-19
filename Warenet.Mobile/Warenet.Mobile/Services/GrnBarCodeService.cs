using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warenet.Mobile.Helpers;
using Warenet.Mobile.Models;

namespace Warenet.Mobile.Services
{
    class GrnBarCodeService
    {
        public static async Task<whit1> GetItem(string itemRefNo)
        {
            var myParams = new[]
            {
                new KeyValuePair<string,object>("ItemRefNo",itemRefNo)
            };
            whit1 myItem = await Settings.ApiHelper.GetAsync<whit1>("api/item/getitembyitemrefno", myParams);
            return myItem;
        }

        public static async Task<bool> SaveItem(whbi2 item)
        {
            bool isValid = await Settings.ApiHelper.PostAsync("api/goodsreceiptnote/saveBarcodeItemDetail", item);
            return isValid;
        }
    }
}