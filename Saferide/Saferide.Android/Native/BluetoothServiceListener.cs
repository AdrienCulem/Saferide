using System;
using Android.Bluetooth;

namespace Saferide.Droid.Native
{
    public class BluetoothServiceListener : IBluetoothProfileServiceListener
    {

        public BluetoothHeadset btHeadset;
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IntPtr Handle { get; }
        public void OnServiceConnected(ProfileType profile, IBluetoothProfile proxy)
        {
            if (profile == ProfileType.Headset)
            {
                 btHeadset = (BluetoothHeadset)proxy;
            }
        }

        public void OnServiceDisconnected(ProfileType profile)
        {
            if (profile == ProfileType.Headset)
            { 
                 btHeadset = null;
            }
        }
    }
}