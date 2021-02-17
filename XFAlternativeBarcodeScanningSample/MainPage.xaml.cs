﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFAlternativeBarcodeScanningSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
        }

        void CameraView_OnDetected(System.Object sender, GoogleVisionBarCodeScanner.OnDetectedEventArg e)
        {
            var results = e.BarcodeResults;

            var resultString = string.Empty;
            foreach (var barcode in results)
            {
                resultString += $"Type: {barcode.BarcodeType}, Value: {barcode.DisplayValue}{Environment.NewLine}";
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Results", resultString, "OK");

                GoogleVisionBarCodeScanner.Methods.SetIsScanning(true);
            });
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var image = await Xamarin.Essentials.MediaPicker.PickPhotoAsync();

            var stream = await image.OpenReadAsync();
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            var results = await GoogleVisionBarCodeScanner.Methods.ScanFromImage(bytes);

            var resultString = string.Empty;
            foreach (var barcode in results)
            {
                resultString += $"Type: {barcode.BarcodeType}, Value: {barcode.DisplayValue}{Environment.NewLine}";
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Results", resultString, "OK");
            });
        }
    }
}
