using UnityEngine;

namespace SoulGames.Examples
{
    public class ExampleUnityEvents : MonoBehaviour
    {
        [SerializeField]private bool showConsoleText = true;
        [Space]
        [SerializeField]private AudioClip audio_OnSelectedBuildableChanged;
        [SerializeField]private AudioClip audio_OnObjectPlaced;
        [SerializeField]private AudioClip audio_OnObjectRemoved;
        [SerializeField]private AudioClip audio_OnGridCellChanged;
        [SerializeField]private AudioClip audio_OnObjectSelected;
        [SerializeField]private AudioClip audio_OnObjectDeselected;
        [SerializeField]private AudioClip audio_OnActiveGridLevelChanged;
        
        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void OnSelectedBuildableChanged()
        {
            if (showConsoleText) Debug.Log("<color=green>Unity Event Fired :</color> On Selected Buildable Changed");
        }

        public void OnObjectPlaced()
        {
            if (showConsoleText) Debug.Log("<color=green>Unity Event Fired :</color> On Object Placed");
        }

        public void OnObjectRemoved()
        {
            if (showConsoleText) Debug.Log("<color=green>Unity Event Fired :</color> On Object Removed");
        }

        public void OnGridCellChanged()
        {
            if (showConsoleText) Debug.Log("<color=green>Unity Event Fired :</color> On Grid Cell Changed");
        }

        public void OnObjectSelected()
        {
            if (showConsoleText) Debug.Log("<color=green>Unity Event Fired :</color> On Object Selected");
        }

        public void OnObjectDeselected()
        {
            if (showConsoleText) Debug.Log("<color=green>Unity Event Fired :</color> On Object Deselected");
        }

        public void OnActiveGridLevelChanged()
        {
            if (showConsoleText) Debug.Log("<color=green>Unity Event Fired :</color> On Active Grid Level Changed");
        }

        public void Audio_OnSelectedBuildableChanged()
        {
            if (!audio_OnSelectedBuildableChanged) return;
            audioSource.pitch = Random.Range(1f, 1.5f);
            audioSource.clip = audio_OnSelectedBuildableChanged;
            audioSource.Play();
        }

        public void Audio_OnObjectPlaced()
        {
            if (!audio_OnObjectPlaced) return;
            audioSource.pitch = Random.Range(1f, 1.5f);
            audioSource.clip = audio_OnObjectPlaced;
            audioSource.Play();
        }
        
        public void Audio_OnObjectRemoved()
        {
            if (!audio_OnObjectRemoved) return;
            audioSource.pitch = Random.Range(1f, 1.5f);
            audioSource.clip = audio_OnObjectRemoved;
            audioSource.Play();
        }

        public void Audio_OnGridCellChanged()
        {
            if (!audio_OnGridCellChanged) return;
            audioSource.pitch = Random.Range(1f, 1.5f);
            audioSource.clip = audio_OnGridCellChanged;
            audioSource.Play();
        }

        public void Audio_OnObjectSelected()
        {
            if (!audio_OnObjectSelected) return;
            audioSource.pitch = Random.Range(1f, 1.5f);
            audioSource.clip = audio_OnObjectSelected;
            audioSource.Play();
        }

        public void Audio_OnObjectDeselected()
        {
            if (!audio_OnObjectDeselected) return;
            audioSource.pitch = Random.Range(1f, 1.5f);
            audioSource.clip = audio_OnObjectDeselected;
            audioSource.Play();
        }
        
        public void Audio_OnActiveGridLevelChanged()
        {
            if (!audio_OnActiveGridLevelChanged) return;
            audioSource.pitch = Random.Range(1f, 1.5f);
            audioSource.clip = audio_OnActiveGridLevelChanged;
            audioSource.Play();
        }
    }
}
