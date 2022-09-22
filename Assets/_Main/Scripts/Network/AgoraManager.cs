using System;
using System.Collections;
using System.Collections.Generic;
using agora_gaming_rtc;
using UnityEngine;

public class AgoraManager : MonoBehaviour {
    public string appId;
    public string channel;

    private IRtcEngine _rtcEngine;

    public bool localAudioMuted;

#if !UNITY_SERVER
    private void Awake() {
        _rtcEngine = IRtcEngine.getEngine(appId);
        _rtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR |
                                LOG_FILTER.CRITICAL);
        _rtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        _rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        _rtcEngine.JoinChannel(token: null, channelId: channel, info: "", uid: 0,
                               options: new ChannelMediaOptions(true, false,
                                                                true, false));

        _rtcEngine.OnUserMutedAudio += OnUserMutedAudio;
    }

    void OnApplicationQuit() {
        IRtcEngine.Destroy();
    }
#endif

    public void SwitchLocalAudioMute() {
        localAudioMuted = !localAudioMuted;
        _rtcEngine.MuteLocalAudioStream(localAudioMuted);
    }

    private void OnUserMutedAudio(uint uid, bool muted) {
        Debug.Log("uid = " + uid + " muted = " + muted);
    }
}