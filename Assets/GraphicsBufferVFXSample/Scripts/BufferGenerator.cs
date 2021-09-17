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

        private const int bufferCount = 20;

        private Vector3[] positionArray = new Vector3[bufferCount];

        private readonly int bufferProperty = Shader.PropertyToID("buffer");

        private readonly int bufferCountProperty = Shader.PropertyToID("bufferCount");

        private float sinIndex = 0;

        private void Start()
        {
            _buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, bufferCount, Marshal.SizeOf(new Vector3()));

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
                positionArray[i] = new Vector3(i * 0.1f, Mathf.Sin(sinIndex + i * 0.5f), 0);
            }

            sinIndex += 0.05f;
            _buffer.SetData(positionArray);
        }
    }
}