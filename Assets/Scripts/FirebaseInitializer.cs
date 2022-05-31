using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    private void Start(){
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }
}
