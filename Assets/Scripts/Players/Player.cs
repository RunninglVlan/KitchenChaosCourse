using System;
using KitchenChaos.Counters;
using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenChaos.Players {
    public partial class Player : NetworkBehaviour {
        private const float ROTATE_SPEED = 10;

        public event Action<Counter?> SelectedCounterChanged = delegate { };

        [SerializeField] private float speed = 1;
        [SerializeField] private float radius = .5f;
        [SerializeField] private LayerMask counterLayer;
        [SerializeField] private LayerMask collisionLayer;

        public bool IsWalking { get; private set; }
        private Vector3 interactDirection;
        private Counter? selectedCounter;

        void Start() {
            GameInput.Instance.Actions.Player.Interact.performed += Interact;
            GameInput.Instance.Actions.Player.InteractAlternate.performed += InteractAlternate;
        }

        void Update() {
            if (!IsOwner) {
                return;
            }
            var input = GameInput.Instance.Actions.Player.Move.ReadValue<Vector2>();
            Move(input);
            SelectCounter(input);
        }

        private void Move(Vector2 input) {
            var playerTransform = transform;
            if (CanMove(playerTransform.position, input, out var move)) {
                playerTransform.position += move * (speed * Time.deltaTime);
            }
            var direction = new Vector3(input.x, 0, input.y).normalized;
            var forward = Vector3.Slerp(playerTransform.forward, direction, ROTATE_SPEED * Time.deltaTime);
            if (forward != Vector3.zero) {
                playerTransform.forward = forward;
            }

            IsWalking = input != Vector2.zero;
        }

        private bool CanMove(Vector3 origin, Vector2 input, out Vector3 move) {
            if (input == Vector2.zero) {
                move = Vector3.zero;
                return false;
            }
            move = new Vector3(input.x, 0, input.y).normalized;
            if (IsAllowed(move)) {
                return true;
            }
            move = new Vector3(input.x, 0, 0).normalized;
            if (input.x is < -.5f or > .5f && IsAllowed(move)) {
                return true;
            }
            move = new Vector3(0, 0, input.y).normalized;
            return input.y is < -.5f or > .5f && IsAllowed(move);

            bool IsAllowed(Vector3 direction) {
                const float capsuleMoveDistance = .1f;
                return !Physics.BoxCast(origin, Vector3.one * radius, direction,
                    Quaternion.identity, capsuleMoveDistance, collisionLayer);
            }
        }

        private void SelectCounter(Vector2 input) {
            var direction = new Vector3(input.x, 0, input.y).normalized;
            if (direction != Vector3.zero) {
                interactDirection = direction;
            }
            var interactDistance = radius + .1f;
            if (!Physics.Raycast(transform.position, interactDirection, out var hit, interactDistance, counterLayer)) {
                SetSelectedCounter(null);
                return;
            }
            if (!hit.transform.TryGetComponent<Counter>(out var counter)) {
                SetSelectedCounter(null);
                return;
            }
            SetSelectedCounter(counter);
        }

        private void SetSelectedCounter(Counter? counter) {
            if (selectedCounter == counter) {
                return;
            }
            selectedCounter = counter;
            SelectedCounterChanged(counter);
        }

        private void Interact(InputAction.CallbackContext _) {
            if (!GameService.Instance.IsPlaying || selectedCounter == null) {
                return;
            }
            selectedCounter.Interact(this);
        }

        private void InteractAlternate(InputAction.CallbackContext _) {
            if (!GameService.Instance.IsPlaying || selectedCounter == null) {
                return;
            }
            selectedCounter.InteractAlternate();
        }

        private void DestroyHoldingObject(ulong client) {
            if (client == OwnerClientId && TryGetKitchenObject(out var holdingObject)) {
                KitchenObjectService.Instance.Destroy(holdingObject);
            }
        }
    }
}
