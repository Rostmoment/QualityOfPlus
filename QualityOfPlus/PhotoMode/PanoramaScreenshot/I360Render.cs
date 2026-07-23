using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

// Original code by yasirkula 
// https://github.com/yasirkula/Unity360ScreenshotCapture

namespace QualityOfPlus.PhotoMode.PanoramaScreenshot
{
    internal static class I360Render
    {
        public static byte[] Capture(int width = 1024, bool encodeAsJPEG = true, Camera renderCam = null, bool faceCameraDirection = true)
        {
            return CaptureInternal(width, encodeAsJPEG, renderCam, faceCameraDirection);
        }

        public static void CaptureAsync(Action<byte[]> callback, int width = 1024, bool encodeAsJPEG = true, Camera renderCam = null, bool faceCameraDirection = true)
        {
            CaptureInternal(width, encodeAsJPEG, renderCam, faceCameraDirection, callback);
        }

        private static byte[] CaptureInternal(int width = 1024, bool encodeAsJPEG = true, Camera renderCam = null, bool faceCameraDirection = true, Action<byte[]> asyncCallback = null)
        {
            int cubemapSize = Mathf.Min(Mathf.NextPowerOfTwo(width), 8192);
            int outWidth = cubemapSize;
            int outHeight = cubemapSize / 2;

            GameCamera gameCam = CoreGameManager.Instance.GetCamera(0);
            if (gameCam != null && gameCam.camCom != null)
            {
                float paddingX = faceCameraDirection ? (gameCam.camCom.transform.eulerAngles.y / 360f) : 0f;
                return DoManualGameCameraConversion(gameCam, cubemapSize, outWidth, outHeight, paddingX, encodeAsJPEG, asyncCallback);
            }

            if (renderCam == null)
            {
                renderCam = Camera.main;
                if (renderCam == null)
                {
                    BasePlugin.Logger.LogError("Error: no camera detected");
                    if (asyncCallback != null) asyncCallback(null);
                    return null;
                }
            }

            RenderTexture camTarget = renderCam.targetTexture;
            bool asyncOperationStarted = false;
            RenderTexture activeRT = RenderTexture.active;
            RenderTexture cubemap = null;

            try
            {
                RenderTextureDescriptor desc = new RenderTextureDescriptor(cubemapSize, cubemapSize, RenderTextureFormat.ARGB32, 0) { dimension = TextureDimension.Cube };
                cubemap = RenderTexture.GetTemporary(desc);

                if (!renderCam.RenderToCubemap(cubemap, 63))
                {
                    BasePlugin.Logger.LogError("Rendering to cubemap is not supported on device/platform!");
                    if (asyncCallback != null) asyncCallback(null);
                    return null;
                }

                float paddingX = faceCameraDirection ? (renderCam.transform.eulerAngles.y / 360f) : 0f;

                if (asyncCallback != null)
                {
                    int completed = 0;
                    bool hasError = false;
                    Color32[][] faces = new Color32[6][];

                    for (int i = 0; i < 6; i++)
                    {
                        int faceIdx = i;
                        AsyncGPUReadback.Request(cubemap, faceIdx, TextureFormat.RGBA32, (asyncResult) =>
                        {
                            if (asyncResult.hasError) hasError = true;
                            else faces[faceIdx] = asyncResult.GetData<Color32>().ToArray();

                            completed++;
                            if (completed == 6)
                            {
                                try
                                {
                                    if (hasError)
                                    {
                                        BasePlugin.Logger.LogError("Async thumbnail request failed, falling back to conventional method");
                                        DoSyncConversion(cubemap, cubemapSize, outWidth, outHeight, paddingX, encodeAsJPEG, asyncCallback);
                                    }
                                    else
                                    {
                                        Color32[] eqPixels = new Color32[outWidth * outHeight];
                                        ConvertCubemapToEquirectangular(faces, cubemapSize, eqPixels, outWidth, outHeight, paddingX);

                                        Texture2D output = new Texture2D(outWidth, outHeight, TextureFormat.RGBA32, false);
                                        output.SetPixels32(eqPixels);
                                        output.Apply();

                                        byte[] bytes = encodeAsJPEG ? InsertXMPIntoTexture2D_JPEG(output) : InsertXMPIntoTexture2D_PNG(output);
                                        UnityEngine.Object.DestroyImmediate(output);

                                        asyncCallback(bytes);
                                    }
                                }
                                finally
                                {
                                    if (cubemap) RenderTexture.ReleaseTemporary(cubemap);
                                }
                            }
                        });
                    }
                    asyncOperationStarted = true;
                    return null;
                }
                else
                {
                    return DoSyncConversion(cubemap, cubemapSize, outWidth, outHeight, paddingX, encodeAsJPEG, null);
                }
            }
            catch (Exception e)
            {
                BasePlugin.Logger.LogError(e);
                if (!asyncOperationStarted && asyncCallback != null) asyncCallback(null);
                return null;
            }
            finally
            {
                renderCam.targetTexture = camTarget;
                if (!asyncOperationStarted)
                {
                    RenderTexture.active = activeRT;
                    if (cubemap) RenderTexture.ReleaseTemporary(cubemap);
                }
            }
        }


