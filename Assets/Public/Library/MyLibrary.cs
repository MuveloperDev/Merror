using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using System;
using System.Security.Cryptography;
using TMPro;
using Cinemachine;
using Newtonsoft.Json.Bson;

namespace MyLibrary
{
    public static class Pool
    {
        /// <summary>
        /// (KEY)<string> -> Object's Name , (VALUE)<Queue> -> Containing Pooling objects
        /// </summary>
        private static Dictionary<string, Queue<GameObject>> listCache = new Dictionary<string, Queue<GameObject>>();

        /// <summary>
        /// Standard Object Pooling
        /// </summary>
        /// <param name="instObj"> Object to be created </param>
        /// <param name="position"> Position to be created </param>
        /// <param name="rotation"> Rotation to be created </param>
        /// <returns> Pooled Object or Created Object </returns>
        public static GameObject ObjectInstantiate(GameObject instObj, Vector3 position, Quaternion rotation)
        {
            string objectID = instObj.name;

            Queue<GameObject> que = null;

            ListCaching(instObj);

            bool listCached = listCache.TryGetValue(objectID, out que);

            if (listCached == false)
            {
                Debug.LogError("Can not Add " + objectID + " Please Request BK");
                return null;
            }

            GameObject inst = null;

            if (que.Count == 0)
            {
                inst = MonoBehaviour.Instantiate(instObj, position, rotation);
            }
            else
            {
                inst = que.Dequeue();
                inst.transform.position = position;
                inst.transform.rotation = rotation;
            }

            inst.SetActive(true);
            return inst;
        }

        /// <summary>
        /// Overloading Standard Pooling for Set Object's Parent 
        /// </summary>
        /// <param name="instObj"> Object to be created </param>
        /// <param name="parent"> instObj's parent </param>
        /// <param name="worldPositionStays"> If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before. </param>
        /// <returns> Pooled Object or Created Object </returns>
        public static GameObject ObjectInstantiate(GameObject instObj, Transform parent, bool worldPositionStays)
        {
            instObj = ObjectInstantiate(instObj, instObj.transform.position, instObj.transform.rotation);

            instObj.transform.SetParent(parent, worldPositionStays);

            return instObj;
        }

        /// <summary>
        /// SetActived false Obj and Enqueue for instObj Queue 
        /// </summary>
        /// <param name="instObj"> Request Destroy Obj </param>
        public static void ObjectDestroy(GameObject instObj)
        {
            string objectID = instObj.name.Replace("(Clone)", "");

            Queue<GameObject> que = null;
            bool listCached = listCache.TryGetValue(objectID, out que);
            if (listCached == false)
            {
                Debug.LogError(objectID + " is not using pool Object");
                return;
            }

            instObj.SetActive(false);
            que.Enqueue(instObj);
        }

        /// <summary>
        /// Cashing Before Using Instantiate
        /// </summary>
        /// <param name="instObj"> cashing obj </param>
        public static void ListCaching(GameObject instObj)
        {
            string objectID = instObj.name;

            bool listCached = listCache.ContainsKey(objectID);
            if (listCached == false)
            {
                listCache.Add(objectID, new Queue<GameObject>());
            }
            else
                return;
        }

        /// <summary>
        /// Overloading single caching for array caching
        /// </summary>
        /// <param name="instObj"> Caching obj array </param>
        public static void ListCaching(GameObject[] instObj)
        {
            string[] objectID = new string[instObj.Length];

            for (int i = 0; i < objectID.Length; i++)
            {
                objectID[i] = instObj[i].name;

                bool listCached = listCache.ContainsKey(objectID[i]);

                if (listCached == false)
                {
                    listCache.Add(objectID[i], new Queue<GameObject>());
                }
                else
                    continue;
            }
        }

    }

    public struct SaveData
    {
        public string chapter;
        public Vector3 playerPos;
        public Quaternion playerRot;
        public ChpterPuzzles puzzles;
        public Door[] isDoorOpen;
        public string[] invenItems;
    }

