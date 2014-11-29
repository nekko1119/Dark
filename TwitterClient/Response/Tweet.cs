using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
	// TODO 不完全
	[DataContract]
	public class Tweet
	{
		[DataMember(Name = "created_at")]
		private string CreateAtString
		{
			get
			{
				return CreateAt.ToString();
			}
			set
			{
				CreateAt = DateTime.ParseExact(value, "ddd MMM dd HH:mm:ss zzz yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
			}
		}

		public DateTime CreateAt
		{
			get;
			private set;
		}

		[DataMember(Name = "id")]
		public long Id
		{
			get;
			private set;
		}

		[DataMember(Name = "id_str")]
		public string IdStr
		{
			get;
			private set;
		}

		[DataMember(Name = "text")]
		public string Text
		{
			get;
			private set;
		}

		[DataMember(Name = "source")]
		public string Source
		{
			get;
			private set;
		}

		[DataMember(Name = "trancated")]
		public bool Trancated
		{
			get;
			private set;
		}

		[DataMember(Name = "in_reply_to_status_id")]
		public long? InReplyToStatusId
		{
			get;
			private set;
		}

		[DataMember(Name = "in_reply_to_status_id_str")]
		public string InReplyToStatusIdStr
		{
			get;
			private set;
		}

		[DataMember(Name = "in_reply_to_user_id")]
		public long? InReplyToUserId
		{
			get;
			private set;
		}

		[DataMember(Name = "in_reply_to_user_id_str")]
		public string InReplyToUserIdStr
		{
			get;
			private set;
		}

		[DataMember(Name = "in_reply_to_screen_name")]
		public string InReplyToScreenName
		{
			get;
			private set;
		}

		[DataMember(Name = "geo")]
		public Geometory Geo
		{
			get;
			private set;
		}

		[DataMember(Name = "coordinates")]
		public Geometory Coordinates
		{
			get;
			private set;
		}

		[DataMember(Name = "place")]
		public Place Place
		{
			get;
			private set;
		}

		[DataMember(Name = "contributors")]
		public List<long> Contributors
		{
			get;
			private set;
		}

		[DataMember(Name = "retweet_count")]
		public long RetweetCount
		{
			get;
			private set;
		}

		[DataMember(Name = "favorite_count")]
		public long FavoriteCount
		{
			get;
			private set;
		}

		[DataMember(Name = "entities")]
		public EntityForTweets Entities
		{
			get;
			private set;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append(
				string.Format(
				"created_at: {0}\nid: {1}\nid_str: {2}\ntext: {3}\nsource: {4}\nin_reply_to_status_id: {5}\nin_reply_to_status_id_str: {6}\nin_reply_to_user_id: {7}\n" + 
				"in_reply_to_user_id_str: {8}\nin_reply_to_screen_name: {9}\ngeo: {10}\ncoordinates: {11}\nplace: {12}\n",
				CreateAt,
				Id,
				IdStr,
				Text,
				Source,
				InReplyToStatusId,
				InReplyToStatusIdStr,
				InReplyToUserId,
				InReplyToUserIdStr,
				InReplyToScreenName,
				Geo,
				Coordinates,
				Place));
			builder.Append("contributors:\n");
			if (Contributors != null)
			{
				foreach (var c in Contributors)
				{
					builder.Append(c + "\n");
				}
			}
			builder.Append(
				string.Format(
				"retweet_count: {0}\nfavorite_count: {1}\nentities:\n{2}\n",
				RetweetCount,
				FavoriteCount,
				Entities));
			return builder.ToString();
		}
	}
}
