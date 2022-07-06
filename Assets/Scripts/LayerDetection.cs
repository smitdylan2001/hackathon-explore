using System;
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

public class LayerDetection : MonoBehaviour
{
    ISemanticBuffer _currentBuffer;

    public ARSemanticSegmentationManager _semanticManager;
    public Camera _camera;
    void Start()
    {
        //add a callback for catching the updated semantic buffer
        _semanticManager.SemanticBufferUpdated += OnSemanticsBufferUpdated;
    }

    private void OnSemanticsBufferUpdated(ContextAwarenessStreamUpdatedArgs<ISemanticBuffer> args)
    {
        //get the current buffer
        _currentBuffer = args.Sender.AwarenessBuffer;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlatformAgnosticInput.touchCount <= 0) { return; }

        var touch = PlatformAgnosticInput.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            //list the channels that are available
            Debug.Log("Number of Channels available " + _semanticManager.SemanticBufferProcessor.ChannelCount);
            //foreach (var c in _semanticManager.SemanticBufferProcessor.Channels)
                //Debug.Log(c);

            int x = (int)touch.position.x;
            int y = (int)touch.position.y;

            //return the indices
            int[] channelsInPixel = _semanticManager.SemanticBufferProcessor.GetChannelIndicesAt(x, y);

            //print them to console
            foreach (var i in channelsInPixel)
                Debug.Log(i);

            //return the names
            string[] channelsNamesInPixel = _semanticManager.SemanticBufferProcessor.GetChannelNamesAt(x, y);

            //print them to console
            foreach (var i in channelsNamesInPixel)
                Debug.Log("Seen: " + i);

        }

    }
}
