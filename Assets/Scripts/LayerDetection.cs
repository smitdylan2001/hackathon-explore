using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.AR;
using Niantic.ARDK.Utilities;

using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;

using Niantic.ARDK.AR.Awareness;
using Niantic.ARDK.AR.Awareness.Semantics;

using Niantic.ARDK.Extensions;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine.UI;
using TMPro;
using Niantic.ARDK.AR.HitTest;

public enum LayerTypes
{
    ground,
    artificial_ground,
    natural_ground,
    grass,
    water,
    building,
    foliage,
    sky,
}

[System.Serializable]
public struct LayerCombinations
{
    public LayerTypes LayerType;
    public string LayerName;
    public Sprite Sprite;
}

public class LayerDetection : MonoBehaviour
{
    [SerializeField] LayerCombinations[] _layerTypes;


    LayerCombinations _currentLayer;

    ISemanticBuffer _currentBuffer;

    public ARSemanticSegmentationManager _semanticManager;
    public Camera _camera;
    IARSession _arSession;

    Dictionary<string, Sprite> _spriteDict = new Dictionary<string, Sprite>();
    void Start()
    {
        //add a callback for catching the updated semantic buffer
        _semanticManager.SemanticBufferUpdated += OnSemanticsBufferUpdated;
        GotoNextLayer(false);

        ARSessionFactory.SessionInitialized += RunARSession;
    }
    void RunARSession(AnyARSessionInitializedArgs args)
    {
        //_arSession = args.Session;
        //var config = ARWorldTrackingConfigurationFactory.Create();

        //// Set to value other than PlaneDetection.None to enable
        //// hit tests against detected planes
        //config.PlaneDetection = PlaneDetection.Horizontal;

        //_arSession.Run(config);
    }
    private void OnSemanticsBufferUpdated(ContextAwarenessStreamUpdatedArgs<ISemanticBuffer> args)
    {
        //get the current buffer
        _currentBuffer = args.Sender.AwarenessBuffer;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.MinigameActive || PlatformAgnosticInput.touchCount <= 0) return;

        var touch = PlatformAgnosticInput.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            int x = (int)touch.position.x;
            int y = (int)touch.position.y;

            foreach (var i in _semanticManager.SemanticBufferProcessor.GetChannelNamesAt(x, y))
            {
                Debug.Log(i);
                if(i == _currentLayer.LayerType.ToString())
                {
                    GameManager.Instance.SpawnMinigame(_camera.transform.position +
                                            (2 * new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z)));

                    //var results =
                    //        _arSession.CurrentFrame.HitTest
                    //        (
                    //            _camera.pixelWidth,
                    //            _camera.pixelHeight,
                    //            touch.position,
                    //            ARHitTestResultType.All
                    //        );

                    //if (results.Count == 0)
                    //    return;

                    //var closestHit = results[0];
                    //var position = closestHit.WorldTransform.ToPosition();

                    //if(Vector3.Distance(position, _camera.transform.position) < 5)
                    //{
                    //    GameManager.Instance.spawnMinigame(_camera.transform.position + 
                    //                        (2 * new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z)));
                    //}

                }
            }
        }

    }

    public void GotoNextLayer(bool notCurrent = true)
    {
        _currentLayer = GetRandomLayer(notCurrent);
        GameManager.Instance.SetPrompt(_currentLayer.LayerName, _currentLayer.Sprite);
    }

    private LayerCombinations GetRandomLayer(bool notCurrent = true)
    {
        var layer = _layerTypes[Random.Range(0, _layerTypes.Length)];
        if (notCurrent && layer.LayerType == _currentLayer.LayerType) layer = GetRandomLayer();

        Debug.Log(layer.LayerName);

        return layer;
    }
}
