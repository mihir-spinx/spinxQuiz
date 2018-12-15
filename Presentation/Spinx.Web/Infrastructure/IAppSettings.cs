namespace Spinx.Web.Infrastructure
{
    public interface IAppSettings
    {
        string WebsiteName { get; set; }
        string WebsiteUrl { get; set; }

        bool RecaptchaEnable { get; set; }
        string RecaptchaPublicKey { get; set; }
        string RecaptchaSecretKey { get; set; }

        string SocialLinkYoutube { get; set; }
        string SocialLinkTwitter { get; set; }
        string SocialLinkFacebook { get; set; }

        string GoogleCustomSearchKey { get; set; }
        string TempUploads { get; set; }
        string ResumeUploads { get; set; }
        string AdvertisementImageUploads { get; set; }
        string StripePublishableApiKey { get; set; }
    }
}