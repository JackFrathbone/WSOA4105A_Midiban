using UnityEngine;
using FMODUnity;
using System;
using System.Runtime.InteropServices;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] EventReference _music;

    public TimelineInfo timelineInfo = null;
    private GCHandle _timelineHandle;

    private FMOD.Studio.EventInstance _musicInstance;

    private FMOD.Studio.EVENT_CALLBACK _beatCallBack;

    public delegate void BeatEventDelegate();
    public static event BeatEventDelegate beatUpdated;

    public delegate void MarkerListenerDelegate();
    public static event MarkerListenerDelegate markerUpdated;

    public static int lastBeat = 0;
    public static string lastMarkerString = null;

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    private void Awake()
    {
        instance = this;

        if (!_music.IsNull)
        {
            _musicInstance = RuntimeManager.CreateInstance(_music);
            _musicInstance.start();
        }
    }

    private void Start()
    {
        if (!_music.IsNull)
        {
            timelineInfo = new TimelineInfo();
            _beatCallBack = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
            _timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
            _musicInstance.setUserData(GCHandle.ToIntPtr(_timelineHandle));
            _musicInstance.setCallback(_beatCallBack, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        }
    }

    private void Update()
    {
        if(lastMarkerString != timelineInfo.lastMarker)
        {
            lastMarkerString = timelineInfo.lastMarker;

            if(markerUpdated != null)
            {
                markerUpdated();
            }
        }

        if(lastBeat != timelineInfo.currentBeat)
        {
            lastBeat = timelineInfo.currentBeat;

            if(beatUpdated != null)
            {
                beatUpdated();
            }
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Box($"Current Beat = {timelineInfo.currentBeat}, last marker = {(string)timelineInfo.lastMarker}");
    }
#endif

    private void OnDestroy()
    {
        _musicInstance.setUserData(IntPtr.Zero);
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.release();
        _timelineHandle.Free();
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline callback error " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBeat = parameter.beat;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
        }

        return FMOD.RESULT.OK;
    }
}