        private static byte[] DoManualGameCameraConversion(GameCamera gameCam, int cubemapSize, int outWidth, int outHeight, float paddingX, bool encodeAsJPEG, Action<byte[]> asyncCallback)
        {
            Color32[][] faces = new Color32[6][];
            RenderTexture faceRT = RenderTexture.GetTemporary(cubemapSize, cubemapSize, 24, RenderTextureFormat.ARGB32);
            Texture2D tempFace = new Texture2D(cubemapSize, cubemapSize, TextureFormat.RGBA32, false);
            RenderTexture activeRT = RenderTexture.active;

            BillboardUpdater[] updaters = UnityEngine.Object.FindObjectsOfType<BillboardUpdater>();
            Quaternion[] origUpdaterRots = new Quaternion[updaters.Length];
            for (int i = 0; i < updaters.Length; i++) origUpdaterRots[i] = updaters[i].transform.rotation;

            Quaternion origMainRot = gameCam.camCom.transform.rotation;
            Quaternion origBillRot = gameCam.billboardCam != null ? gameCam.billboardCam.transform.rotation : Quaternion.identity;
            Quaternion origCanvasRot = gameCam.canvasCam != null ? gameCam.canvasCam.transform.rotation : Quaternion.identity;

            float origMainFov = gameCam.camCom.fieldOfView;
            float origBillFov = gameCam.billboardCam != null ? gameCam.billboardCam.fieldOfView : 60f;

            RenderTexture origTarget = gameCam.camCom.targetTexture;
            Quaternion origBillboardUpdaterRot = BillboardUpdater.camRot;

            gameCam.camCom.fieldOfView = 90f;
            if (gameCam.billboardCam) gameCam.billboardCam.fieldOfView = 90f;
            if (gameCam.canvasCam) gameCam.canvasCam.fieldOfView = 90f;

            gameCam.camCom.targetTexture = faceRT;

            Quaternion[] faceRots = new Quaternion[] {
            Quaternion.LookRotation(Vector3.right, Vector3.up),
            Quaternion.LookRotation(Vector3.left, Vector3.up),
            Quaternion.LookRotation(Vector3.up, Vector3.back),
            Quaternion.LookRotation(Vector3.down, Vector3.forward),
            Quaternion.LookRotation(Vector3.forward, Vector3.up),
            Quaternion.LookRotation(Vector3.back, Vector3.up)
        };

            for (int i = 0; i < 6; i++)
            {
                Quaternion rot = faceRots[i];

                gameCam.camCom.transform.rotation = rot;
                if (gameCam.billboardCam) gameCam.billboardCam.transform.rotation = rot;
                if (gameCam.canvasCam) gameCam.canvasCam.transform.rotation = rot;

                BillboardUpdater.camRot = rot;

                for (int j = 0; j < updaters.Length; j++)
                {
                    if (updaters[j] != null)
                    {
                        Vector3 up = updaters[j].transform.parent != null ? updaters[j].transform.parent.up : Vector3.up;
                        updaters[j].transform.LookAt(updaters[j].transform.position + rot * Vector3.forward, up);
                    }
                }

                gameCam.camCom.ResetCullingMatrix();
                if (gameCam.billboardCam) gameCam.billboardCam.ResetCullingMatrix();
                if (gameCam.canvasCam) gameCam.canvasCam.ResetCullingMatrix();

                gameCam.camCom.Render();

                RenderTexture.active = faceRT;
                tempFace.ReadPixels(new Rect(0, 0, cubemapSize, cubemapSize), 0, 0);

                Color32[] rawPixels = tempFace.GetPixels32();
                Color32[] flippedPixels = new Color32[rawPixels.Length];

                for (int y = 0; y < cubemapSize; y++)
                {
                    int srcRow = y * cubemapSize;
                    int dstRow = (cubemapSize - 1 - y) * cubemapSize;
                    Array.Copy(rawPixels, srcRow, flippedPixels, dstRow, cubemapSize);
                }

                faces[i] = flippedPixels;
            }

            gameCam.camCom.transform.rotation = origMainRot;
            if (gameCam.billboardCam) gameCam.billboardCam.transform.rotation = origBillRot;
            if (gameCam.canvasCam) gameCam.canvasCam.transform.rotation = origCanvasRot;

            gameCam.camCom.fieldOfView = origMainFov;
            if (gameCam.billboardCam) gameCam.billboardCam.fieldOfView = origBillFov;

            gameCam.camCom.targetTexture = origTarget;

            BillboardUpdater.camRot = origBillboardUpdaterRot;
            for (int i = 0; i < updaters.Length; i++)
            {
                if (updaters[i] != null) updaters[i].transform.rotation = origUpdaterRots[i];
            }

            RenderTexture.active = activeRT;
            UnityEngine.Object.DestroyImmediate(tempFace);
            RenderTexture.ReleaseTemporary(faceRT);

            Color32[] eqPixels = new Color32[outWidth * outHeight];
            ConvertCubemapToEquirectangular(faces, cubemapSize, eqPixels, outWidth, outHeight, paddingX);

            Texture2D output = new Texture2D(outWidth, outHeight, TextureFormat.RGBA32, false);
            output.SetPixels32(eqPixels);
            output.Apply();

            byte[] bytes = encodeAsJPEG ? InsertXMPIntoTexture2D_JPEG(output) : InsertXMPIntoTexture2D_PNG(output);
            UnityEngine.Object.DestroyImmediate(output);

            if (asyncCallback != null) asyncCallback(bytes);

            return bytes;
        }


