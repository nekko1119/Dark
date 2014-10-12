using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Response
{
	[DataContract]
	public class ProfileResponse
	{
		public StatusCode StatusCode
		{
			get;
			set;
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

		[DataMember(Name = "name")]
		public string Name
		{
			get;
			private set;
		}

		[DataMember(Name = "screen_name")]
		public string ScreenName
		{
			get;
			private set;
		}

		[DataMember(Name = "location")]
		public string Locaation
		{
			get;
			private set;
		}

		[DataMember(Name = "description")]
		public string Description
		{
			get;
			private set;
		}

		[DataMember(Name = "url")]
		private string UrlString
		{
			get
			{
				return Url.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				Url = new Uri(value);
			}
		}

		public Uri Url
		{
			get;
			private set;
		}

		[DataMember(Name = "entities")]
		public EntityForUsers Entities
		{
			get;
			private set;
		}


		[DataMember(Name = "protected")]
		public bool Protected
		{
			get;
			private set;
		}

		[DataMember(Name = "followers_count")]
		public long FollowersCount
		{
			get;
			private set;
		}

		[DataMember(Name = "friends_count")]
		public int FriendsCount
		{
			get;
			private set;
		}

		[DataMember(Name = "listed_count")]
		public long ListedCount
		{
			get;
			private set;
		}

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

		[DataMember(Name = "favourites_count")]
		public long FavouritesCount
		{
			get;
			set;
		}

		[DataMember(Name = "utc_offset")]
		public long UtcOffset
		{
			get;
			private set;
		}

		[DataMember(Name = "time_zone")]
		public string TimeZone
		{
			get;
			private set;
		}

		[DataMember(Name = "geo_enabled")]
		public bool GeoEnabled
		{
			get;
			private set;
		}

		[DataMember(Name = "verified")]
		public bool Verified
		{
			get;
			private set;
		}

		[DataMember(Name = "statuses_count")]
		public long StatusesCount
		{
			get;
			private set;
		}

		[DataMember(Name = "lang")]
		public string Lang
		{
			get;
			private set;
		}

		[DataMember(Name = "status")]
		public Tweet Status
		{
			get;
			private set;
		}

		[DataMember(Name = "contributors_enabled")]
		public bool ContributorsEnable
		{
			get;
			private set;
		}

		[DataMember(Name = "is_translator")]
		public bool IsTranslator
		{
			get;
			private set;
		}

		[DataMember(Name = "is_translation_enabled")]
		public bool IsTranslationEnabled
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_background_color")]
		public string ProfileBackgroundColor
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_background_image_url")]
		private string ProfileBackgroundImageUrlString
		{
			get
			{
				return ProfileBackgroundImageUrl.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				ProfileBackgroundImageUrl = new Uri(value);
			}
		}

		public Uri ProfileBackgroundImageUrl
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_background_image_url_https")]
		private string ProfileBackgroundImageUrlHttpsString
		{
			get
			{
				return ProfileBackgroundImageUrlHttps.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				ProfileBackgroundImageUrlHttps = new Uri(value);
			}
		}

		public Uri ProfileBackgroundImageUrlHttps
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_background_tile")]
		public bool ProfileBackgroundTile
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_image_url")]
		private string ProfileImageUrlString
		{
			get
			{
				return ProfileImageUrl.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				ProfileImageUrl = new Uri(value);
			}
		}

		public Uri ProfileImageUrl
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_image_url_https")]
		private string ProfileImageUrlHttpsString
		{
			get
			{
				return ProfileImageUrlHttps.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				ProfileImageUrlHttps = new Uri(value);
			}
		}

		public Uri ProfileImageUrlHttps
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_banner_url")]
		private string ProfileBannerUrlString
		{
			get
			{
				return ProfileBannerUrl.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				ProfileBannerUrl = new Uri(value);
			}
		}

		public Uri ProfileBannerUrl
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_link_color")]
		public string ProfileLinkColor
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_sidebar_border_color")]
		public string ProfileSidebarBorderColor
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_sidebar_fill_color")]
		public string ProfileSidebarFillColor
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_text_color")]
		public string ProfileTextColor
		{
			get;
			private set;
		}

		[DataMember(Name = "profile_use_background_image")]
		public bool ProfileUseBackgroundImage
		{
			get;
			private set;
		}

		[DataMember(Name = "default_profile")]
		public bool DefaultProfile
		{
			get;
			private set;
		}

		[DataMember(Name = "default_profile_image")]
		public bool DefaultProfileImage
		{
			get;
			private set;
		}

		[DataMember(Name = "following")]
		public bool Following
		{
			get;
			private set;
		}

		[DataMember(Name = "follow_request_sent")]
		public bool FollowRequestSent
		{
			get;
			private set;
		}

		[DataMember(Name = "notifications")]
		public bool Notifications
		{
			get;
			private set;
		}

		[DataMember(Name = "suspended")]
		public bool Suspended
		{
			get;
			private set;
		}

		[DataMember(Name = "needs_phone_verification")]
		public bool NeedsPhoneVerification
		{
			get;
			private set;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append(
				string.Format(
				"id: {0}\nid_str: {1}\nname: {2}\nscreen_name: {3}\nlocation: {4}\ndescription: {5}\nurl: {6}\n",
				Id,
				IdStr,
				Name,
				ScreenName,
				Locaation,
				Description,
				Url));

			builder.Append("entites:\n");
			builder.Append(Entities);
			builder.Append(
				string.Format(
				"protected: {0}\nfollowers_count: {1}\nfriends_count: {2}\nlisted_count: {3}\ncreated_at: {4}\nfavourites_count: {5}\nutc_offset: {6}\ntime_zone: {7}\n" + 
				"geo_enabled: {8}\nverified: {9}\nstatuses_count: {10}\nlang: {11}\n",
				Protected,
				FollowersCount,
				FriendsCount,
				ListedCount,
				CreateAt,
				FavouritesCount,
				UtcOffset,
				TimeZone,
				GeoEnabled,
				Verified,
				StatusesCount,
				Lang));

			builder.Append("status:\n");
			builder.Append(Status);

			builder.Append(
				string.Format(
				"is_translator: {0}\nis_translation_enabled: {1}\nprofile_background_color: {2}\nprofile_background_image_url: {3}\nprofile_background_image_url_https: {4}\nprofile_background_tile: {5}\nprofile_image_url: {6}\nprofile_image_url_https: {7}\n" +
				"profile_banner_url: {8}\nprofile_link_color: {9}\nprofile_sidebar_border_color: {10}\nprofile_sidebar_fill_color: {11}\nprofile_text_color: {12}\nprofile_use_background_image: {13}\ndefault_profile: {14}\ndefault_profile_image: {15}\n" +
				"following: {16}\nfollow_request_sent: {17}\nnotifications: {18}\n",
				IsTranslator,
				IsTranslationEnabled,
				ProfileBackgroundColor,
				ProfileBackgroundImageUrl,
				ProfileBackgroundImageUrlHttps,
				ProfileBackgroundTile,
				ProfileImageUrl,
				ProfileImageUrlHttps,
				ProfileBannerUrl,
				ProfileLinkColor,
				ProfileSidebarBorderColor,
				ProfileSidebarFillColor,
				ProfileTextColor,
				ProfileUseBackgroundImage,
				DefaultProfile,
				DefaultProfileImage,
				Following,
				FollowRequestSent,
				Notifications));

			return builder.ToString();
		}
	}
}
