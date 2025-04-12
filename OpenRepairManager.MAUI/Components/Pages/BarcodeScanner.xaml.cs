using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camera.MAUI;
using Camera.MAUI.ZXing;
using Camera.MAUI.ZXingHelper;
using OpenRepairManager.Common.Models.ApiModels;
using OpenRepairManager.MAUI.Services;

namespace OpenRepairManager.MAUI.Components.Pages;

public partial class BarcodeScanner : ContentPage
{
    bool playing = false;
    public BarcodeScanner()
    {
        InitializeComponent();
        cameraView.CamerasLoaded += CameraView_CamerasLoaded;
        cameraView.BarcodeDetected += CameraView_BarcodeDetected;
        cameraView.BarCodeDecoder = (IBarcodeDecoder)new ZXingBarcodeDecoder();
        cameraView.BarCodeDecoder = new ZXingBarcodeDecoder();
        cameraView.BarCodeOptions = new BarcodeDecodeOptions
        {
            AutoRotate = true,
            PossibleFormats = { BarcodeFormat.QR_CODE },
            ReadMultipleCodes = false,
            TryHarder = true,
            TryInverted = true
        };
        cameraView.BarCodeDetectionFrameRate = 10;
        cameraView.BarCodeDetectionMaxThreads = 5;
        cameraView.ControlBarcodeResultDuplicate = true;
        cameraView.BarCodeDetectionEnabled = true;
        
    }

    private async void CameraView_BarcodeDetected(object sender, BarcodeEventArgs args)
    {
        Debug.WriteLine("BarcodeText=" + args.Result[0].Text);
        ReturningItemModel _item = new ReturningItemModel()
        {
            Guid = Guid.Parse(args.Result[0].Text),
            SessionID = Preferences.Default.Get("SessionID", 0)
        };
        var response = await ApiService.ReturningItemAsync(_item);
        if (response.Status == "Success")
        {
            await cameraView.StopCameraAsync();
            await DisplayAlert("Success", "Item imported!", "OK");
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        else
        {
            await cameraView.StopCameraAsync();
            await DisplayAlert("Error", response.Message, "OK");
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (await cameraView.StartCameraAsync() == CameraResult.Success)
                {
                    controlButton.Text = "Cancel";
                    playing = true;
                }
            });
            
        }
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await cameraView.StopCameraAsync();
        await App.Current.MainPage.Navigation.PopModalAsync();
    }
}