    public struct ChpterPuzzles 
    {
        public bool[] isCleared;

        public ChpterPuzzles(int count)
        {
            isCleared = new bool[count];
        }
    }

    public struct Door 
    {
        public string doorName;
        public bool isOpened;

        public Door(string doorName,bool isOpened)
        {
            this.doorName = doorName;
            this.isOpened = isOpened;
        }
    }

    /// <summary>
    /// Save Load By Bat File 
    /// </summary>
    /// <typeparam name="SaveData"> Constraint : T -> ScriptableObject </typeparam>
    public static class SaveLoad
    {
        private static readonly string defaultPath = System.IO.Directory.GetCurrentDirectory();
        private static readonly string savePath = defaultPath + "/MakeFile.bat";
        private static readonly string loadPath = defaultPath + "/ReadFile.bat";

        private static byte[] myKey = null;
        private static byte[] myIV = null;

        static SaveLoad()
        {
            myKey = Convert.FromBase64String("FsaPKfHqao6CB26XJ/TlldRaFxCg7141pQE2gHx2iRw=");
            myIV = Convert.FromBase64String("ZL4IxFBvQzlnirk5wZVIxw==");
        }


        /// <summary>
        /// Save File in SaveData Folder
        /// Data Type is Json
        /// </summary>
        /// <param name="data"> Data to Save  </param>
        public static void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            json = json.Replace(",", "$");
            json = Convert.ToBase64String(Encryption(myKey, myIV, json));
            json = json.Replace("=", "@");

            System.Diagnostics.Process.Start(savePath, json).WaitForExit();
        }

        /// <summary>
        /// Load Data.json File
        /// </summary>
        /// <param name="myData"> OverWritten Player Current Data </param>
        /// <returns> OverWritten Scriptable Object </returns>
        public static SaveData? Load()
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = loadPath;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;

            System.Diagnostics.Process loadProcess = System.Diagnostics.Process.Start(psi);

            string encodingString = null;
            while (true)
            {
                string parsing = null;
                if ((parsing = loadProcess.StandardOutput.ReadLine()) != null)
                {
                    encodingString = parsing;
                }

                if (parsing == null)
                    break;

            }
            SaveData? data = null;

            encodingString = encodingString.Replace("@", "=");

            encodingString = Descryption(myKey, myIV, Convert.FromBase64String(encodingString));
            data = JsonUtility.FromJson<SaveData>(encodingString.Replace("$", ","));

