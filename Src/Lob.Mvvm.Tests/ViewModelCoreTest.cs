using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lob.Mvvm.Tests
{
    [TestClass]
    public class ViewModelCoreTest
    {
        [TestInitialize]
        public void InitializeTests()
        {
            //UnityServiceLocator locator = new UnityServiceLocator(ConfigureUnityContainer());
            //ServiceLocator.SetLocatorProvider(() => locator);
        }

        //protected virtual IUnityContainer ConfigureUnityContainer()
        //{
        //    IUnityContainer unityContainer = new UnityContainer();
        //    unityContainer.RegisterType<IEventAggregator, EventAggregator>();
        //    unityContainer.RegisterType<INotificationService, NotificationServiceCore>();
        //    unityContainer.RegisterType<IMessageService, MessageBoxServiceMock>();
        //    unityContainer.RegisterType<INavigationServiceCore, NavigationServiceMock>();

        //    return unityContainer;
        //}
    }
}