using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.VFX;

namespace GraphicsBufferVFXSample
{
    public class BufferGenerator : MonoBehaviour
    {
        [SerializeField] private VisualEffect _effect;

        private GraphicsBuffer _buffer;

        private int bufferCount = 20;

        private float[] bufferArray = new float[20];

        private readonly int bufferProperty = Shader.PropertyToID("buffer");

        private readonly int bufferCountProperty = Shader.PropertyToID("bufferCount");

        private float sinIndex = 0;

        private void Start()
        {
            _buffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw, bufferCount, sizeof(float));

            if (_effect != null)
            {
                _effect.SetGraphicsBuffer(bufferProperty, _buffer);
                _effect.SetInt(bufferCountProperty, bufferCount);
            }
        }

        private void Update()
        {
            for (var i = 0; i < bufferCount; i++)
            {
                bufferArray[i] = Mathf.Sin(sinIndex + i * 0.5f);
            }

            sinIndex += 0.05f;
            _buffer.SetData(bufferArray);
        }
    }
}