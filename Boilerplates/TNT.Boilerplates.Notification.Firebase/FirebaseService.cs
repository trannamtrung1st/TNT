using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using TNT.Boilerplates.Notification.Firebase.Abstracts;
using TNT.Boilerplates.Notification.Firebase.Configurations;

namespace TNT.Boilerplates.Notification.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        private FirebaseApp _defaultApp;
        private FirebaseMessaging _messaging;
        private readonly IOptions<FirebaseOptions> _options;

        public FirebaseService(IOptions<FirebaseOptions> options)
        {
            _options = options;
        }

        public FirebaseMessaging Messaging => _messaging;

        public async Task Initialize(CancellationToken cancellationToken = default)
        {
            _defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = await GoogleCredential.FromFileAsync(_options.Value.CredentialsFilePath, cancellationToken)
            });

            _messaging = FirebaseMessaging.GetMessaging(_defaultApp);
        }
    }
}
