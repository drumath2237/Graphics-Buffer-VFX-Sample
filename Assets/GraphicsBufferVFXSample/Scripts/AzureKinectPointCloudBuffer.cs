﻿using System;
using System.Runtime.InteropServices;
using Microsoft.Azure.Kinect.Sensor;
using UnityEngine;
using UnityEngine.VFX;
using System.Linq;
using System.Threading.Tasks;

namespace VisualEffectBuffer
{
    public class AzureKinectPointCloudBuffer : MonoBehaviour
    {
        private Device _kinect;
        private bool _isRunning = false;

        private GraphicsBuffer colorBuffer = null;
        private GraphicsBuffer positionBuffer = null;

        private Color[] colorArray = null;
        private Vector3[] positionArray = null;

        private Transformation _kinectTransformation = null;

        private readonly int _propertyColorBuffer = Shader.PropertyToID("colorBuffer");
        private readonly int _propertyPositionBuffer = Shader.PropertyToID("positionBuffer");

        [SerializeField] private VisualEffect _effect;

        private void Start()
        {
            _kinect = Device.Open();
            _kinect.StartCameras(new DeviceConfiguration
            {
                ColorFormat = ImageFormat.ColorBGRA32,
                ColorResolution = ColorResolution.R1080p,
                DepthMode = DepthMode.NFOV_2x2Binned,
                SynchronizedImagesOnly = true,
                CameraFPS = FPS.FPS30
            });

            _isRunning = true;

            var depthCalibration = _kinect.GetCalibration().DepthCameraCalibration;
            var width = depthCalibration.ResolutionWidth;
            var height = depthCalibration.ResolutionHeight;

            colorBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                width * height,
                Marshal.SizeOf(new Color())
            );
            positionBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, width * height,
                Marshal.SizeOf(new Vector3()));

            _effect.SetGraphicsBuffer(
                _propertyColorBuffer,
                colorBuffer
            );
            _effect.SetGraphicsBuffer(_propertyPositionBuffer, positionBuffer);

            _kinectTransformation = _kinect.GetCalibration().CreateTransformation();

            _ = Task.Run(CaptureLoop);
        }

        private void CaptureLoop()
        {
            while (_isRunning)
            {
                using var capture = _kinect.GetCapture();

                using Image colorImage = _kinectTransformation.ColorImageToDepthCamera(capture);
                colorArray = colorImage.GetPixels<BGRA>().ToArray()
                    .Select(bgra => new Color(bgra.R, bgra.G, bgra.B, bgra.A)).ToArray();

                using Image positionImage = _kinectTransformation.DepthImageToPointCloud(capture.Depth);
                positionArray = positionImage.GetPixels<Short3>().ToArray()
                    .Select(short3 => new Vector3(short3.X / 1000.0f, short3.Y / 1000.0f, short3.Z / 1000.0f))
                    .ToArray();
            }
        }

        private void Update()
        {
            if (colorArray != null && colorArray.Length != 0)
            {
                colorBuffer.SetData(colorArray);
            }

            if (positionArray != null && positionArray.Length != 0)
            {
                positionBuffer.SetData(positionArray);
            }
        }

        private void OnApplicationQuit()
        {
            _isRunning = false;

            if (colorBuffer != null)
            {
                colorBuffer.Dispose();
                colorBuffer = null;
            }

            if (positionBuffer != null)
            {
                positionBuffer.Dispose();
                positionBuffer = null;
            }

            if (_kinectTransformation != null)
            {
                _kinectTransformation.Dispose();
                _kinectTransformation = null;
            }

            if (_kinect != null)
            {
                _kinect.StopCameras();
                _kinect.Dispose();
                _kinect = null;
            }
        }
    }
}