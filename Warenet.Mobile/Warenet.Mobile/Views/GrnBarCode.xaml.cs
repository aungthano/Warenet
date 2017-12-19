using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Warenet.Mobile.Models;
using Warenet.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Warenet.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GrnBarCode : ContentPage
    {
        bool isFlashOn = false;
        whbi2 lastItemDetail;
        string lastItemRefNo;

        public GrnBarCode()
        {
            InitializeComponent();

            zxScanner.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () => {
                    // Stop analysis until we navigate away so we don't keep reading barcodes
                    zxScanner.IsAnalyzing = false;

                    if (result.Text == null || result.Text.Equals(lastItemRefNo))
                    {
                        // Prevent duplicate scans
                        return;
                    }

                    
                    lastItemRefNo = result.Text;
                    bool isValidItem = await LoadItemDetail(lastItemRefNo);
                    if (isValidItem)
                    {
                        await SaveItemDetail(lastItemDetail);
                    }
                    else
                    {
                        lastItemRefNo = "";
                    }

                    zxScanner.IsAnalyzing = true;
                });

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            zxScanner.IsScanning = true;

            TimeSpan ts = new TimeSpan(0, 0, 0, 2, 0);
            Device.StartTimer(ts, () => {
                zxScanner.AutoFocus();
                return true;
            });
        }

        protected override void OnDisappearing()
        {
            zxScanner.IsScanning = false;
            base.OnDisappearing();
        }

        private void btnRescan_Clicked(object sender, EventArgs e)
        {
            ResetItemInfo();
        }
        
        private void btnFlash_Clicked(object sender, EventArgs e)
        {
            isFlashOn = !isFlashOn;
            btnFlash.Text = string.Format("Flash {0}", isFlashOn ? "Off" : "On");
        }

        void ResetItemInfo()
        {
            lblItem.Text = "Item - ";
            lblQty.Text = "Dim - , Qty - ";
            lblSize.Text = "Size(cm) - ";
            lblCapacity.Text = "Vol - , Space - ";
        }

        async Task<bool> LoadItemDetail(string itemRefNo)
        {
            whit1 myItem = await GrnBarCodeService.GetItem(itemRefNo);
            if (myItem == null)
            {
                ResetItemInfo();
                DependencyService.Get<IAudio>().PlayWavFail();
                await DisplayAlert("Scan Result", "Invalid Item Ref No!", "OK");

                return false;
            }

            DependencyService.Get<IAudio>().PlayWavSuccess();

            decimal? length, width, height, weight, volume, space;
            int? qty;
            string uom;
            switch (myItem.DimensionFlag)
            {
                case "P":
                    length = myItem.PackingLength;
                    width = myItem.PackingWidth;
                    height = myItem.PackingHeight;
                    weight = myItem.PackingWeight;
                    volume = myItem.PackingVolume;
                    space = myItem.PackingSpaceArea;
                    qty = myItem.PackingQty;
                    uom = myItem.PackingUomCode;
                    break;
                
                case "W":
                    length = myItem.WholeLength;
                    width = myItem.WholeWidth;
                    height = myItem.WholeHeight;
                    weight = myItem.WholeWeight;
                    volume = myItem.WholeVolume;
                    space = myItem.WholeSpaceArea;
                    qty = myItem.WholeQty;
                    uom = myItem.WholeUomCode;
                    break;

                default:
                    length = myItem.LooseLength;
                    width = myItem.LooseWidth;
                    height = myItem.LooseHeight;
                    weight = myItem.LooseWeight;
                    volume = myItem.LooseVolume;
                    space = myItem.LooseSpaceArea;
                    qty = myItem.LooseQty;
                    uom = myItem.LooseUomCode;
                    break;
            }
            string strLength = length.HasValue ? length.Value.ToString("F2") : "";
            string strWidth = width.HasValue ? width.Value.ToString("F2") : "";
            string strHeight = height.HasValue ? height.Value.ToString("F2") : "";
            string strVolume = volume.HasValue ? volume.Value.ToString("F4") : "";
            string strSpace = space.HasValue ? space.Value.ToString("F4") : "";
            string strQty = qty.HasValue ? qty.Value.ToString("F2") : "";
            string strUom = !string.IsNullOrEmpty(uom) ? uom : "";

            string strDim = "";
            switch (myItem.DimensionFlag)
            {
                case "P":
                    strDim = "PACKING";
                    break;
                case "W":
                    strDim = "WHOLE";
                    break;
                default:
                    strDim = "LOOSE";
                    break;
            }

            lastItemDetail = new whbi2
            {
                TrxNo = 0,
                TablePrefix = "GRN",
                ItemCode = myItem.ItemCode,
                ItemName = myItem.ItemName,
                DimensionFlag = myItem.DimensionFlag,
                Qty = qty,
                UomCode = uom,
                Length = length,
                Width = width,
                Height = height,
                Weight = weight,
                Volume = volume,
                SpaceArea = space
            };

            lblItem.Text = string.Format("Item - ({0}) {1}", 
                                        myItem.ItemCode, 
                                        myItem.ItemName);

            lblQty.Text = string.Format("Dim - {0}, Qty - {1} ({2})",
                                        strDim,
                                        strQty,
                                        strUom);

            lblSize.Text = string.Format("Size(cm) - {0} x {1} x {2}",
                                        strLength,
                                        strWidth,
                                        strHeight);

            lblCapacity.Text = string.Format("Vol - {0}, Space - {1}",
                                            strVolume,
                                            strSpace);

            return true;
        }

        async Task<bool> SaveItemDetail(whbi2 itemDetail)
        {
            bool isValid = await GrnBarCodeService.SaveItem(itemDetail);
            return isValid;
        }
    }
}