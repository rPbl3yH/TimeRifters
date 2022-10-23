using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Recorder : MonoBehaviour
{
    private List<Frame> _siquence = new List<Frame>();
    [SerializeField] private List<Transform> _childs;
    [SerializeField] private Phase _phase = Phase.Recording;
    [SerializeField] private PlayerController _recordingPlayer;
    [SerializeField] private Slider _slider;

    private int _counterOfFrame;

    private void SetPlayerComponent() {
        _recordingPlayer.Animator.enabled = _phase != Phase.Playing;
        _recordingPlayer.IsPlaying = _phase == Phase.Playing;
        _recordingPlayer.CurrentWeapon.Aim.SetActive(_phase == Phase.Playing);

        if (_siquence.Count > 0 && _phase == Phase.Recording) {
            _siquence.Clear();
        }
    }

    private void FixedUpdate() {
        Record();

        PlayRecording();
    }

    private void Record() {
        if (_phase == Phase.Recording) {
            Frame frame = new Frame();
            var childCount = _childs.Count;
            frame.objectProperties = new ObjectProperties[childCount];

            for (int i = 0; i < childCount; i++) {

                Vector3 position = _childs[i].position;
                Vector3 eulers = _childs[i].eulerAngles;
                frame.objectProperties[i] = new ObjectProperties(position, eulers);
                frame.IsShoot = _recordingPlayer.CurrentWeapon.IsShoot;
            }

            _siquence.Add(frame);
            if (frame.IsShoot) {
                print(_siquence.Count + " frame shoot " + frame.IsShoot);
            }
        }
    }

    private void PlayRecording() {
        if (_phase == Phase.Playing) {
            if (_siquence.Count <= _counterOfFrame) return;
            SetValue(_counterOfFrame);
            _counterOfFrame++;
        }
    }

    public void StopRecording() {
        _phase = Phase.None;
        _recordingPlayer.enabled = false;
        _recordingPlayer.transform.rotation = Quaternion.identity;
        _recordingPlayer.DeactivateCamera();
        _recordingPlayer.OffRotation();
    }

    public void StartRecord() {
        _siquence.Clear();
        _recordingPlayer.enabled = true;
        _phase = Phase.Recording;
        SetPlayerComponent();
    }

    public void ResumeRecords() {
        _phase = Phase.Recording;
    }

    public void Play() {
        _phase = Phase.Playing;
        _recordingPlayer.enabled = false;

        _counterOfFrame = 0;
        SetPlayerComponent();
    }

    public void SetValue(float value) {
        int frameId = (int)(value * (_siquence.Count-1));

        for (int childId = 0; childId < _childs.Count; childId++) {
            ObjectProperties properties = _siquence[frameId].objectProperties[childId];
            //print("Positions " + properties.Position[0] + "Eulers " + properties.Eulers[0]);
            SetPropertiesToChild(childId, properties);  
            
        }
        if (_siquence[frameId].IsShoot) {
            _recordingPlayer.Shoot();
        }
    }

    public void SetValue(int frameId) {
        //int frameId = (int)(value * (_siquence.Count - 1));

        for (int childId = 0; childId < _childs.Count; childId++) {
            Frame frame = _siquence[frameId];
            ObjectProperties properties = frame.objectProperties[childId];
            //print("Positions " + properties.Position[0] + "Eulers " + properties.Eulers[0]);
            SetPropertiesToChild(childId, properties);

        }
        if (_siquence[frameId].IsShoot) {
            _recordingPlayer.Shoot();

        }
    }

    private void SetPropertiesToChild(int idChild, ObjectProperties properties) {
        var child = _childs[idChild];
        
        var posX = properties.Position[0];
        var posY = properties.Position[1];
        var posZ = properties.Position[2];
        child.transform.position = new Vector3(posX, posY, posZ);

        var eulersX = properties.Eulers[0];
        var eulersY = properties.Eulers[1];
        var eulersZ = properties.Eulers[2];
        child.transform.eulerAngles = new Vector3(eulersX, eulersY, eulersZ);
    }
    

    public void AddStartChild(Transform value) {
        _childs.Add(value);
    }

    public PlayerFrames GetRecording() {
        StopRecording();
        PlayerFrames playerFrames = new PlayerFrames();
        playerFrames.Frames = _siquence.ToArray();
        //_phase = Phase.None;
        return playerFrames;
    }

    public void SetRecording(Frame[] recording) {
        _siquence = recording.ToList();
        _phase = Phase.Playing;
    }
}
