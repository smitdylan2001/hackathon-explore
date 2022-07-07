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

[System.Flags]
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
public class LayerDetection : MonoBehaviour
{
    string[] _layerTypes = { "ground", "water", "artificial_ground", "building", "foliage" };
    string _currentLayer;

    ISemanticBuffer _currentBuffer;

    public ARSemanticSegmentationManager _semanticManager;
    public Camera _camera;
    void Start()
    {
        //add a callback for catching the updated semantic buffer
        _semanticManager.SemanticBufferUpdated += OnSemanticsBufferUpdated;
        _currentLayer = GetRandomLayer(false);
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

            //return the names
            string[] channelsNamesInPixel = _semanticManager.SemanticBufferProcessor.GetChannelNamesAt(x, y);

            //print them to console
            foreach (var i in channelsNamesInPixel)
            {
                Debug.Log(i);
                if(i == _currentLayer)
                {
                    GameManager.Instance.spawnMinigame(_camera.transform.position + (2 * new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z)));
                }
            }
        }

    }

    public void GotoNextLayer()
    {
        _currentLayer = GetRandomLayer();
    }

    private string GetRandomLayer(bool notCurrent = true)
    {
        var layer = _layerTypes[Random.Range(0, _layerTypes.Length)];
        if (notCurrent && layer == _currentLayer) layer = GetRandomLayer();

        Debug.Log(layer);

        return layer;
    }
}