        private static byte[] DoSyncConversion(RenderTexture cubemap, int cubemapSize, int outWidth, int outHeight, float paddingX, bool encodeAsJPEG, Action<byte[]> asyncCallback)
        {
            Color32[][] faces = new Color32[6][];
            Texture2D tempFace = new Texture2D(cubemapSize, cubemapSize, TextureFormat.RGBA32, false);
            RenderTexture activeRT = RenderTexture.active;

            for (int i = 0; i < 6; i++)
            {
                Graphics.SetRenderTarget(cubemap, 0, (CubemapFace)i);
                tempFace.ReadPixels(new Rect(0, 0, cubemapSize, cubemapSize), 0, 0);
                faces[i] = tempFace.GetPixels32();
            }
            RenderTexture.active = activeRT;
            UnityEngine.Object.DestroyImmediate(tempFace);

            Color32[] eqPixels = new Color32[outWidth * outHeight];
            ConvertCubemapToEquirectangular(faces, cubemapSize, eqPixels, outWidth, outHeight, paddingX);

            Texture2D output = new Texture2D(outWidth, outHeight, TextureFormat.RGBA32, false);
            output.SetPixels32(eqPixels);
            output.Apply();

            byte[] bytes = encodeAsJPEG ? InsertXMPIntoTexture2D_JPEG(output) : InsertXMPIntoTexture2D_PNG(output);
            UnityEngine.Object.DestroyImmediate(output);

            if (asyncCallback != null)
                asyncCallback(bytes);

            return bytes;
        }

