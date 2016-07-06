using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace TraderList.Events
{
    public interface IBeastEventAggregatorExtensionsProvider
    { 
        void Publish<TEvent>(IEventAggregator eventAggregator, TEvent publishEvent);

        SubscriptionToken Subscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription);

        SubscriptionToken Subscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription,
                                            bool keepSubscriberReferenceAlive);

        SubscriptionToken Subscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription,
                                            ThreadOption threadOption, bool keepSubscriberReferenceAlive = false,
                                            Predicate<TEvent> filter = null);

        void Unsubscribe<TEvent>(IEventAggregator eventAggregator, SubscriptionToken token);

        void Unsubscribe<TEvent>(IEventAggregator eventAggregator, Action<TEvent> subscription);        
    }
}
