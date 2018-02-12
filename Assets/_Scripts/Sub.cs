using System.Collections;
using System.Linq;
using Depth.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

namespace Depth
{
    public class Sub : MonoBehaviour
    {
        public bool IsDestroyed { get; private set; }
        public CanBeLockedOnTo Targetable { get; set; }
        public Taggable Taggable { get; private set; }
        public SubBody Body { get; private set; }
        public GamePadState InputState { get; private set; }
        public int Kills { get; private set; }
        public Camera SubCamera { get; private set; }
        public SubModifiers Modifiers { get; private set; }
        public SubParameters Parameters { get; private set; }
        public int InputPlayer { get; set; }
        public Ability[] Abilities { get; private set; }

        public PlayerCam Cam;

        /// <summary>0-indexed</summary>
        public int Player;
        public GameObject KillPrefab;
        public float AimDistance = 30;
        public RectTransform AimReticule;
        public GameObject ExplosionPrefab;
        public float RespawnDelay = 3;

        public PlayerPanel Panel;

        private float maxSubHeight;

        public void Awake()
        {
            IsDestroyed = true;
            Taggable = GetComponent<Taggable>();
            Targetable = GetComponent<CanBeLockedOnTo>();

            Targetable.OnHitByTorpedo += BlowUp;
            Targetable.OnHitByMine += BlowUp;
            Targetable.OnLockedOn += OnLockedOn;
            Targetable.OnLockedOff += OnLockedOff;

            Body = GetComponentInChildren<SubBody>();
            Modifiers = GetComponentInChildren<SubModifiers>();
            Parameters = Modifiers.GetParameters();
            Abilities = GetComponentsInChildren<Ability>();

            InputPlayer = Player;

            SubCamera = Cam.GetComponent<Camera>();
            IsDestroyed = true;

            maxSubHeight = GameObject.FindGameObjectWithTag("Ceiling").transform.position.y;
        }

        public void Start()
        {
            GameplayManager.Instance.GameStarted += OnGameStarted;
            SubManager.Instance.Subs.Add(this);

            var ability = Abilities.FirstOrDefault();
            if (ability != null)
            {
                Panel.AbilityIndicator.SetIcon(ability.Icon);
            }

            Respawn();
        }

        public bool HasAbility(string ability)
        {
            return Abilities != null && Abilities.Any(x => x.Name == ability);
        }

        private void OnLockedOff(CanBeLockedOnTo canBeLockedOnTo)
        {
            if (canBeLockedOnTo.CurrentLocks == 0)
            {
                Panel.LockedOnWarning.Hide();
            }
        }

        private void OnLockedOn(ICanLockOn canLockOn)
        {
            Panel.LockedOnWarning.Show();
        }

        private void OnGameStarted()
        {
            Kills = 0;
            foreach (Transform child in Panel.KillIndicator.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void GotKill()
        {
            Kills++;

            Instantiate(KillPrefab, Panel.KillIndicator.transform);
        }

        private void BlowUp(Mine mine)
        {
            BlowUp(mine.Parent.Player);
        }

        private void BlowUp(Torpedo torpedo)
        {
            BlowUp(torpedo.Parent.Player);
        }

        private void BlowUp(int playerCaused)
        {
            if (IsDestroyed)
                return;

            Instantiate(ExplosionPrefab, transform.position, transform.rotation);

            GetComponentInChildren<SubBody>(true).gameObject.SetActive(false);
            IsDestroyed = true;

            Panel.DeadPanel.gameObject.SetActive(true);

            if (playerCaused != Player)
                SubManager.Instance.GetSub(playerCaused).GotKill();

            // explosion of ship is done in the mine or torpedo
            // SoundManager.PlaySound("explosion_far1");

            StartCoroutine(RespawnAfterDelay());
        }

        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(RespawnDelay);
            Respawn();
        }

        private void Respawn()
        {
            IsDestroyed = false;

            var spawnPosition = SpawnPoints.Instance.GetSpawnPoint(Player);
            transform.SetPositionAndRotation(spawnPosition.position, spawnPosition.rotation);

            GetComponentInChildren<SubBody>(true).gameObject.SetActive(true);
            Panel.DeadPanel.gameObject.SetActive(false);
        }

        public void Update()
        {
            InputState = GamePad.GetState((PlayerIndex)InputPlayer);
            
            AimReticule.anchoredPosition = Helpers.CameraSpaceToMultiplyerSpace(
                Helpers.WorldPointToScreenSpace(transform.position + (transform.forward * AimDistance), SubCamera));

            Parameters = Modifiers.GetParameters();
        }

        public void FixedUpdate()
        {
            if (transform.position.y > maxSubHeight)
                GetComponent<SubController>().HitCeiling();
        }

        public void OnDestroy()
        {
            SubManager.Instance.Subs.Remove(this);
        }

        public void OnCollisionEnter(Collision collision)
        {
            var otherSub = collision.gameObject.GetComponentInParent<Sub>();
            if (otherSub != null)
            {
                BlowUp(otherSub.Player);
            }
        }
    }
}