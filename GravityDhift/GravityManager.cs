using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;

namespace GravityShift
{
    public class GravityManager : MonoBehaviour
    {
        public static GravityManager instance;

        public Vector3 targetGravity;
        bool isMasterClient;
        public float lerpSpeed = 0.1f;
        public float maxY = -2f;
        public float minY = -4f;
        float defaultMagnitude;

        void OnEnable()
        {
            if(instance != this)
            {
                instance = this;
            }
            defaultMagnitude = Physics.gravity.magnitude;
            targetGravity = new Vector3(Random.Range(-1f, 1f), Random.Range(minY, maxY), Random.Range(-1f, 1f)).normalized * defaultMagnitude;
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash.Add("currentGravity", Physics.gravity);
            hash.Add("currentTargetGravity", Physics.gravity);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        void OnDisable()
        {
            Physics.gravity = Vector3.down * defaultMagnitude;
        }

        void Update()
        {
            isMasterClient = PhotonNetwork.IsMasterClient;
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

            if (isMasterClient)
            {
                if (Vector3.Distance(targetGravity, Physics.gravity) < 0.1f)
                {
                    targetGravity = new Vector3(Random.Range(-1f, 1f), Random.Range(minY, maxY), Random.Range(-1f, 1f)).normalized * defaultMagnitude;
                }                
                
                Physics.gravity = Vector3.Lerp(Physics.gravity, targetGravity, Time.deltaTime * lerpSpeed);
                hash["currentGravity"] = Physics.gravity;
                hash["currentTargetGravity"] = targetGravity;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            }
            else
            {
                Physics.gravity = (Vector3)PhotonNetwork.CurrentRoom.CustomProperties["currentGravity"];
                targetGravity = (Vector3)PhotonNetwork.CurrentRoom.CustomProperties["currentTargetGravity"];
            }
        }
    }
}