using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace KitchenChaos.Services {
    public class NetworkLobby : MonoSingleton<NetworkLobby> {
        private Lobby joined = null!;

        protected override void Awake() {
            if (Instance) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            base.Awake();
            InitUnityAuth();
        }

        private async void InitUnityAuth() {
            if (UnityServices.State == ServicesInitializationState.Initialized) {
                return;
            }
            var options = new InitializationOptions();
            options.SetProfile(Guid.NewGuid().ToString()[..30]);
            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public async void Create(string lobbyName, bool isPrivate) {
            try {
                var options = new CreateLobbyOptions { IsPrivate = isPrivate };
                joined = await LobbyService.Instance.CreateLobbyAsync(lobbyName, NetworkService.MAX_PLAYERS, options);
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }

        public async void QuickJoin() {
            try {
                joined = await LobbyService.Instance.QuickJoinLobbyAsync();
                NetworkService.Instance.StartClient();
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }
}