        private static void ConvertCubemapToEquirectangular(Color32[][] faces, int cubemapSize, Color32[] outputPixels, int outWidth, int outHeight, float paddingX)
        {
            double PI = Math.PI;
            double TWOPI = Math.PI * 2.0;

            Parallel.For(0, outHeight, y =>
            {
                float v = (float)y / (outHeight - 1);
                float theta = v * (float)PI;
                float sinTheta = (float)Math.Sin(theta);
                float cosTheta = (float)Math.Cos(theta);

                for (int x = 0; x < outWidth; x++)
                {
                    float u = (float)x / (outWidth - 1);
                    float phi = (u + paddingX) * (float)TWOPI;

                    // Replicate exact shader math unit directions
                    float dx = (float)-Math.Sin(phi) * sinTheta;
                    float dy = -cosTheta;
                    float dz = (float)-Math.Cos(phi) * sinTheta;

                    float ax = Math.Abs(dx);
                    float ay = Math.Abs(dy);
                    float az = Math.Abs(dz);

                    int faceIndex = 0;
                    float uc = 0, vc = 0, maxA = 0;

                    // Map to cubemap face based on highest axis magnitude
                    if (ax >= ay && ax >= az)
                    {
                        maxA = ax;
                        if (dx > 0) { faceIndex = 0; uc = -dz; vc = dy; } // +X
                        else { faceIndex = 1; uc = dz; vc = dy; }  // -X
                    }
                    else if (ay >= ax && ay >= az)
                    {
                        maxA = ay;
                        if (dy > 0) { faceIndex = 2; uc = dx; vc = -dz; } // +Y
                        else { faceIndex = 3; uc = dx; vc = dz; }  // -Y
                    }
                    else
                    {
                        maxA = az;
                        if (dz > 0) { faceIndex = 4; uc = dx; vc = dy; }  // +Z
                        else { faceIndex = 5; uc = -dx; vc = dy; } // -Z
                    }

                    // Transform to 0..1 range UV coordinates
                    float uf = 0.5f * (uc / maxA + 1f);
                    float vf = 0.5f * (1f - vc / maxA);

                    // Quick Bilinear Filter Implementation
                    float fX = uf * (cubemapSize - 1);
                    float fY = vf * (cubemapSize - 1);

                    int x0 = (int)fX;
                    int y0 = (int)fY;
                    int x1 = x0 + 1 < cubemapSize ? x0 + 1 : x0;
                    int y1 = y0 + 1 < cubemapSize ? y0 + 1 : y0;

                    float tx = fX - x0;
                    float ty = fY - y0;
                    float itx = 1f - tx;
                    float ity = 1f - ty;

                    float w00 = itx * ity;
                    float w10 = tx * ity;
                    float w01 = itx * ty;
                    float w11 = tx * ty;

                    Color32[] face = faces[faceIndex];
                    Color32 c00 = face[y0 * cubemapSize + x0];
                    Color32 c10 = face[y0 * cubemapSize + x1];
                    Color32 c01 = face[y1 * cubemapSize + x0];
                    Color32 c11 = face[y1 * cubemapSize + x1];

                    byte r = (byte)(c00.r * w00 + c10.r * w10 + c01.r * w01 + c11.r * w11);
                    byte g = (byte)(c00.g * w00 + c10.g * w10 + c01.g * w01 + c11.g * w11);
                    byte b = (byte)(c00.b * w00 + c10.b * w10 + c01.b * w01 + c11.b * w11);
                    byte a = (byte)(c00.a * w00 + c10.a * w10 + c01.a * w01 + c11.a * w11);

                    outputPixels[y * outWidth + x] = new Color32(r, g, b, a);
                }
            });
        }

