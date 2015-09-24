using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using AuthorizeServer.api.Entities;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace AuthorizeServer.api.Models
{
    //This is just a fake store we can use until there are actual audiences (clients)
    public static class AudiencesStore
    {
        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();

        static AudiencesStore()
        {
            AudiencesList.TryAdd("d3c2e8f35db549df8b6507f7e025301d",
                                new Audience
                                {
                                    ClientId = "d3c2e8f35db549df8b6507f7e025301d",
                                    Base64Secret = "77WQguvGb_VS60ugvxT78i3ugL34ZgBLYygxtBMpsC0",
                                    Name = "Test App"
                                });
        }

        public static Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            var newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AudiencesList.TryAdd(clientId, newAudience);
            return newAudience;
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience;
            return AudiencesList.TryGetValue(clientId, out audience) ? audience : null;
        }
    }
}