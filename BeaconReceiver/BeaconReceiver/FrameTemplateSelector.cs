using UniversalBeacon.Library.Core.Entities;
using BeaconReceiver.ViewCells;
using Xamarin.Forms;
using System;

namespace BeaconReceiver
{
    internal class FrameTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _eddystoneTlmTemplate;
        private readonly DataTemplate _otherTemplate;

        public FrameTemplateSelector()
        {
            _eddystoneTlmTemplate = new DataTemplate(typeof(EddystoneTLMViewCell));
            _otherTemplate = new DataTemplate(typeof(GenericViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            Console.WriteLine("Received beacon. Eddystone: " + (item is Beacon beacone && beacone.BeaconType == Beacon.BeaconTypeEnum.Eddystone));
            if (item is Beacon beacon && beacon.BeaconType == Beacon.BeaconTypeEnum.Eddystone)
            {
                return _eddystoneTlmTemplate;
            }
            return _otherTemplate;
        }
    }
}
