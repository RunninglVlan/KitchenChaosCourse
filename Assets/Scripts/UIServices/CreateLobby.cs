﻿using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public partial class CreateLobby : UIService {
        private TextField lobbyName = null!;
        private Toggle local = null!;

        void Start() {
            Hide();
            var root = document.rootVisualElement;
            lobbyName = root.Q<TextField>("lobby-name");
            local = root.Q<Toggle>("local");
            root.Q<Button>("create-private").clicked += CreatePrivate;
            root.Q<Button>("create-public").clicked += CreatePublic;
            root.Q<Button>("close").clicked += Hide;
            return;

            void CreatePrivate() {
                NetworkLobby.Instance.Create(lobbyName.value, true, local.value);
            }

            void CreatePublic() {
                NetworkLobby.Instance.Create(lobbyName.value, false, local.value);
            }
        }

        public override void Show() {
            base.Show();
            lobbyName.Focus();
        }
    }
}
