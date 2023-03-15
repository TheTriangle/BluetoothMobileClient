using UniversalBeacon.Library.Core.Entities;
using BeaconReceiver.ViewCells;
using Xamarin.Forms;

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
            if (item is Beacon beacon && beacon.BeaconType == Beacon.BeaconTypeEnum.Eddystone)
            {
                return _eddystoneTlmTemplate;
            }
            return _otherTemplate;
        }
    }
}