            return data;
        }

        /// <summary>
        /// Encryptor Json String Data
        /// </summary>
        /// <param name="myKey"> Encrytor Decryptor Key </param>
        /// <param name="myIV"> Key Vector </param>
        /// <param name="data"> Json String Data </param>
        /// <returns> Encyptored Data Data by byte Array  </returns>
        private static byte[] Encryption(byte[] myKey, byte[] myIV, string data)
        {
            byte[] encryptoed = null;

            using (Aes enAes = Aes.Create())
            {
                enAes.Key = myKey;
                enAes.IV = myIV;

                // ��ȣȭ ������ �⺻ �۾� ����
                ICryptoTransform encrypto = enAes.CreateEncryptor(enAes.Key, enAes.IV);

                // ��� ����Ҹ� �޸𸮷� ����
                using (MemoryStream msEncryto = new MemoryStream())
                {
                    using (CryptoStream enCrStream = new CryptoStream(msEncryto, encrypto, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypto = new StreamWriter(enCrStream))
                        {
                            swEncrypto.Write(data);
                        }
                        encryptoed = msEncryto.ToArray();
                    }
                }
            }
            return encryptoed;
        }

        /// <summary>
        /// Decyriptor Json String Data
        /// </summary>
        /// <param name="myKey"> Encrytor Decryptor Key </param>
        /// <param name="myIV"> Key Vector </param>
        /// <param name="data"> Encrytored Data byte Array</param>
        /// <returns> Decryptored Data string Data </returns>
        private static string Descryption(byte[] myKey, byte[] myIV, byte[] data)
        {
            string result = null;
            using (Aes deAes = Aes.Create())
            {
                deAes.Key = myKey;
                deAes.IV = myIV;

                ICryptoTransform decryto = deAes.CreateDecryptor(deAes.Key, deAes.IV);

                using (MemoryStream msDescryto = new MemoryStream(data))
                {
                    using (CryptoStream deCrStream = new CryptoStream(msDescryto, decryto, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDescrypto = new StreamReader(deCrStream))
                        {
                            result = srDescrypto.ReadToEnd();
                        }
                    }
                }

            }
            return result;
        }
    }

    /// <summary>
    /// Pause And Play Game
    /// </summary>
    public static class TimeControl
    {
        public delegate void PauseGame();
        public delegate void PlayGame();
        public static PauseGame Pause = null;
        public static PlayGame Play = null;

        static TimeControl()
        {
            Pause = () =>
            {
                Time.timeScale = 0f;
                //Time.fixedDeltaTime = Time.fixedTime * Time.timeScale;
            };

            Play = () =>
            {
                Time.timeScale = 1f;
                //Time.fixedDeltaTime = Time.fixedTime * Time.timeScale;
            };
        }
    }

    public static class MyRay
    {
        private static RaycastHit? lastHit = null; // Last hit object
        private static RaycastHit currentHit; // Current hitting object
        private static bool isRayExit = false; // Is my ray point another one?
        private static Interactable interObj = null; // Can interactable object
        public static Interactable GetInterObj() => interObj;
        public static bool IsRayExit { get { return isRayExit; } }

        private static CinemachineVirtualCamera startPointCam = null; // Starting point of camera

        /// <summary>
        /// Shoot Ray Main Logic
        /// </summary>
        private static void ShootRay(float maxDistance, bool isClicked)
        {
            if (Time.timeScale == 0f)
                return;

            Vector3 direction;
            Vector3 startPos;

            if (startPointCam != null) // If received camera information
            {
                startPos = startPointCam.gameObject.transform.position;
                direction = startPointCam.gameObject.transform.forward;
            }
            else // Just for main camera
            {
                startPos = Camera.main.transform.position;
                direction = Camera.main.transform.forward;
            }
            Debug.DrawRay(startPos, direction * maxDistance, Color.red);
            if (Physics.Raycast(startPos, direction, out currentHit, maxDistance)) // Hit something
            { 

                if (lastHit == null) // When game start at first
                {
                    isRayExit = true;
                    lastHit = currentHit;
                }
                else // When playing,
                {
                    if (isRayExit = !(lastHit.Equals(currentHit))) // Something have changes
                    {
                        // Turn off the outline last object's. if it can interactable.
                        if (lastHit.Value.transform.TryGetComponent<Interactable>(out interObj))
                        {
                            interObj.SendMessage("Do_Outline", false, SendMessageOptions.DontRequireReceiver);
                        }
                        // Turn on the outline new object's. if it can interactable.
                        if (currentHit.transform.TryGetComponent<Interactable>(out interObj))
                        {
                            interObj.SendMessage("Do_Outline", true, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
                if (isClicked == true && interObj != null) // Click down and can interactable
                {
                    // Do something
                    interObj.SendMessage("Do_Interact", SendMessageOptions.DontRequireReceiver);
                }
                lastHit = currentHit; // Initialize
            }
            else // Ray didn't hit anything.
            {
                if (lastHit != null) // If have last hit information,
                {
                    // Turn off last one.
                    if (lastHit.Value.transform.TryGetComponent<Interactable>(out interObj))
                    {
                        interObj.SendMessage("Do_Outline", false, SendMessageOptions.DontRequireReceiver);
                    }
                }
                isRayExit = true; // Ray is exit.
                lastHit = null; // Initialize
            }
        }

        /// <summary>
        /// Default Ray(Start Main Camera)
        /// </summary>
        /// <param name="maxDistance"> Raycast Max distance </param>
        /// <param name="isClicked"> Is input costom Key (Keyboard or Mouse) </param>
        public static void StartRay(float maxDistance, bool isClicked)
        {
            startPointCam = null;
            ShootRay(maxDistance, isClicked);
        }

        /// <summary>
        /// Custom Ray(Start Param's Cinemachine Virtual Camera)
        /// </summary>
        /// <param name="pointCamera"> Starting Point Camera </param>
        /// <param name="maxDistance"> Raycast Max distance </param>
        /// <param name="isClicked"> Is input costom Key (Keyboard or Mouse) </param>
        public static void StartRay(CinemachineVirtualCamera pointCam, float maxDistance, bool isClicked)
        {
            startPointCam = pointCam;
            ShootRay(maxDistance, isClicked);
        }
    }

    /// <summary>
    /// GameObject Inventory Class
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        private int InventoryLayer;
        private TextMeshProUGUI notice = null;
        private Canvas canvas = null;
        private Camera uiCamera = null;

        private List<GameObject> inven = null;
        public int GetItemCount { get { return inven.Count; } }
        private int count = 0;

        public List<GameObject> GetInventoryItem()
        {
            return inven;
        }

        /// <summary>
        /// Class Constructor Initializing Members
        /// </summary>
        /// <param name="notice"> Notice Noting in inventory TMP </param>
        /// <param name="canvas"> BackGround Canvas (Background Panel and Notice in the Canvas) </param>
        /// <param name="InventoryLayer"> Inventory Layer's Value </param>
        public Inventory(TextMeshProUGUI notice, Canvas canvas)
        {
            inven = new List<GameObject>();

            // UI Camera is Show only Inventory UI
            // Main Camera is Shouldn't Show Inventory UI
            if (uiCamera == null)
            {
                uiCamera = new GameObject("UICAM").AddComponent<Camera>();
                uiCamera.clearFlags = CameraClearFlags.Depth;
                uiCamera.cullingMask = 0;
                uiCamera.cullingMask |= 1 << LayerMask.NameToLayer("Inventory");
                uiCamera.cullingMask |= 1 << LayerMask.NameToLayer("UI");
                uiCamera.gameObject.SetActive(false);

                Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Inventory"));
                Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("UI"));
            }

            this.notice = notice;
            this.canvas = canvas;
            InventoryLayer = LayerMask.NameToLayer("Inventory");

            canvas.planeDistance = 6;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uiCamera;
            canvas.gameObject.SetActive(false);
        }



        /// <summary>
        /// Get Item and insert inventory list
        /// </summary>
        /// <param name="item"> acquired item </param>
        public void InsertItem(GameObject item, float scale)
        {
            Interactable interObj = null;

            if (item.TryGetComponent<Interactable>(out interObj) == true)
            {
                Debug.Log(InventoryLayer);
                item.layer = InventoryLayer;
                item.transform.localScale = new Vector3(scale, scale, scale);
                item.transform.SetParent(uiCamera.transform, false);
                Destroy(interObj);
                item.SetActive(false);
                inven.Add(item);
            }
            else
            {
                return;
            }

        }

        /// <summary>
        /// Show Inventory List as a 3D Object
        /// GamePlaying state is Pause
        /// </summary>
        public void ShowInventory()
        {
            TimeControl.Pause();

            uiCamera.gameObject.gameObject.SetActive(true);

            Vector3 cameraPos = Camera.main.transform.position;
            uiCamera.transform.position = cameraPos;
            uiCamera.transform.rotation = Camera.main.transform.rotation;

            canvas.gameObject.SetActive(true);

            if (inven.Count != 0)
            {
                notice.gameObject.SetActive(false);
                inven[count].SetActive(true);
                inven[count].transform.localPosition = new Vector3(0, 0, 5);
                inven[count].transform.rotation = Quaternion.identity;
            }
            else
            {
                notice.gameObject.SetActive(true);
            }

        }

        /// <summary>
        /// Hide Inventory List
        /// GamePlaying state is Play
        /// </summary>
        public void HideInventory()
        {
            count = 0;
            uiCamera.gameObject.SetActive(false);
            canvas.gameObject.SetActive(false);

            TimeControl.Play();
        }

        /// <summary>
        /// Show next item
        /// </summary>
        public void NextItem()
        {
            if (inven.Count == 0 || inven.Count == 1)
                return;

            count++;

            if (count >= inven.Count)
            {
                count = 0;
                inven[count].transform.localPosition = inven[inven.Count - 1].transform.localPosition;
                inven[inven.Count - 1].SetActive(false);
            }
            else
            {
                inven[count].transform.localPosition = inven[count - 1].transform.localPosition;
                inven[count - 1].SetActive(false);
            }

            inven[count].SetActive(true);
            inven[count].transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Show Prev Item
        /// </summary>
        public void PrevItem()
        {
            if (inven.Count == 0 || inven.Count == 1)
                return;

            count--;

            if (count < 0)
            {
                count = inven.Count - 1;
                inven[count].transform.localPosition = inven[0].transform.localPosition;
                inven[0].SetActive(false);
            }
            else
            {
                inven[count].transform.localPosition = inven[count + 1].transform.localPosition;
                inven[count + 1].SetActive(false);
            }

            inven[count].SetActive(true);
            inven[count].transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// When Mouse Click And Drag 3D Object is Rotate
        /// </summary>
        public void RotationItem()
        {
            if (Input.GetMouseButton(0))
            {
                float mouseXAxis = Input.GetAxis("Mouse X");
                float mouseYAxis = Input.GetAxis("Mouse Y");

                inven[count].transform.Rotate(mouseYAxis * 5f, -mouseXAxis * 5f, 0f);
            }
        }

        /// <summary>
        /// Checking List is have interactable objects
        /// </summary>
        /// <param name="objID"> interactable objects name </param>
        /// <returns> 
        /// True is have interactable objects and Remove Obj
        /// False is interactable objects is not in List
        /// </returns>
        public GameObject UseItem(string objID)
        {
            foreach (GameObject iObj in inven)
            {
                string iObjName = iObj.name.Replace("(Clone)", "");

                if (iObjName == objID)
                {
                    inven.Remove(iObj);
                    return iObj;
                }
            }

            return null;
        }

        /// <summary>
        /// Clear List
        /// </summary>
        public void ClearItem()
        {
            inven.Clear();
        }

    }
    public static class GameInput
    {
        public static float MouseX = 0f;
        public static float MouseY = 0f;
        public static float Clamped_Delta_Mouse_Y = 0f;
        public static bool LeftShift { get; private set; } = false;
        public static bool LeftShiftDown { get; private set; } = false;
        public static bool LeftShiftUp { get; private set; } = false;
        public static bool LeftCtrl { get; private set; } = false;
        public static bool LeftCtrlUp { get; private set; } = false;

        private static float XSpeed = 70f;
        public static void SetXSpeed(float speed) => XSpeed = speed;
        private static float YSpeed = 70f;
        public static void SetYSpeed(float speed) => YSpeed = speed;

        public static void UpdateKey()
        {
            MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * XSpeed;
            MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * YSpeed;

            ClampMouseY();

            LeftShift = Input.GetKey(KeyCode.LeftShift);
            LeftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
            LeftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);

            LeftCtrl = Input.GetKey(KeyCode.LeftControl);
            LeftCtrlUp = Input.GetKeyUp(KeyCode.LeftControl);
        }
        private static void ClampMouseY()
        {
            Clamped_Delta_Mouse_Y += MouseY; // Get delta
                                             // Clamp degree 0 to 360
            Clamped_Delta_Mouse_Y = Clamped_Delta_Mouse_Y > 180f ? Clamped_Delta_Mouse_Y - 360f : Clamped_Delta_Mouse_Y;
            Clamped_Delta_Mouse_Y = Mathf.Clamp(Clamped_Delta_Mouse_Y, -70f, 70f); // Clamp Range : (-Upside, -Downside)
        }
    }
}
 