        #region XMP Injection
        private const string XMP_NAMESPACE_JPEG = "http://ns.adobe.com/xap/1.0/";
        private const string XMP_CONTENT_TO_FORMAT_JPEG = "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" x:xmptk=\"Adobe XMP Core 5.1.0-jc003\"> <rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"> <rdf:Description rdf:about=\"\" xmlns:GPano=\"http://ns.google.com/photos/1.0/panorama/\" GPano:UsePanoramaViewer=\"True\" GPano:CaptureSoftware=\"Unity3D\" GPano:StitchingSoftware=\"Unity3D\" GPano:ProjectionType=\"equirectangular\" GPano:PoseHeadingDegrees=\"180.0\" GPano:InitialViewHeadingDegrees=\"0.0\" GPano:InitialViewPitchDegrees=\"0.0\" GPano:InitialViewRollDegrees=\"0.0\" GPano:InitialHorizontalFOVDegrees=\"{0}\" GPano:CroppedAreaLeftPixels=\"0\" GPano:CroppedAreaTopPixels=\"0\" GPano:CroppedAreaImageWidthPixels=\"{1}\" GPano:CroppedAreaImageHeightPixels=\"{2}\" GPano:FullPanoWidthPixels=\"{1}\" GPano:FullPanoHeightPixels=\"{2}\"/></rdf:RDF></x:xmpmeta>";
        private const string XMP_CONTENT_TO_FORMAT_PNG = "XML:com.adobe.xmp\0\0\0\0\0<?xpacket begin=\"ï»¿\" id=\"W5M0MpCehiHzreSzNTczkc9d\"?><x:xmpmeta xmlns:x=\"adobe:ns:meta/\" x:xmptk=\"Adobe XMP Core 5.1.0-jc003\"> <rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"> <rdf:Description rdf:about=\"\" xmlns:GPano=\"http://ns.google.com/photos/1.0/panorama/\" xmlns:xmp=\"http://ns.adobe.com/xap/1.0/\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:xmpMM=\"http://ns.adobe.com/xap/1.0/mm/\" xmlns:stEvt=\"http://ns.adobe.com/xap/1.0/sType/ResourceEvent#\" xmlns:tiff=\"http://ns.adobe.com/tiff/1.0/\" xmlns:exif=\"http://ns.adobe.com/exif/1.0/\"> <GPano:UsePanoramaViewer>True</GPano:UsePanoramaViewer> <GPano:CaptureSoftware>Unity3D</GPano:CaptureSoftware> <GPano:StitchingSoftware>Unity3D</GPano:StitchingSoftware> <GPano:ProjectionType>equirectangular</GPano:ProjectionType> <GPano:PoseHeadingDegrees>180.0</GPano:PoseHeadingDegrees> <GPano:InitialViewHeadingDegrees>0.0</GPano:InitialViewHeadingDegrees> <GPano:InitialViewPitchDegrees>0.0</GPano:InitialViewPitchDegrees> <GPano:InitialViewRollDegrees>0.0</GPano:InitialViewRollDegrees> <GPano:InitialHorizontalFOVDegrees>{0}</GPano:InitialHorizontalFOVDegrees> <GPano:CroppedAreaLeftPixels>0</GPano:CroppedAreaLeftPixels> <GPano:CroppedAreaTopPixels>0</GPano:CroppedAreaTopPixels> <GPano:CroppedAreaImageWidthPixels>{1}</GPano:CroppedAreaImageWidthPixels> <GPano:CroppedAreaImageHeightPixels>{2}</GPano:CroppedAreaImageHeightPixels> <GPano:FullPanoWidthPixels>{1}</GPano:FullPanoWidthPixels> <GPano:FullPanoHeightPixels>{2}</GPano:FullPanoHeightPixels> <tiff:Orientation>1</tiff:Orientation> <exif:PixelXDimension>{1}</exif:PixelXDimension> <exif:PixelYDimension>{2}</exif:PixelYDimension> </rdf:Description></rdf:RDF></x:xmpmeta><?xpacket end=\"w\"?>";
        private static uint[] CRC_TABLE_PNG = null;

        public static byte[] InsertXMPIntoTexture2D_JPEG(Texture2D image)
        {
            return DoTheHardWork_JPEG(image.EncodeToJPG(100), image.width, image.height);
        }

        public static byte[] InsertXMPIntoTexture2D_PNG(Texture2D image)
        {
            return DoTheHardWork_PNG(image.EncodeToPNG(), image.width, image.height);
        }

        #region JPEG Encoding
        private static byte[] DoTheHardWork_JPEG(byte[] fileBytes, int imageWidth, int imageHeight)
        {
            int xmpIndex = 0, xmpContentSize = 0;
            while (!SearchChunkForXMP_JPEG(fileBytes, ref xmpIndex, ref xmpContentSize))
            {
                if (xmpIndex == -1)
                    break;
            }

            int copyBytesUntil, copyBytesFrom;
            if (xmpIndex == -1)
            {
                copyBytesUntil = copyBytesFrom = FindIndexToInsertXMPCode_JPEG(fileBytes);
            }
            else
            {
                copyBytesUntil = xmpIndex;
                copyBytesFrom = xmpIndex + 2 + xmpContentSize;
            }

            string xmpContent = string.Concat(XMP_NAMESPACE_JPEG, "\0", string.Format(XMP_CONTENT_TO_FORMAT_JPEG, 75f.ToString("F1"), imageWidth, imageHeight));
            int xmpLength = xmpContent.Length + 2;
            xmpContent = string.Concat((char)0xFF, (char)0xE1, (char)(xmpLength / 256), (char)(xmpLength % 256), xmpContent);

            byte[] result = new byte[copyBytesUntil + xmpContent.Length + (fileBytes.Length - copyBytesFrom)];

            Array.Copy(fileBytes, 0, result, 0, copyBytesUntil);

            for (int i = 0; i < xmpContent.Length; i++)
            {
                result[copyBytesUntil + i] = (byte)xmpContent[i];
            }

            Array.Copy(fileBytes, copyBytesFrom, result, copyBytesUntil + xmpContent.Length, fileBytes.Length - copyBytesFrom);

            return result;
        }

