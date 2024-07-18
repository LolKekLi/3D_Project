#region

using Zenject;

#endregion

namespace Project.Service
{
    public static class AdvertisingServiceController
    {
        public static IAdvertisingService GetService(InjectContext context)
        {
            var defaultAdvertisingService = context.Container.Instantiate<DefaultAdvertisingService>();
            defaultAdvertisingService?.Init();
            return defaultAdvertisingService;
        }
    }
}