/// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.Globalization;
using System.Net;


namespace AtomFeedService
{
    // TODO: Modify the service behavior settings (instancing, concurrency etc) based on the service's requirements.
    // TODO: Please set IncludeExceptionDetailInFaults to false in production environments
    [ServiceBehavior( IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single )]
    [AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    [ServiceContract]
    public partial class FeedService
    {
        // TODO: Modify the URI template and method parameters according to your application. An example URL is http://<url-for-svc-file>?numItems=1
        /// <summary>
        /// Returns an Atom feed.
        /// </summary>
        /// <param name="topItems"></param>
        /// <returns>Atom feed in response to a HTTP GET request at URLs conforming to the URI template of the WebGetAttribute.</returns>
        [WebGet( UriTemplate = "?max={topItems}" )]
        [OperationContract]
        public Atom10FeedFormatter GetProcessesFeed( int topItems )
        {
            // Create the list of syndication items. These correspond to Atom entries
            List<SyndicationItem> items = new List<SyndicationItem>();
            int currentItem = 0;
            foreach (Process currentProces in Process.GetProcesses())
            {
                currentItem += 1;
                if ( topItems > 0 && currentItem > topItems )
                {
                    continue;
                }
                items.Add( new SyndicationItem
                {                   
                    // Every entry must have a stable unique URI id
                    Id = String.Format( "http://tempuri.org/Processes?max={0}", currentProces.Id ),
                    Title = new TextSyndicationContent( String.Format( "Process '{0}'", currentProces.ProcessName ) ),
                    // Every entry should include the last time it was updated
                    LastUpdatedTime = DateTime.Now,
                    // The Atom spec requires an author for every entry. If the entry has no author, use the empty string
                    Authors = 
                    { 
                        new SyndicationPerson() 
                        {
                            Name = currentProces.StartInfo.UserName
                        }
                    },
                    // The content of an Atom entry can be text, xml, a link or arbitrary content. In this sample text content is used.
                    Content = new TextSyndicationContent( String.Format( "FileName: {0}\nArguments: {1}", currentProces.StartInfo.FileName, currentProces.StartInfo.Arguments) ),
                } );

            }
            // create the feed containing the syndication items.
            SyndicationFeed feed = 
                new SyndicationFeed
                {
                    // The feed must have a unique stable URI id
                    Id = "http://tempuri.org/FeedId",
                    Title = new TextSyndicationContent( "Processor List" ),
                    Items = items
                };
            #region Sets response content-type for Atom feeds
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/atom+xml";
            #endregion
            return feed.GetAtom10Formatter();
        }

    }
}
