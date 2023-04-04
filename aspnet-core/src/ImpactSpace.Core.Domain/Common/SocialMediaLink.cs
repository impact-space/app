using System;
using System.Collections.Generic;
using ImpactSpace.Core.Common;
using Volo.Abp.Domain.Values;

namespace ImpactSpace.Core.Organizations;

public class SocialMediaLink : ValueObject
{
    public SocialMediaPlatform Platform { get; private set; }
    public string Url { get; private set; }
    
    private SocialMediaLink()
    {
        
    }

    public SocialMediaLink(SocialMediaPlatform platform, string url)
    {
        SetPlatform(platform);
        SetUrl(url);
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        // Order is important for comparing value objects.
        // The properties that define the object should be yielded in the order they appear in the class.
        yield return Platform;
        yield return Url;
    }
    
    public void SetPlatform(SocialMediaPlatform platform)
    {
        // You can add validation for the platform name here, if needed.
        Platform = platform;
    }

    public void SetUrl(string url)
    {
        // Validate and set the URL like in the previous examples.
        if (string.IsNullOrWhiteSpace(url))
        {
            Url = null;
            return;
        }

        if (!IsValidUrl(url))
        {
            throw new ArgumentException("The provided URL is not valid.", nameof(url));
        }

        Url = url;
    }

    private static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}