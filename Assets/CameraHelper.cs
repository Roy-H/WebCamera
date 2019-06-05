using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using BestHTTP.SignalRCore.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CameraHelper : MonoBehaviour {

    public WebCamTexture mCamera = null;
    public GameObject plane;
    public RawImage Holder;
    public Dropdown dropdown;
    private WebCamTexture[] Cameras;
    readonly Uri URI = new Uri("http://localhost:53660" + "/chatHub");
    HubConnection hub;


    // Use this for initialization
    void Start () {
        Debug.Log("Script has been started");
        //plane = GameObject.FindWithTag("Player");
        Cameras = new WebCamTexture[]
        {
            new WebCamTexture(3,4,5),
            new WebCamTexture(30, 40, 5),
            new WebCamTexture(300, 400, 5),
        };
        mCamera = Cameras[0];

        //plane.GetComponent<Renderer>().material.mainTexture = mCamera;
        //mCamera.Play();
        a = new Texture2D(mCamera.width, mCamera.height);
        Holder.texture = a;
        dropdown.onValueChanged.AddListener((i) => ResolutionChanged(i));

        InitNetwork();
    }

    public void Play()
    {
        mCamera.Play();
    }
    public void Stop()
    {
        mCamera.Stop();
    }

    Texture2D a;
    // Update is called once per frame
    void Update ()
    {
        if (mCamera.isPlaying)
        {
            var data = TakeSnapshot();
            a.LoadImage(data);
            Holder.texture = a;
            File.WriteAllBytes("D:/imgo.jpg", data);
            //hub.Send("SendMessage", "u", "m");

            //string base64 = Convert.ToBase64String(bytes);
            hub.Send("UnicastVideoFrameMessage2", Convert.ToBase64String(data));
            //.OnSuccess(result => 
            //{
            //    Debug.Log("ok");
            //})
            //.OnError(error => 
            //{
            //    Debug.Log("error");
            //});
            Debug.Log("ok");
        }
        
    }

    private byte[] TakeSnapshot()
    {
        if (mCamera == null)
            return null;
        Texture2D snap = new Texture2D(mCamera.width, mCamera.height);
        snap.SetPixels(mCamera.GetPixels());
        snap.Apply();
        Debug.Log(snap.EncodeToJPG().Length);
        return snap.EncodeToJPG();

    }

    public void ResolutionChanged(int value)
    {
        Debug.Log(value);
        Stop();
        mCamera = Cameras[value];
        Play();
        //switch (value)
        //{
        //    case 0:
        //        //mCamera.requestedWidth = 480;
        //        //mCamera.requestedHeight = 640;
        //        Stop();

        //        break;
        //    case 1:

        //        mCamera.requestedWidth = 405;
        //        mCamera.requestedHeight = 725;
        //        break;
        //    case 2:
        //        mCamera.requestedWidth = 540;
        //        mCamera.requestedHeight = 960;
        //        break;
        //    default:
        //        break;
        //}
    }

    private void InitNetwork()
    {
        // Set up optional options
        HubOptions options = new HubOptions();
        options.SkipNegotiation = false;

        // Crete the HubConnection
        hub = new HubConnection(URI, new JsonProtocol(new LitJsonEncoder()), options);

        hub.OnConnected += Hub_OnConnected;
        hub.OnError += Hub_OnError;
        hub.OnClosed += Hub_OnClosed;

        hub.OnMessage += Hub_OnMessage;

        hub.On("ReceiveMessage", (string user, string msg) => { Debug.Log(string.Format("{0},{1}", user, msg)); });

        hub.StartConnect();
    }

    private bool Hub_OnMessage(HubConnection arg1, Message arg2)
    {
        //throw new NotImplementedException();
        return true;
    }

    private void Hub_OnClosed(HubConnection obj)
    {
        //throw new NotImplementedException();
    }

    private void Hub_OnError(HubConnection arg1, string arg2)
    {
        //throw new NotImplementedException();
    }

    private void Hub_OnConnected(HubConnection obj)
    {
        //throw new NotImplementedException();
    }
}
