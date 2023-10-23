using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace KitchenChaos.Services {
    public static class Relay {
        public static async Task<Allocation> Allocate() {
            try {
                var allocation = await RelayService.Instance.CreateAllocationAsync(NetworkService.MAX_PLAYERS);
                return allocation;
            } catch (RelayServiceException e) {
                Debug.Log(e);
                return default!;
            }
        }

        public static async Task<string> JoinCode(Allocation allocation) {
            try {
                var code = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                return code;
            } catch (RelayServiceException e) {
                Debug.Log(e);
                return default!;
            }
        }

        public static async Task<JoinAllocation> Join(string joinCode) {
            try {
                var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                return allocation;
            } catch (RelayServiceException e) {
                Debug.Log(e);
                return default!;
            }
        }
    }
}
