using System;
using System.Threading.Tasks;
using ETLAppInternal.Constants;
using ETLAppInternal.Contracts.Services.General;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace ETLAppInternal.Services.General
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectivity _connectivity;

        public ConnectionService()
        {
            _connectivity = CrossConnectivity.Current;
            _connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ConnectivityChanged?.Invoke(this, new ConnectivityChangedEventArgs() { IsConnected = e.IsConnected });
        }

       

        public bool IsConnected => _connectivity.IsConnected;

        public async Task<bool> IsApiReachable()
        {
            if (!_connectivity.IsConnected) return false;
            return await _connectivity.IsRemoteReachable(ApiConstants.BaseApiUrl, 80, 5000);
        }

        public event ConnectivityChangedEventHandler ConnectivityChanged;
    }
}
