using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Warenet.WebApi.Models;

namespace Warenet.WebApi.Hubs
{
    [HubName("BarcodeHub")]
    public class BarcodeHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void AddNewBarcodeItemDetail(whbi2 itemDetail)
        {
            Clients.All.addNewBarcodeItemDetail(itemDetail);
        }

        public void DeleteBarcodeItemDetail(whbi2 itemDetail)
        {
            Clients.All.deleteBarcodeItemDetail(itemDetail);
        }
    }
}