        private static bool CheckBytesForXMPNamespace_JPEG(byte[] bytes, int startIndex)
        {
            for (int i = 0; i < XMP_NAMESPACE_JPEG.Length; i++)
            {
                if (bytes[startIndex + i] != XMP_NAMESPACE_JPEG[i])
                    return false;
            }

            return true;
        }

        private static bool SearchChunkForXMP_JPEG(byte[] bytes, ref int startIndex, ref int chunkSize)
        {
            if (startIndex + 4 < bytes.Length)
            {
                if (bytes[startIndex] == 0xFF)
                {
                    byte secondByte = bytes[startIndex + 1];
                    if (secondByte == 0xDA)
                    {
                        startIndex = -1;
                        return false;
                    }
                    else if (secondByte == 0x01 || (secondByte >= 0xD0 && secondByte <= 0xD9))
                    {
                        startIndex += 2;
                        return false;
                    }
                    else
                    {
                        chunkSize = bytes[startIndex + 2] * 256 + bytes[startIndex + 3];

                        if (secondByte == 0xE1 && chunkSize >= 31 && CheckBytesForXMPNamespace_JPEG(bytes, startIndex + 4))
                        {
                            return true;
                        }

                        startIndex = startIndex + 2 + chunkSize;
                    }
                }
            }

            return false;
        }

        private static int FindIndexToInsertXMPCode_JPEG(byte[] bytes)
        {
            int chunkSize = bytes[4] * 256 + bytes[5];
            return chunkSize + 4;
        }
        #endregion

        #region PNG Encoding
        private static byte[] DoTheHardWork_PNG(byte[] fileBytes, int imageWidth, int imageHeight)
        {
            string xmpContent = "iTXt" + string.Format(XMP_CONTENT_TO_FORMAT_PNG, 75f.ToString("F1"), imageWidth, imageHeight);
            int copyBytesUntil = 33;
            int xmpLength = xmpContent.Length - 4; // minus iTXt
            string xmpCRC = CalculateCRC_PNG(xmpContent);
            xmpContent = string.Concat((char)(xmpLength >> 24), (char)(xmpLength >> 16), (char)(xmpLength >> 8), (char)(xmpLength),
                                        xmpContent, xmpCRC);

            byte[] result = new byte[fileBytes.Length + xmpContent.Length];

            Array.Copy(fileBytes, 0, result, 0, copyBytesUntil);

            for (int i = 0; i < xmpContent.Length; i++)
            {
                result[copyBytesUntil + i] = (byte)xmpContent[i];
            }

            Array.Copy(fileBytes, copyBytesUntil, result, copyBytesUntil + xmpContent.Length, fileBytes.Length - copyBytesUntil);

            return result;
        }

        private static string CalculateCRC_PNG(string xmpContent)
        {
            if (CRC_TABLE_PNG == null)
                CalculateCRCTable_PNG();

            uint crc = ~UpdateCRC_PNG(xmpContent);
            byte[] crcBytes = CalculateCRCBytes_PNG(crc);

            return string.Concat((char)crcBytes[0], (char)crcBytes[1], (char)crcBytes[2], (char)crcBytes[3]);
        }

        private static uint UpdateCRC_PNG(string xmpContent)
        {
            uint c = 0xFFFFFFFF;
            for (int i = 0; i < xmpContent.Length; i++)
            {
                c = (c >> 8) ^ CRC_TABLE_PNG[xmpContent[i] ^ c & 0xFF];
            }

            return c;
        }

        private static void CalculateCRCTable_PNG()
        {
            CRC_TABLE_PNG = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint c = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((c & 1) == 1)
                        c = (c >> 1) ^ 0xEDB88320;
                    else
                        c = (c >> 1);
                }

                CRC_TABLE_PNG[i] = c;
            }
        }

        private static byte[] CalculateCRCBytes_PNG(uint crc)
        {
            var result = BitConverter.GetBytes(crc);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
        #endregion
        #endregion
    }
}
