
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;

namespace Utility
{
    [ServiceContract]
    public interface IFeedInterface
    {
        [OperationContract]
        [WebGet( UriTemplate = "Feeds/formats=(format)" )]
        [ServiceKnownType( typeof( Rss20FeedFormatter ) )]
        [ServiceKnownType( typeof( Atom10FeedFormatter ) )]
        SyndicationFeedFormatter Feed( string format );
    }

    public class RssFeed : IFeedInterface
    {
        public SyndicationFeedFormatter Feed( string format )
        {
            IEnumerable<SyndicationItem> feedItems = from item in DataSource.GetData() select new SyndicationItem( item.ToString(), "The factor for Euler PrimeFactors", new Uri( "http://localhost:8000" ) );

            SyndicationFeed feed = new SyndicationFeed( feedItems );
            feed.Title = new TextSyndicationContent( "Date ranges" );
            feed.ImageUrl = new Uri( "http://localhost:8000/_layouts/images/blank.gif" );
            feed.Description = new TextSyndicationContent( "Takes a daterange of 356 days and returns it" );
            feed.LastUpdatedTime = DateTime.Now;

            if ( string.IsNullOrEmpty( format ) || format.Equals( "rss" ) )
            {
                return new Rss20FeedFormatter( feed );
            }
            return new Atom10FeedFormatter( feed );
        }
        private class DataSource
        {
            public static IEnumerable<DateTime> GetData()
            {
                Collection<DateTime> range = new Collection<DateTime>();
                foreach ( int i in Enumerable.Range( 1, 365 ).Reverse() )
                {
                    range.Add( DateTime.Today.AddDays( -1 * i ) );
                }
                return range;
            }
        }
    }
}