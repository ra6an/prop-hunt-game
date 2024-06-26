using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayController : MonoBehaviour
{
    public static RelayController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> CreateRelay(int numOfPlayers)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(numOfPlayers);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            //OLDER VERSIONS
            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
            //    allocation.RelayServer.IpV4,
            //    (ushort)allocation.RelayServer.Port,
            //    allocation.AllocationIdBytes,
            //    allocation.Key,
            //    allocation.ConnectionData
            //);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return joinCode;
        } catch (RelayServiceException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // OLD METHOD
            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
            //    joinAllocation.RelayServer.IpV4,
            //    (ushort)joinAllocation.RelayServer.Port,
            //    joinAllocation.AllocationIdBytes,
            //    joinAllocation.Key,
            //    joinAllocation.ConnectionData,
            //    joinAllocation.HostConnectionData
            //);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }
}
