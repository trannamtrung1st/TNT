using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace TNT.Boilerplates.Notification.Firebase.Abstracts
{
    public interface IFirebaseService
    {
        FirebaseMessaging Messaging { get; }

        Task Initialize(CancellationToken cancellationToken = default);
    }
}
