using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public static class FireBaseManager
{
    private const string TokenDataName = "data.dat";
    private const string ApiKey = @"AIzaSyCIEhgadQTB5lWjKHUm5IGSpvkjr2Ud7VU";
    private const string DataBaseDomain = @"https://adsexperiments-3b00f.firebaseio.com/";

    public static FirebaseClient FireBaseClient { get; private set; }

    static FireBaseManager()
    {
        FireBaseClient = new FirebaseClient(DataBaseDomain);
    }
}
