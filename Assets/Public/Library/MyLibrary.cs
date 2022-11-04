using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using System;
using System.Security.Cryptography;
using TMPro;

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

    /// <summary>
    /// Save Load By Bat File 
    /// </summary>
    /// <typeparam name="T"> Constraint : T -> ScriptableObject </typeparam>
    public static class SaveLoad<T> where T : ScriptableObject
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
        public static void Save(T data)
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
        public static T Load(T myData)
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
            T data = myData;

            encodingString = encodingString.Replace("@", "=");

            encodingString = Descryption(myKey, myIV, Convert.FromBase64String(encodingString));
            JsonUtility.FromJsonOverwrite(encodingString.Replace("$", ","), data);

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

                // 암호화 변형의 기본 작업 설정
                ICryptoTransform encrypto = enAes.CreateEncryptor(enAes.Key, enAes.IV);

                // 백업 저장소를 메모리로 만듬
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
                Time.fixedDeltaTime = 0.2f * Time.timeScale;
            };

            Play = () =>
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.2f * Time.timeScale;
            };
        }
    }

    public static class MyRay
    {
        private static RaycastHit? originalInfo = null;
        private static RaycastHit tempInfo;
        private static bool isChangeObj = false;
        private static Interactable interObj = null;

        public static bool IsChangeObj { get { return isChangeObj; } }

        /// <summary>
        /// Shot Raycast at Main Camera foward
        /// </summary>
        /// <param name="pointCamera"> Starting Point Camera </param>
        /// <param name="maxDistance"> Raycast Max distance </param>
        /// <param name="isClicked"> Is input costom Key (Keyboard or Mouse) </param>
        public static void StartRay(Camera pointCamera, float maxDistance, bool isClicked)
        {
            Vector3 direction = pointCamera.transform.forward;
            Debug.DrawRay(pointCamera.transform.position, direction, Color.red);

            if (Physics.Raycast(pointCamera.transform.position, direction, out tempInfo, maxDistance))
            {
                if (originalInfo == null)
                {
                    isChangeObj = true;
                    originalInfo = tempInfo;
                }
                else
                {
                    if (isChangeObj = !(originalInfo.Equals(tempInfo)))
                    {
                        originalInfo = null;
                        interObj = null;
                    }
                }

                if (interObj == null)
                {
                    if (!tempInfo.transform.gameObject.TryGetComponent<Interactable>(out interObj))
                        return;
                }

                if (isClicked == true)
                {
                    interObj.SendMessage("Do_Interact", SendMessageOptions.DontRequireReceiver);
                }
            }
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
        private int count = 0;

        /// <summary>
        /// Class Constructor Initializing Members
        /// </summary>
        /// <param name="notice"> Notice Noting in inventory TMP </param>
        /// <param name="canvas"> BackGround Canvas (Background Panel and Notice in the Canvas) </param>
        /// <param name="InventoryLayer"> Inventory Layer's Value </param>
        public Inventory(TextMeshProUGUI notice, Canvas canvas, int InventoryLayer)
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
                Camera.main.cullingMask = ~(-1 << LayerMask.NameToLayer("UI"));
            }

            this.notice = notice;
            this.canvas = canvas;
            this.InventoryLayer = InventoryLayer;

            canvas.planeDistance = 6;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uiCamera;
            canvas.gameObject.SetActive(false);
        }



        /// <summary>
        /// Get Item and insert inventory list
        /// </summary>
        /// <param name="item"> acquired item </param>
        public void InsertItem(GameObject item)
        {
            Interactable interObj = null;

            if (item.TryGetComponent<Interactable>(out interObj) == true)
            {
                item.layer = InventoryLayer;
                item.transform.SetParent(uiCamera.transform, true);
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
            float mouseXAxis = Input.GetAxis("Mouse X");
            float mouseYAxis = Input.GetAxis("Mouse Y");

            inven[count].transform.Rotate(mouseYAxis * 15f, -mouseXAxis * 15f, 0f);
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




}
