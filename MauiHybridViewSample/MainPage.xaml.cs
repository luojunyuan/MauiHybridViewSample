using System.Diagnostics;

namespace MauiHybridViewSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            hybridWebView.SetInvokeJavaScriptTarget(this);
        }

        /// <summary>
        /// 从 CSharp 发消息到 js
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendMessageButtonClicked(object sender, EventArgs e)
        {
            hybridWebView.SendRawMessage($"Hello from C#!");
        }

        /// <summary>
        /// 从 js 发消息到 CSharp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnHybridWebViewRawMessageReceived(object sender, HybridWebViewRawMessageReceivedEventArgs e)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Alert", "You have been alerted", "OK");
                });
                return;
            }

            await DisplayAlert("Raw Message Received", e.Message, "OK");
        }

        // 下面是从 js invoke CSharp 函数，也可以带参数返回，见 wwwroot/index.html

        public void DoSyncWork()
        {
            Debug.WriteLine("DoSyncWork");
        }

        public void DoSyncWorkParams(int i, string s)
        {
            Debug.WriteLine($"DoSyncWorkParams: {i}, {s}");
        }

        public string DoSyncWorkReturn()
        {
            Debug.WriteLine("DoSyncWorkReturn");
            return "Hello from C#!";
        }

        public SyncReturn DoSyncWorkParamsReturn(int i, string s)
        {
            Debug.WriteLine($"DoSyncWorkParamReturn: {i}, {s}");
            return new SyncReturn
            {
                Message = "Hello from C#!" + s,
                Value = i
            };
        }

        public async Task DoAsyncWork()
        {
            Debug.WriteLine("DoAsyncWork");
            await Task.Delay(1000);
        }

        public async Task DoAsyncWorkParams(int i, string s)
        {
            Debug.WriteLine($"DoAsyncWorkParams: {i}, {s}");
            await Task.Delay(1000);
        }

        public async Task<String> DoAsyncWorkReturn()
        {
            Debug.WriteLine("DoAsyncWorkReturn");
            await Task.Delay(1000);
            return "Hello from C#!";
        }

        public async Task<SyncReturn> DoAsyncWorkParamsReturn(int i, string s)
        {
            Debug.WriteLine($"DoAsyncWorkParamsReturn: {i}, {s}");
            await Task.Delay(1000);
            return new SyncReturn
            {
                Message = "Hello from C#!" + s,
                Value = i
            };
        }

        public class SyncReturn
        {
            public string? Message { get; set; }
            public int Value { get; set; }
        }
    }
}
